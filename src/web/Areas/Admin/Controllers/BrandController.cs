using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using FluentValidation;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Models;
using System.Text.Json;
using web.Areas.Admin.Validators.Brand;
using web.Areas.Admin.ViewModels.Brand;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public partial class BrandController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<BrandController> _logger;

    public BrandController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<BrandController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/Brand
    public async Task<IActionResult> Index(BrandFilterViewModel filter, int page = 1, int pageSize = 15)
    {
        filter ??= new BrandFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IQueryable<Brand> query = _context.Set<Brand>()
                                         .Include(b => b.Products)
                                         .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(b => b.Name.ToLower().Contains(lowerSearchTerm) ||
                                     (b.Description != null && b.Description.ToLower().Contains(lowerSearchTerm)));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(b => b.IsActive == filter.IsActive.Value);
        }

        query = query.OrderBy(b => b.Name);

        IPagedList<BrandListItemViewModel> brandsPaged = await query
            .ProjectTo<BrandListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, currentPageSize);

        filter.StatusOptions = GetStatusSelectList(filter.IsActive);

        BrandIndexViewModel viewModel = new()
        {
            Brands = brandsPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/Brand/Create
    public IActionResult Create()
    {
        BrandViewModel viewModel = new()
        {
            IsActive = true,
        };
        return View(viewModel);
    }

    // POST: Admin/Brand/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BrandViewModel viewModel)
    {
        var result = await new BrandViewModelValidator(_context).ValidateAsync(viewModel);
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        var brand = _mapper.Map<Brand>(viewModel);
        _context.Add(brand);

        try
        {
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Thêm thương hiệu '{brand.Name}' thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo thương hiệu: {Name}", viewModel.Name);
            if (ex.InnerException?.Message.Contains("slug", StringComparison.OrdinalIgnoreCase) == true)
            {
                ModelState.AddModelError(nameof(viewModel.Slug), "Slug này đã được sử dụng.");
            }
            else
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi cơ sở dữ liệu khi lưu thương hiệu.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi tạo thương hiệu: {Name}", viewModel.Name);
            ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi lưu thương hiệu.");
        }

        TempData["ToastMessage"] = JsonSerializer.Serialize(
            new ToastData("Lỗi", $"Không thể thêm thương hiệu '{viewModel.Name}'.", ToastType.Error)
        );
        return View(viewModel);
    }

    // GET: Admin/Brand/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        Brand? brand = await _context.Set<Brand>()
                                           .AsNoTracking()
                                           .FirstOrDefaultAsync(b => b.Id == id);

        if (brand == null)
        {
            return NotFound();
        }

        BrandViewModel viewModel = _mapper.Map<BrandViewModel>(brand);
        return View(viewModel);
    }

    // POST: Admin/Brand/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BrandViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var result = await new BrandViewModelValidator(_context).ValidateAsync(viewModel);
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        var brand = await _context.Set<Brand>().FirstOrDefaultAsync(b => b.Id == id);
        if (brand == null)
        {
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy thương hiệu để cập nhật.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        _mapper.Map(viewModel, brand);

        try
        {
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Cập nhật thương hiệu '{brand.Name}' thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi cập nhật thương hiệu ID {Id}, Name {Name}", id, brand.Name);
            if (ex.InnerException?.Message.Contains("slug", StringComparison.OrdinalIgnoreCase) == true)
            {
                ModelState.AddModelError(nameof(viewModel.Slug), "Slug này đã được sử dụng.");
            }
            else
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi cơ sở dữ liệu khi cập nhật thương hiệu.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi cập nhật thương hiệu ID {Id}", id);
            ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật thương hiệu.");
        }

        TempData["ToastMessage"] = JsonSerializer.Serialize(
            new ToastData("Lỗi", $"Không thể cập nhật thương hiệu '{viewModel.Name}'.", ToastType.Error)
        );
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
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy thương hiệu.", ToastType.Error)
            );
            return Json(new { success = false, message = "Không tìm thấy thương hiệu." });
        }

        if (brand.Products?.Any() == true)
        {
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", $"Không thể xóa thương hiệu '{brand.Name}' vì đang được sử dụng bởi {brand.Products.Count} sản phẩm.", ToastType.Error)
            );
            return Json(new
            {
                success = false,
                message = $"Không thể xóa thương hiệu '{brand.Name}' vì đang được sử dụng bởi {brand.Products.Count} sản phẩm."
            });
        }

        try
        {
            string brandName = brand.Name;
            _context.Remove(brand);
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Xóa thương hiệu '{brandName}' thành công.", ToastType.Success)
            );
            return Json(new { success = true, message = $"Xóa thương hiệu '{brandName}' thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa thương hiệu: {Name}", brand.Name);
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Đã xảy ra lỗi không mong muốn khi xóa thương hiệu.", ToastType.Error)
            );
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa thương hiệu." });
        }
    }
}

public partial class BrandController
{
    private List<SelectListItem> GetStatusSelectList(bool? selectedValue)
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "Tất cả trạng thái", Selected = !selectedValue.HasValue },
            new SelectListItem { Value = "true", Text = "Đang kích hoạt", Selected = selectedValue == true },
            new SelectListItem { Value = "false", Text = "Đã hủy kích hoạt", Selected = selectedValue == false }
        };
    }
}