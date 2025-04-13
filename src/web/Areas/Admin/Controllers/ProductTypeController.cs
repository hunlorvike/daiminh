using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.ProductType;
using X.PagedList.EF;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public partial class ProductTypeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductTypeController> _logger;

    public ProductTypeController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<ProductTypeController> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: Admin/ProductType
    public async Task<IActionResult> Index(string? searchTerm = null, bool? isActive = null, int page = 1, int pageSize = 15)
    {
        ViewData["Title"] = "Quản lý loại sản phẩm - Hệ thống quản trị";
        ViewData["PageTitle"] = "Danh sách Loại sản phẩm";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Sản phẩm",  Url.Action(nameof(Index), "Product", new { area ="Admin" })),
            ("Loại sản phẩm", Url.Action(nameof(Index)))
        };

        int pageNumber = page;
        var query = _context.Set<ProductType>()
                            .Include(pt => pt.Products)
                            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string lowerSearchTerm = searchTerm.Trim().ToLower();
            query = query.Where(pt => pt.Name.ToLower().Contains(lowerSearchTerm)
                                  || (pt.Description != null && pt.Description.ToLower().Contains(lowerSearchTerm)));
        }

        if (isActive.HasValue)
        {
            query = query.Where(pt => pt.IsActive == isActive.Value);
        }

        var productTypesPaged = await query
            .OrderBy(pt => pt.Name)
            .ProjectTo<ProductTypeListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        ViewBag.SearchTerm = searchTerm;
        ViewBag.SelectedIsActive = isActive;
        ViewBag.ActiveStatusList = new List<SelectListItem>
            {
                new SelectListItem { Value = "true", Text = "Đang hoạt động" },
                new SelectListItem { Value = "false", Text = "Không hoạt động" }
            };

        return View(productTypesPaged);
    }

    // GET: Admin/ProductType/Create
    public IActionResult Create()
    {
        ViewData["Title"] = "Thêm loại sản phẩm - Hệ thống quản trị";
        ViewData["PageTitle"] = "Thêm loại sản phẩm mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Sản phẩm",  Url.Action(nameof(Index), "Product", new { area ="Admin" })),
            ("Loại sản phẩm", Url.Action(nameof(Index))),
            ("Thêm mới", "")
        };

        var viewModel = new ProductTypeViewModel { IsActive = true };
        return View(viewModel);
    }

    // POST: Admin/ProductType/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductTypeViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var productType = _mapper.Map<ProductType>(viewModel);

                _context.Add(productType);
                await _context.SaveChangesAsync();

                _logger.LogInformation("ProductType '{Name}' created successfully by {User}.", productType.Name, User.Identity?.Name ?? "Unknown");
                TempData["success"] = $"Thêm loại sản phẩm '{productType.Name}' thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error creating ProductType '{Name}'. Check unique constraints.", viewModel.Name);
                if (ex.InnerException?.Message.Contains("UNIQUE constraint failed: product_types.slug", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    ModelState.AddModelError(nameof(viewModel.Slug), "Slug này đã tồn tại.");
                }
                else
                {
                    ModelState.AddModelError("", "Lỗi cơ sở dữ liệu khi tạo loại sản phẩm.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ProductType '{Name}'.", viewModel.Name);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi tạo loại sản phẩm.");
            }
        }
        else
        {
            _logger.LogWarning("Failed to create ProductType '{Name}'. Model state is invalid.", viewModel.Name);
        }


        ViewData["Title"] = "Thêm loại sản phẩm - Hệ thống quản trị";
        ViewData["PageTitle"] = "Thêm loại sản phẩm mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> 
        {
            ("Sản phẩm",  Url.Action(nameof(Index), "Product", new { area ="Admin" })),
            ("Loại sản phẩm", Url.Action(nameof(Index))),
            ("Thêm mới", "")
        };
        return View(viewModel);
    }

    // GET: Admin/ProductType/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var productType = await _context.Set<ProductType>().AsNoTracking().FirstOrDefaultAsync(pt => pt.Id == id);
        if (productType == null)
        {
            _logger.LogWarning("Edit GET: ProductType with ID {ProductTypeId} not found.", id);
            return NotFound();
        }

        var viewModel = _mapper.Map<ProductTypeViewModel>(productType);

        ViewData["Title"] = "Chỉnh sửa loại sản phẩm - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa loại sản phẩm: {productType.Name}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Sản phẩm",  Url.Action(nameof(Index), "Product", new { area ="Admin" })),
            ("Loại sản phẩm", Url.Action(nameof(Index))),
            ($"Chỉnh sửa: {productType.Name}", "")
        };

        return View(viewModel);
    }

    // POST: Admin/ProductType/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductTypeViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            _logger.LogWarning("Edit POST: ID mismatch. Route ID: {RouteId}, ViewModel ID: {ViewModelId}", id, viewModel.Id);
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            var productTypeToUpdate = await _context.Set<ProductType>().FindAsync(id);
            if (productTypeToUpdate == null)
            {
                _logger.LogWarning("Edit POST: ProductType with ID {ProductTypeId} not found for update.", id);
                TempData["error"] = "Không tìm thấy loại sản phẩm để cập nhật.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _mapper.Map(viewModel, productTypeToUpdate);

                await _context.SaveChangesAsync();

                _logger.LogInformation("ProductType '{Name}' (ID: {ProductTypeId}) updated successfully by {User}.", productTypeToUpdate.Name, id, User.Identity?.Name ?? "Unknown");
                TempData["success"] = $"Cập nhật loại sản phẩm '{productTypeToUpdate.Name}' thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Concurrency error occurred while editing ProductType ID {ProductTypeId}.", id);
                TempData["error"] = "Lỗi: Xung đột dữ liệu. Loại sản phẩm này có thể đã được cập nhật bởi người khác.";
                // Optionally reload the view model with current DB data
                // var currentDbData = await _context.Set<ProductType>().AsNoTracking().FirstOrDefaultAsync(pt => pt.Id == id);
                // if(currentDbData != null) _mapper.Map(currentDbData, viewModel);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating ProductType '{Name}' (ID: {ProductTypeId}). Check unique constraints.", viewModel.Name, id);
                if (ex.InnerException?.Message.Contains("UNIQUE constraint failed: product_types.slug", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    ModelState.AddModelError(nameof(viewModel.Slug), "Slug này đã tồn tại.");
                }
                else
                {
                    ModelState.AddModelError("", "Lỗi cơ sở dữ liệu khi cập nhật loại sản phẩm.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ProductType '{Name}' (ID: {ProductTypeId}).", viewModel.Name, id);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật.");
            }
        }
        else
        {
            _logger.LogWarning("Failed to update ProductType '{Name}' (ID: {ProductTypeId}). Model state is invalid.", viewModel.Name, id);
        }

        ViewData["Title"] = "Chỉnh sửa loại sản phẩm - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa loại sản phẩm: {viewModel.Name}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>  {
            ("Sản phẩm",  Url.Action(nameof(Index), "Product", new { area ="Admin" })),
            ("Loại sản phẩm", Url.Action(nameof(Index))),
            ($"Chỉnh sửa: {viewModel.Name}", "")
        };
        return View(viewModel);
    }

    // POST: Admin/ProductType/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var productType = await _context.Set<ProductType>()
                                    .Include(pt => pt.Products)
                                    .FirstOrDefaultAsync(pt => pt.Id == id);

        if (productType == null)
        {
            _logger.LogWarning("Delete POST: ProductType with ID {ProductTypeId} not found.", id);
            return Json(new { success = false, message = "Không tìm thấy loại sản phẩm." });
        }

        if (productType.Products != null && productType.Products.Any())
        {
            _logger.LogWarning("Attempted to delete ProductType '{Name}' (ID: {ProductTypeId}) which has {ProductCount} associated products.", productType.Name, id, productType.Products.Count);
            return Json(new { success = false, message = $"Không thể xóa loại '{productType.Name}'. Đang có {productType.Products.Count} sản phẩm thuộc loại này." });
        }

        try
        {
            string name = productType.Name;
            _context.Set<ProductType>().Remove(productType);
            await _context.SaveChangesAsync();

            _logger.LogInformation("ProductType '{Name}' (ID: {ProductTypeId}) deleted successfully by {User}.", name, id, User.Identity?.Name ?? "Unknown");
            return Json(new { success = true, message = $"Xóa loại sản phẩm '{name}' thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting ProductType ID {ProductTypeId} ('{Name}').", id, productType.Name);
            return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa loại sản phẩm." });
        }
    }
}
