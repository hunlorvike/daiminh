using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Extensions;
using web.Areas.Admin.Services;
using web.Areas.Admin.ViewModels.Project;
using X.PagedList.EF;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ProjectController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ProjectController> _logger;
    private readonly IMinioStorageService _minioService;

    public ProjectController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<ProjectController> logger,
        IMinioStorageService minioService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _minioService = minioService ?? throw new ArgumentNullException(nameof(minioService));
    }

    // GET: Admin/Project
    public async Task<IActionResult> Index(string? searchTerm = null, int? categoryId = null, ProjectStatus? status = null, PublishStatus? publishStatus = null, int page = 1, int pageSize = 15)
    {
        ViewData["Title"] = "Quản lý Dự án - Hệ thống quản trị";
        ViewData["PageTitle"] = "Danh sách Dự án";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Dự án", Url.Action(nameof(Index))) };

        int pageNumber = page;
        var query = _context.Set<Project>()
                            .Include(p => p.Category)
                            .Include(p => p.Images.OrderBy(i => i.OrderIndex))
                            .AsNoTracking();

        // Filtering
        if (!string.IsNullOrWhiteSpace(searchTerm)) { string st = searchTerm.Trim().ToLower(); query = query.Where(p => p.Name.ToLower().Contains(st) || (p.Client != null && p.Client.ToLower().Contains(st)) || (p.Location != null && p.Location.ToLower().Contains(st))); }
        if (categoryId.HasValue && categoryId > 0) { query = query.Where(pc => pc.CategoryId == categoryId.Value); }
        if (status.HasValue) { query = query.Where(p => p.Status == status.Value); }
        if (publishStatus.HasValue) { query = query.Where(p => p.PublishStatus == publishStatus.Value); }

        // Sorting & Pagination
        var projectsPaged = await query
            .OrderByDescending(p => p.IsFeatured)
            .ThenByDescending(p => p.CompletionDate ?? p.StartDate ?? p.CreatedAt) // Order by completion, start, or creation
            .ProjectTo<ProjectListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        await LoadFilterDropdownsAsync(categoryId, status, publishStatus);
        ViewBag.SearchTerm = searchTerm;

        return View(projectsPaged);
    }

    // GET: Admin/Project/Create
    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Thêm Dự án mới - Hệ thống quản trị";
        ViewData["PageTitle"] = "Thêm Dự án mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Dự án", Url.Action(nameof(Index))), ("Thêm mới", "") };

        var viewModel = new ProjectViewModel
        {
            Status = ProjectStatus.InProgress,
            PublishStatus = PublishStatus.Draft,
            SitemapPriority = 0.7,
            SitemapChangeFrequency = "monthly",
            OgType = "article", // Adjust OgType if needed
            Images = new List<ProjectImageViewModel>(), // Initialize lists
            SelectedTagIds = new List<int>(),
            SelectedProductIds = new List<int>()
        };
        await LoadRelatedDataForFormAsync(viewModel);
        return View(viewModel);
    }

    // POST: Admin/Project/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProjectViewModel viewModel)
    {
        await ValidateRelationsAndUniquenessAsync(viewModel, isEdit: false);

        if (ModelState.IsValid)
        {
            var project = _mapper.Map<Project>(viewModel);

            // Handle Relationships
            project.ProjectTags = viewModel.SelectedTagIds?.Select(id => new ProjectTag { TagId = id }).ToList() ?? new();
            project.ProjectProducts = viewModel.SelectedProductIds?.Select((id, idx) => new ProjectProduct { ProductId = id, OrderIndex = idx }).ToList() ?? new();
            project.Images = _mapper.Map<List<ProjectImage>>(viewModel.Images?.Where(i => !i.IsDeleted).ToList() ?? new());

            // Set audit fields
            // project.CreatedBy = User.Identity?.Name;

            _context.Projects.Add(project);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Project '{Name}' (ID: {ProjectId}) created successfully by {User}.", project.Name, project.Id, User.Identity?.Name ?? "Unknown");
                TempData["success"] = $"Thêm dự án '{project.Name}' thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex) { HandleDbUpdateException(ex, viewModel.Slug); }
            catch (Exception ex) { _logger.LogError(ex, "Error creating project '{Name}'.", viewModel.Name); ModelState.AddModelError("", "Lỗi không mong muốn khi tạo dự án."); }
        }
        else { _logger.LogWarning("Project creation failed for '{Name}'. Model state is invalid.", viewModel.Name); }

        await LoadRelatedDataForFormAsync(viewModel);
        ViewData["Title"] = "Thêm Dự án mới - Hệ thống quản trị"; ViewData["PageTitle"] = "Thêm Dự án mới"; ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Dự án", Url.Action(nameof(Index))), ("Thêm mới", "") };
        return View(viewModel);
    }


    // GET: Admin/Project/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var project = await _context.Set<Project>()
            .Include(p => p.Images.OrderBy(i => i.OrderIndex))
            .Include(p => p.Category)
            .Include(p => p.ProjectTags)
            .Include(p => p.ProjectProducts)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project == null) { _logger.LogWarning("Edit GET: Project ID {ProjectId} not found.", id); return NotFound(); }

        var viewModel = _mapper.Map<ProjectViewModel>(project);
        await LoadRelatedDataForFormAsync(viewModel);

        ViewData["Title"] = "Chỉnh sửa Dự án - Hệ thống quản trị"; ViewData["PageTitle"] = $"Chỉnh sửa Dự án: {project.Name}"; ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Dự án", Url.Action(nameof(Index))), ($"Chỉnh sửa: {Truncate(project.Name)}", "") };
        return View(viewModel);
    }

    // POST: Admin/Project/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProjectViewModel viewModel)
    {
        if (id != viewModel.Id) { _logger.LogWarning("Edit POST: ID mismatch. Route ID: {RouteId}, ViewModel ID: {ViewModelId}", id, viewModel.Id); return BadRequest(); }

        await ValidateRelationsAndUniquenessAsync(viewModel, isEdit: true);

        if (ModelState.IsValid)
        {
            var project = await _context.Set<Project>()
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.ProjectTags)
                .Include(p => p.ProjectProducts)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null) { _logger.LogWarning("Edit POST: Project ID {ProjectId} not found for update.", id); TempData["error"] = "Không tìm thấy dự án."; return RedirectToAction(nameof(Index)); }

            _mapper.Map(viewModel, project); // Map scalar/SEO properties

            try
            {
                // --- Update Relationships ---
                UpdateJunctionTable(project.ProjectTags, viewModel.SelectedTagIds, id, (tagId) => new ProjectTag { ProjectId = id, TagId = tagId }, pt => pt.TagId);
                UpdateJunctionTable(project.ProjectProducts, viewModel.SelectedProductIds, id, (prodId) => new ProjectProduct { ProjectId = id, ProductId = prodId }, pp => pp.ProductId);
                UpdateProjectImages(project, viewModel.Images);

                // Set audit fields
                // project.UpdatedBy = User.Identity?.Name;

                await _context.SaveChangesAsync();
                _logger.LogInformation("Project ID {ProjectId} ('{Name}') updated successfully by {User}.", id, project.Name, User.Identity?.Name ?? "Unknown");
                TempData["success"] = $"Cập nhật dự án '{project.Name}' thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex) { _logger.LogWarning(ex, "Concurrency error updating project ID: {ProjectId}", id); TempData["error"] = "Lỗi: Xung đột dữ liệu."; }
            catch (DbUpdateException ex) { HandleDbUpdateException(ex, viewModel.Slug); }
            catch (Exception ex) { _logger.LogError(ex, "Error updating project ID: {ProjectId}", id); ModelState.AddModelError("", "Lỗi không mong muốn khi cập nhật."); }
        }
        else { _logger.LogWarning("Project editing failed for ID: {ProjectId}. Model state is invalid.", id); }

        await LoadRelatedDataForFormAsync(viewModel);
        ViewData["Title"] = "Chỉnh sửa Dự án - Hệ thống quản trị"; ViewData["PageTitle"] = $"Chỉnh sửa Dự án: {viewModel.Name}"; ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Dự án", Url.Action(nameof(Index))), ($"Chỉnh sửa: {Truncate(viewModel.Name)}", "") };
        return View(viewModel);
    }


    // POST: Admin/Project/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var project = await _context.Set<Project>().Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
        if (project == null) { _logger.LogWarning("Delete POST: Project ID {ProjectId} not found.", id); return Json(new { success = false, message = "Không tìm thấy dự án." }); }

        try
        {
            string name = project.Name;
            var imagesToDelete = project.Images?.Select(i => i.ImageUrl).Where(url => !string.IsNullOrEmpty(url)).Distinct().ToList() ?? new();
            imagesToDelete.AddRange(project.Images?.Select(i => i.ThumbnailUrl).Where(url => !string.IsNullOrEmpty(url)).Distinct().ToList() ?? new());
            if (!string.IsNullOrEmpty(project.FeaturedImage)) imagesToDelete.Add(project.FeaturedImage);
            if (!string.IsNullOrEmpty(project.ThumbnailImage)) imagesToDelete.Add(project.ThumbnailImage);
            // Add OG/Twitter images if they are paths
            if (!string.IsNullOrEmpty(project.OgImage) && !project.OgImage.StartsWith("http")) imagesToDelete.Add(project.OgImage);
            if (!string.IsNullOrEmpty(project.TwitterImage) && !project.TwitterImage.StartsWith("http")) imagesToDelete.Add(project.TwitterImage);

            _context.Projects.Remove(project); // Cascade handles join tables & ProjectImages DB entries
            await _context.SaveChangesAsync();
            _logger.LogInformation("Project '{Name}' (ID: {ProjectId}) deleted successfully by {User}.", name, id, User.Identity?.Name ?? "Unknown");

            // Delete images from storage AFTER successful DB delete
            foreach (var path in imagesToDelete.Distinct())
            {
                if (string.IsNullOrEmpty(path)) continue;
                try { await _minioService.DeleteFileAsync(path); _logger.LogInformation("Deleted image '{ImagePath}' from MinIO for deleted Project ID {ProjectId}.", path, id); }
                catch (Exception minioEx) { _logger.LogError(minioEx, "Failed to delete image '{ImagePath}' from MinIO for Project ID {ProjectId}.", path, id); }
            }

            return Json(new { success = true, message = $"Xóa dự án '{name}' thành công." });
        }
        catch (DbUpdateException ex) { _logger.LogError(ex, "Error deleting Project ID {ProjectId}. Potential FK constraint.", id); return Json(new { success = false, message = "Lỗi cơ sở dữ liệu khi xóa." }); }
        catch (Exception ex) { _logger.LogError(ex, "Error deleting Project ID {ProjectId}.", id); return Json(new { success = false, message = "Lỗi không mong muốn khi xóa." }); }
    }


    // --- Helper Methods ---
    private async Task LoadFilterDropdownsAsync(int? categoryId, ProjectStatus? status, PublishStatus? publishStatus)
    { /* Similar to Article, load Project Categories, ProjectStatus, PublishStatus */
        ViewBag.Categories = await _context.Set<Category>().Where(c => c.Type == CategoryType.Project && c.IsActive).OrderBy(c => c.Name).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name, Selected = c.Id == categoryId }).ToListAsync();
        ViewBag.ProjectStatuses = Enum.GetValues(typeof(ProjectStatus)).Cast<ProjectStatus>().Select(s => new SelectListItem { Value = s.ToString(), Text = s.GetDisplayName(), Selected = s == status }).ToList();
        ViewBag.PublishStatuses = Enum.GetValues(typeof(PublishStatus)).Cast<PublishStatus>().Select(s => new SelectListItem { Value = s.ToString(), Text = s.GetDisplayName(), Selected = s == publishStatus }).ToList();
        ViewBag.SelectedCategoryId = categoryId;
        ViewBag.SelectedStatus = status;
        ViewBag.SelectedPublishStatus = publishStatus;
    }
    private async Task LoadRelatedDataForFormAsync(ProjectViewModel viewModel)
    { /* Load Categories, Tags, Products, Statuses */
        viewModel.CategoryList = new SelectList(await _context.Set<Category>().Where(c => c.IsActive && c.Type == CategoryType.Project).OrderBy(c => c.Name).Select(c => new { c.Id, c.Name }).ToListAsync(), "Id", "Name");
        viewModel.TagList = new SelectList(await _context.Set<Tag>().Where(t => t.Type == TagType.Project).OrderBy(t => t.Name).Select(t => new { t.Id, t.Name }).ToListAsync(), "Id", "Name");
        viewModel.ProductList = new SelectList(await _context.Set<Product>().Where(p => p.IsActive && p.Status == PublishStatus.Published).OrderBy(p => p.Name).Select(p => new { p.Id, p.Name }).ToListAsync(), "Id", "Name");
        viewModel.StatusList = new SelectList(Enum.GetValues(typeof(ProjectStatus)).Cast<ProjectStatus>().Select(e => new { Value = e, Text = e.GetDisplayName() }), "Value", "Text", viewModel.Status);
        viewModel.PublishStatusList = new SelectList(Enum.GetValues(typeof(PublishStatus)).Cast<PublishStatus>().Select(e => new { Value = e, Text = e.GetDisplayName() }), "Value", "Text", viewModel.PublishStatus);
    }
    private async Task ValidateRelationsAndUniquenessAsync(ProjectViewModel viewModel, bool isEdit)
    { /* Only unique slug check needed here */
        if (!string.IsNullOrWhiteSpace(viewModel.Slug))
        {
            bool slugExists = await _context.Set<Project>().AnyAsync(p => p.Slug.ToLower() == viewModel.Slug.ToLower() && p.Id != viewModel.Id);
            if (slugExists) { ModelState.AddModelError(nameof(viewModel.Slug), "Slug này đã tồn tại."); }
        }
        // Add main image check similar to Product
        if (viewModel.Images != null && viewModel.Images.Any(i => !i.IsDeleted) && !viewModel.Images.Any(i => i.IsMain && !i.IsDeleted)) { ModelState.AddModelError("Images", "Vui lòng chọn một ảnh làm ảnh đại diện chính."); }
        if (viewModel.Images != null && viewModel.Images.Count(i => i.IsMain && !i.IsDeleted) > 1) { ModelState.AddModelError("Images", "Chỉ được chọn một ảnh làm ảnh đại diện chính."); }
    }
    private void UpdateJunctionTable<TEntity, TKey>(ICollection<TEntity> currentCollection, IEnumerable<TKey>? selectedIds, int parentId, Func<TKey, TEntity> createEntity, Func<TEntity, TKey> getKey)
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        selectedIds ??= new List<TKey>(); // Ensure list is not null

        var currentIds = currentCollection.Select(getKey).ToList();
        var idsToAdd = selectedIds.Except(currentIds).ToList();
        var entitiesToRemove = currentCollection.Where(e => !selectedIds.Contains(getKey(e))).ToList();

        if (entitiesToRemove.Any()) _context.RemoveRange(entitiesToRemove);

        if (idsToAdd.Any())
        {
            foreach (var idToAdd in idsToAdd)
            {
                currentCollection.Add(createEntity(idToAdd));
            }
        }
    }

    private void UpdateProjectImages(Project project, List<ProjectImageViewModel>? imageVMs)
    {
        imageVMs ??= new List<ProjectImageViewModel>();

        // Delete images marked for deletion in VM or not present in VM anymore
        var vmImageIds = imageVMs.Where(i => i.Id > 0).Select(i => i.Id).ToList();
        var dbImagesToRemove = project.Images
            .Where(dbImg => !vmImageIds.Contains(dbImg.Id) || imageVMs.First(vm => vm.Id == dbImg.Id).IsDeleted)
            .ToList();

        if (dbImagesToRemove.Any())
        {
            _context.ProjectImages.RemoveRange(dbImagesToRemove);
        }

        // Update existing or add new images
        foreach (var imgVm in imageVMs.Where(i => !i.IsDeleted))
        {
            if (imgVm.Id > 0) // Update existing
            {
                var dbImage = project.Images.FirstOrDefault(i => i.Id == imgVm.Id);
                if (dbImage != null)
                {
                    // Map only the editable fields from VM to existing DB entity
                    dbImage.AltText = imgVm.AltText;
                    dbImage.Title = imgVm.Title;
                    dbImage.OrderIndex = imgVm.OrderIndex;
                    dbImage.IsMain = imgVm.IsMain;
                    // Don't map ImageUrl/ThumbnailUrl here as they are set on upload
                }
            }
            else // Add new
            {
                // Only add if ImageUrl is actually set (meaning upload was successful or path provided)
                if (!string.IsNullOrWhiteSpace(imgVm.ImageUrl))
                {
                    var newImage = _mapper.Map<ProjectImage>(imgVm);
                    // ProjectId is set automatically by EF Core when adding to navigation property
                    project.Images.Add(newImage);
                }
                else
                {
                    _logger.LogWarning("Skipping adding new image because ImageUrl is empty. VM data: {@ImageViewModel}", imgVm);
                }
            }
        }
    }

    private void HandleDbUpdateException(DbUpdateException ex, string? slug)
    {
        _logger.LogError(ex, "DbUpdateException occurred.");
        if (ex.InnerException?.Message.Contains("UNIQUE constraint failed: projects.slug", StringComparison.OrdinalIgnoreCase) ?? false)
        {
            ModelState.AddModelError(nameof(ProjectViewModel.Slug), "Slug này đã tồn tại.");
        }
        else
        {
            ModelState.AddModelError("", "Lỗi cơ sở dữ liệu khi lưu.");
        }
    }
    private string Truncate(string value, int maxLength = 40)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
    }
}
// --- END OF FILE ProjectController.cs ---