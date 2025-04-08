using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Admin.Services;
using web.Areas.Admin.ViewModels.Project;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ProjectController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<ProjectViewModel> _validator;
    private readonly IMinioStorageService _minioService;
    private readonly ILogger<ProjectController> _logger;

    public ProjectController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<ProjectViewModel> validator,
        IMinioStorageService minioService,
        ILogger<ProjectController> logger)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
        _minioService = minioService;
        _logger = logger;
    }

    // GET: Admin/Project
    public async Task<IActionResult> Index(string? searchTerm = null, int? categoryId = null, ProjectStatus? status = null, PublishStatus? publishStatus = null)
    {
        ViewData["PageTitle"] = "Quản lý Dự án";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Dự án", "") };

        var query = _context.Set<Project>()
                            // Eager load data needed for list display
                            .Include(p => p.Images)
                            .AsQueryable();

        // Filtering
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p => p.Name.Contains(searchTerm) || p.Client.Contains(searchTerm) || p.Location.Contains(searchTerm));
        }
        if (categoryId.HasValue)
        {
            query = query.Where(p => p.ProjectCategories.Any(pc => pc.CategoryId == categoryId.Value));
        }
        if (status.HasValue)
        {
            query = query.Where(p => p.Status == status.Value);
        }
        if (publishStatus.HasValue)
        {
            query = query.Where(p => p.PublishStatus == publishStatus.Value);
        }

        var projects = await query.OrderByDescending(p => p.CompletionDate ?? p.StartDate ?? p.CreatedAt).ToListAsync();
        var viewModels = _mapper.Map<List<ProjectListItemViewModel>>(projects);

        // Load filter data
        ViewBag.Categories = await _context.Set<Category>().Where(c => c.Type == CategoryType.Project && c.IsActive).OrderBy(c => c.Name).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToListAsync();
        ViewBag.Statuses = Enum.GetValues(typeof(ProjectStatus)).Cast<ProjectStatus>().Select(s => new SelectListItem { Value = s.ToString(), Text = GetStatusDisplayName(s) }).ToList();
        ViewBag.PublishStatuses = Enum.GetValues(typeof(PublishStatus)).Cast<PublishStatus>().Select(ps => new SelectListItem { Value = ps.ToString(), Text = GetPublishStatusDisplayName(ps) }).ToList();

        ViewBag.SearchTerm = searchTerm;
        ViewBag.SelectedCategoryId = categoryId;
        ViewBag.SelectedStatus = status;
        ViewBag.SelectedPublishStatus = publishStatus;

        return View(viewModels);
    }


    // GET: Admin/Project/Create
    public async Task<IActionResult> Create()
    {
        ViewData["PageTitle"] = "Thêm Dự án mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Dự án", Url.Action(nameof(Index))), ("Thêm mới", "") };

        var viewModel = new ProjectViewModel
        {
            PublishStatus = PublishStatus.Draft,
            Status = ProjectStatus.Planning,
            SitemapPriority = 0.6,
            SitemapChangeFrequency = "monthly",
            OgType = "object" // Adjust if there's a better type
        };

        await LoadDropdownsAsync(viewModel);
        return View(viewModel);
    }

    // POST: Admin/Project/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProjectViewModel viewModel)
    {
        if (await _context.Set<Project>().AnyAsync(p => p.Slug == viewModel.Slug))
        {
            ModelState.AddModelError(nameof(ProjectViewModel.Slug), "Slug đã tồn tại.");
        }
        // Check Main Image
        if (viewModel.Images != null && viewModel.Images.Any(i => !i.IsDeleted) && !viewModel.Images.Any(i => i.IsMain && !i.IsDeleted))
        {
            ModelState.AddModelError("Images", "Vui lòng chọn một ảnh làm ảnh đại diện chính.");
        }
        // Check Product association validity (optional but good)
        var productIds = viewModel.ProjectProducts?.Where(pp => !pp.IsDeleted).Select(pp => pp.ProductId).ToList() ?? new List<int>();
        if (productIds.Any())
        {
            var validProductCount = await _context.Products.CountAsync(p => productIds.Contains(p.Id));
            if (validProductCount != productIds.Count)
            {
                ModelState.AddModelError("ProjectProducts", "Một hoặc nhiều sản phẩm liên kết không hợp lệ.");
            }
        }


        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid || !ModelState.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            _logger.LogWarning("Project creation validation failed.");
            await LoadDropdownsAsync(viewModel);
            ViewData["PageTitle"] = "Thêm Dự án mới";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Dự án", Url.Action(nameof(Index))), ("Thêm mới", "") };
            return View(viewModel);
        }

        var project = _mapper.Map<Project>(viewModel);

        // --- MANUAL RELATIONSHIP HANDLING ---
        project.ProjectCategories = viewModel.SelectedCategoryIds.Select(catId => new ProjectCategory { CategoryId = catId }).ToList();
        project.ProjectTags = viewModel.SelectedTagIds.Select(tagId => new ProjectTag { TagId = tagId }).ToList();
        project.Images = _mapper.Map<List<ProjectImage>>(viewModel.Images.Where(i => !i.IsDeleted));
        project.ProjectProducts = _mapper.Map<List<ProjectProduct>>(viewModel.ProjectProducts.Where(pp => !pp.IsDeleted));


        _context.Projects.Add(project);

        try
        {
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Thêm dự án thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error saving new project.");
            ModelState.AddModelError("", "Không thể lưu dự án. Vui lòng kiểm tra lại dữ liệu.");
            await LoadDropdownsAsync(viewModel);
            ViewData["PageTitle"] = "Thêm Dự án mới";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Dự án", Url.Action(nameof(Index))), ("Thêm mới", "") };
            return View(viewModel);
        }
    }


    // GET: Admin/Project/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var project = await _context.Set<Project>()
            .Include(p => p.Images.OrderBy(i => i.OrderIndex))
            .Include(p => p.ProjectCategories)
            .Include(p => p.ProjectTags)
            .Include(p => p.ProjectProducts).ThenInclude(pp => pp.Product).ThenInclude(prod => prod.Images) // Include Product and its images for display
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project == null) return NotFound();

        var viewModel = _mapper.Map<ProjectViewModel>(project);
        await LoadDropdownsAsync(viewModel);

        ViewData["PageTitle"] = "Chỉnh sửa Dự án";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Dự án", Url.Action(nameof(Index))), ("Chỉnh sửa", "") };

        return View(viewModel);
    }

    // POST: Admin/Project/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProjectViewModel viewModel)
    {
        if (id != viewModel.Id) return BadRequest();

        // --- Manual Checks ---
        if (await _context.Set<Project>().AnyAsync(p => p.Slug == viewModel.Slug && p.Id != id))
        {
            ModelState.AddModelError(nameof(ProjectViewModel.Slug), "Slug đã tồn tại.");
        }
        if (viewModel.Images != null && viewModel.Images.Any(i => !i.IsDeleted) && !viewModel.Images.Any(i => i.IsMain && !i.IsDeleted))
        {
            ModelState.AddModelError("Images", "Vui lòng chọn một ảnh làm ảnh đại diện chính.");
        }
        var productIds = viewModel.ProjectProducts?.Where(pp => !pp.IsDeleted).Select(pp => pp.ProductId).ToList() ?? new List<int>();
        if (productIds.Any())
        {
            var validProductCount = await _context.Products.CountAsync(p => productIds.Contains(p.Id));
            if (validProductCount != productIds.Count)
            {
                ModelState.AddModelError("ProjectProducts", "Một hoặc nhiều sản phẩm liên kết không hợp lệ.");
            }
        }


        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid || !ModelState.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            _logger.LogWarning("Project editing validation failed for ID: {ProjectId}", id);
            await LoadDropdownsAsync(viewModel);
            // Need to reload Product names/images if returning view after validation fail
            // This part is tricky without re-fetching or storing more in the viewmodel
            var existingProductData = await _context.ProjectProducts
               .Where(pp => pp.ProjectId == id)
               .Include(pp => pp.Product).ThenInclude(p => p.Images)
               .ToListAsync();
            viewModel.ProjectProducts = _mapper.Map<List<ProjectProductViewModel>>(existingProductData);
            ViewData["PageTitle"] = "Chỉnh sửa Dự án";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Dự án", Url.Action(nameof(Index))), ("Chỉnh sửa", "") };
            return View(viewModel);
        }

        var project = await _context.Set<Project>()
            .Include(p => p.Images)
            .Include(p => p.ProjectCategories)
            .Include(p => p.ProjectTags)
            .Include(p => p.ProjectProducts)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project == null) return NotFound();

        // --- Map Scalar Properties ---
        _mapper.Map(viewModel, project);

        // --- MANUAL RELATIONSHIP HANDLING ---
        // Categories & Tags (Same as Article)
        UpdateJunctionTable(project.ProjectCategories, viewModel.SelectedCategoryIds, id, (catId) => new ProjectCategory { ProjectId = id, CategoryId = catId }, pc => pc.CategoryId);
        UpdateJunctionTable(project.ProjectTags, viewModel.SelectedTagIds, id, (tagId) => new ProjectTag { ProjectId = id, TagId = tagId }, pt => pt.TagId);

        // Images (Same as Product)
        var imagesToDeleteFromMinio = UpdateImages(project.Images, viewModel.Images, id);

        // ProjectProducts (More complex due to 'Usage' field)
        UpdateProjectProducts(project.ProjectProducts, viewModel.ProjectProducts, id);


        try
        {
            await _context.SaveChangesAsync(); // Save DB changes

            // Delete images from MinIO AFTER successful DB save
            await DeleteMinioFilesAsync(imagesToDeleteFromMinio, $"Project ID {id}");

            TempData["SuccessMessage"] = "Cập nhật dự án thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ProjectExists(id)) return NotFound();
            _logger.LogWarning("Concurrency conflict updating project ID: {ProjectId}", id);
            TempData["ErrorMessage"] = "Lỗi: Xung đột dữ liệu khi cập nhật.";
            await LoadDropdownsAsync(viewModel);
            return View(viewModel);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error updating project ID: {ProjectId}", id);
            ModelState.AddModelError("", "Không thể lưu dự án. Vui lòng kiểm tra lại dữ liệu.");
            await LoadDropdownsAsync(viewModel);
            return View(viewModel);
        }
    }


    // POST: Admin/Project/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var project = await _context.Set<Project>()
                                 .Include(p => p.Images)
                                 .FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
        {
            return Json(new { success = false, message = "Không tìm thấy dự án." });
        }

        // Store image paths BEFORE deleting from DB
        var imagesToDeleteFromMinio = project.Images?
           .SelectMany(i => new[] { i.ImageUrl, i.ThumbnailUrl }) // Get both paths
           .Where(url => !string.IsNullOrEmpty(url))
           .Distinct()
           .ToList() ?? new List<string>();


        _context.Projects.Remove(project); // Cascade should handle junction tables

        try
        {
            await _context.SaveChangesAsync(); // Delete from DB

            // Delete MinIO files AFTER successful DB save
            await DeleteMinioFilesAsync(imagesToDeleteFromMinio, $"Project ID {id}");

            return Json(new { success = true, message = "Xóa dự án thành công." });
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error deleting Project ID {ProjectId}. Check RESTRICT constraints.", id);
            return Json(new { success = false, message = "Không thể xóa dự án. Có thể có lỗi ràng buộc dữ liệu." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting Project ID {ProjectId}.", id);
            return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa dự án." });
        }
    }


    // --- Helper Methods ---
    private async Task LoadDropdownsAsync(ProjectViewModel viewModel)
    {
        viewModel.CategoryList = new SelectList(await _context.Set<Category>().Where(c => c.IsActive && c.Type == CategoryType.Project).OrderBy(c => c.Name).ToListAsync(), "Id", "Name");
        viewModel.TagList = new SelectList(await _context.Set<Tag>().Where(t => t.Type == TagType.Project).OrderBy(t => t.Name).ToListAsync(), "Id", "Name");
        // Load ALL active products for selection
        viewModel.ProductList = new SelectList(await _context.Set<Product>().Where(p => p.IsActive && p.Status == PublishStatus.Published).OrderBy(p => p.Name).ToListAsync(), "Id", "Name");
        viewModel.StatusList = new SelectList(Enum.GetValues(typeof(ProjectStatus)).Cast<ProjectStatus>().Select(e => new { Value = e, Text = GetStatusDisplayName(e) }), "Value", "Text", viewModel.Status);
        viewModel.PublishStatusList = new SelectList(Enum.GetValues(typeof(PublishStatus)).Cast<PublishStatus>().Select(e => new { Value = e, Text = GetPublishStatusDisplayName(e) }), "Value", "Text", viewModel.PublishStatus);
    }

    // Generic helper used by ArticleController too
    private void UpdateJunctionTable<TEntity, TKey>(ICollection<TEntity> currentCollection, IEnumerable<TKey> selectedIds, int parentId, Func<TKey, TEntity> createEntity, Func<TEntity, TKey> getKey) where TEntity : class where TKey : IEquatable<TKey> { /* ... same as ArticleController ... */ }

    // Specific helper for ProjectProducts due to 'Usage' field
    private void UpdateProjectProducts(ICollection<ProjectProduct> currentProducts, List<ProjectProductViewModel> productVMs, int projectId)
    {
        var vmsNotDeleted = productVMs.Where(vm => !vm.IsDeleted).ToList();
        var currentProductIds = currentProducts.Select(pp => pp.ProductId).ToList();
        var vmProductIds = vmsNotDeleted.Select(vm => vm.ProductId).ToList();

        // Remove products no longer selected
        var productsToRemove = currentProducts.Where(pp => !vmProductIds.Contains(pp.ProductId)).ToList();
        _context.ProjectProducts.RemoveRange(productsToRemove);

        // Update existing or Add new ones
        foreach (var vm in vmsNotDeleted)
        {
            var existing = currentProducts.FirstOrDefault(pp => pp.ProductId == vm.ProductId);
            if (existing != null) // Update Usage and OrderIndex
            {
                existing.Usage = vm.Usage;
                existing.OrderIndex = vm.OrderIndex;
                // No need to use Mapper here unless more fields are involved
            }
            else // Add new association
            {
                var newProjectProduct = _mapper.Map<ProjectProduct>(vm); // Use mapper for consistency
                newProjectProduct.ProjectId = projectId;
                // Use Add method of DbContext or add to navigation property if loaded
                _context.ProjectProducts.Add(newProjectProduct);
                // If project.ProjectProducts was loaded: project.ProjectProducts.Add(newProjectProduct);
            }
        }
    }


    // Specific helper for Images similar to ProductController
    private List<string> UpdateImages(ICollection<ProjectImage> currentImages, List<ProjectImageViewModel> imageVMs, int projectId)
    {
        var imagesToDeleteFromMinio = new List<string>();
        var vmsNotDeleted = imageVMs.Where(vm => !vm.IsDeleted).ToList();
        var currentImageIds = currentImages.Select(i => i.Id).ToList();
        var vmImageIds = vmsNotDeleted.Where(vm => vm.Id > 0).Select(vm => vm.Id).ToList(); // IDs of existing images in VM

        // Remove images not present in the final VM list
        var imagesToRemove = currentImages.Where(dbImg => !vmImageIds.Contains(dbImg.Id)).ToList();
        foreach (var imgToRemove in imagesToRemove)
        {
            if (!string.IsNullOrEmpty(imgToRemove.ImageUrl)) imagesToDeleteFromMinio.Add(imgToRemove.ImageUrl);
            if (!string.IsNullOrEmpty(imgToRemove.ThumbnailUrl) && imgToRemove.ThumbnailUrl != imgToRemove.ImageUrl) imagesToDeleteFromMinio.Add(imgToRemove.ThumbnailUrl);
            _context.ProjectImages.Remove(imgToRemove);
        }

        // Update existing or add new images
        foreach (var vm in vmsNotDeleted)
        {
            if (vm.Id > 0)
            { // Update existing
                var dbImage = currentImages.FirstOrDefault(i => i.Id == vm.Id);
                if (dbImage != null) _mapper.Map(vm, dbImage);
            }
            else
            { // Add new
                var newImage = _mapper.Map<ProjectImage>(vm);
                newImage.ProjectId = projectId;
                currentImages.Add(newImage); // Add via navigation property
            }
        }
        return imagesToDeleteFromMinio.Distinct().ToList();
    }

    // Helper to delete MinIO files safely
    private async Task DeleteMinioFilesAsync(List<string> pathsToDelete, string contextInfo)
    {
        foreach (var path in pathsToDelete)
        {
            if (string.IsNullOrEmpty(path)) continue;
            try
            {
                await _minioService.DeleteFileAsync(path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete file '{ImagePath}' from MinIO for {ContextInfo}.", path, contextInfo);
            }
        }
    }


    private string GetStatusDisplayName(ProjectStatus status)
    {
        return status switch
        {
            ProjectStatus.Planning => "Lập kế hoạch",
            ProjectStatus.InProgress => "Đang tiến hành",
            ProjectStatus.Completed => "Hoàn thành",
            ProjectStatus.OnHold => "Tạm dừng",
            _ => status.ToString()
        };
    }
    private string GetPublishStatusDisplayName(PublishStatus status)
    {
        return status switch
        {
            PublishStatus.Draft => "Nháp",
            PublishStatus.Published => "Đã xuất bản",
            PublishStatus.Scheduled => "Đã lên lịch",
            PublishStatus.Archived => "Đã lưu trữ",
            _ => status.ToString()
        };
    }
    private async Task<bool> ProjectExists(int id) => await _context.Set<Project>().AnyAsync(e => e.Id == id);
}