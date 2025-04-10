using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Brand;
using X.PagedList.EF;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class BrandController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<BrandController> _logger;

    public BrandController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<BrandController> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: Admin/Brand
    public async Task<IActionResult> Index(string? searchTerm = null, int page = 1, int pageSize = 15)
    {
        ViewData["Title"] = "Quản lý Thương hiệu - Hệ thống quản trị";
        ViewData["PageTitle"] = "Danh sách Thương hiệu";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Thương hiệu", Url.Action(nameof(Index)) ?? string.Empty)
        };

        int pageNumber = page;

        var query = _context.Set<Brand>()
                            .Include(b => b.Products)
                            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string lowerSearchTerm = searchTerm.Trim().ToLower();
            query = query.Where(b => b.Name.ToLower().Contains(lowerSearchTerm)
                                  || (b.Website != null && b.Website.ToLower().Contains(lowerSearchTerm)));
        }

        var brandsPaged = await query
            .OrderBy(b => b.Name)
            .ProjectTo<BrandListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        ViewBag.SearchTerm = searchTerm;
        return View(brandsPaged);
    }

    // GET: Admin/Brand/Create
    public IActionResult Create()
    {
        ViewData["Title"] = "Thêm Thương hiệu mới - Hệ thống quản trị";
        ViewData["PageTitle"] = "Thêm Thương hiệu mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Thương hiệu", Url.Action(nameof(Index))?? string.Empty),
            ("Thêm mới", "") // Active
        };

        var viewModel = new BrandViewModel
        {
            IsActive = true,
            SitemapPriority = 0.5,
            SitemapChangeFrequency = "monthly",
            OgType = "website",
            TwitterCard = "summary_large_image"
        };

        return View(viewModel);
    }

    // POST: Admin/Brand/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BrandViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var brand = _mapper.Map<Brand>(viewModel);

                _context.Add(brand);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Brand '{BrandName}' created successfully by {User}.", brand.Name, User.Identity?.Name ?? "Unknown");
                TempData["success"] = $"Thêm thương hiệu '{brand.Name}' thành công!"; // Use "success" key
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating brand '{BrandName}'.", viewModel.Name);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi thêm thương hiệu. Vui lòng thử lại.");
            }
        }
        else
        {
            _logger.LogWarning("Failed to create brand '{BrandName}'. Model state is invalid.", viewModel.Name);
        }

        ViewData["Title"] = "Thêm Thương hiệu mới - Hệ thống quản trị";
        ViewData["PageTitle"] = "Thêm Thương hiệu mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Thương hiệu", Url.Action(nameof(Index)) ?? string.Empty), ("Thêm mới", "") };
        return View(viewModel);
    }

    // GET: Admin/Brand/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var brand = await _context.Set<Brand>().AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
        if (brand == null)
        {
            _logger.LogWarning("Edit GET: Brand with ID {BrandId} not found.", id);
            return NotFound();
        }

        var viewModel = _mapper.Map<BrandViewModel>(brand);

        ViewData["Title"] = $"Chỉnh sửa Thương hiệu - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa Thương hiệu: {brand.Name}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Thương hiệu", Url.Action(nameof(Index))?? string.Empty),
            ($"Chỉnh sửa: {brand.Name}", "") // Active, include name
        };

        return View(viewModel);
    }

    // POST: Admin/Brand/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BrandViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            _logger.LogWarning("Edit POST: ID mismatch. Route ID: {RouteId}, ViewModel ID: {ViewModelId}", id, viewModel.Id);
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            var brandToUpdate = await _context.Set<Brand>().FindAsync(id);
            if (brandToUpdate == null)
            {
                _logger.LogWarning("Edit POST: Brand with ID {BrandId} not found for update.", id);
                TempData["error"] = "Không tìm thấy thương hiệu để cập nhật.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _mapper.Map(viewModel, brandToUpdate);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Brand '{BrandName}' (ID: {BrandId}) updated successfully by {User}.", brandToUpdate.Name, brandToUpdate.Id, User.Identity?.Name ?? "Unknown");
                TempData["success"] = $"Cập nhật thương hiệu '{brandToUpdate.Name}' thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) // Catch broader exceptions
            {
                _logger.LogError(ex, "Error updating brand '{BrandName}' (ID: {BrandId}).", viewModel.Name, viewModel.Id);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật thương hiệu. Vui lòng thử lại.");
            }
        }
        else
        {
            _logger.LogWarning("Failed to update brand '{BrandName}' (ID: {BrandId}). Model state is invalid.", viewModel.Name, viewModel.Id);
        }

        ViewData["Title"] = $"Chỉnh sửa Thương hiệu - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa Thương hiệu: {viewModel.Name}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Thương hiệu", Url.Action(nameof(Index)) ?? string.Empty), ($"Chỉnh sửa: {viewModel.Name}", "") };
        return View(viewModel);
    }

    // POST: Admin/Brand/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var brand = await _context.Set<Brand>()
                                .Include(b => b.Products)
                                .FirstOrDefaultAsync(b => b.Id == id);

        if (brand == null)
        {
            _logger.LogWarning("Delete POST: Brand with ID {BrandId} not found.", id);
            return Json(new { success = false, message = "Không tìm thấy thương hiệu." });
        }

        if (brand.Products != null && brand.Products.Any())
        {
            _logger.LogWarning("Attempted to delete brand '{BrandName}' (ID: {BrandId}) which has associated products.", brand.Name, id);
            return Json(new { success = false, message = $"Không thể xóa thương hiệu '{brand.Name}' vì đang được sử dụng bởi {brand.Products.Count} sản phẩm. Vui lòng gỡ bỏ liên kết sản phẩm trước." });
        }

        try
        {
            string brandName = brand.Name;
            _context.Remove(brand);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Brand '{BrandName}' (ID: {BrandId}) deleted successfully by {User}.", brandName, id, User.Identity?.Name ?? "Unknown");
            return Json(new { success = true, message = $"Xóa thương hiệu '{brandName}' thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting brand '{BrandName}' (ID: {BrandId}).", brand.Name, id);
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa thương hiệu." });
        }
    }
}