using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.ProductType;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public partial class ProductTypeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<ProductTypeViewModel> _validator;

    public ProductTypeController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<ProductTypeViewModel> validator)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
    }

    // GET: Admin/ProductType
    public async Task<IActionResult> Index(string? searchTerm = null, bool? isActive = null)
    {
        ViewData["PageTitle"] = "Quản lý loại sản phẩm";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Sản phẩm", "/Admin/Product"),
            ("Loại sản phẩm", "")
        };

        var query = _context.Set<ProductType>()
                            .Include(pt => pt.Products)
                            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(pt => pt.Name.Contains(searchTerm) || (pt.Description != null && pt.Description.Contains(searchTerm)));
        }

        if (isActive.HasValue)
        {
            query = query.Where(pt => pt.IsActive == isActive.Value);
        }

        var productTypes = await query.OrderBy(pt => pt.Name).ToListAsync();

        var viewModels = _mapper.Map<List<ProductTypeListItemViewModel>>(productTypes);

        ViewBag.SearchTerm = searchTerm;
        ViewBag.SelectedIsActive = isActive;

        return View(viewModels);
    }

    // GET: Admin/ProductType/Create
    public IActionResult Create()
    {
        ViewData["PageTitle"] = "Thêm loại sản phẩm mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Sản phẩm", "/Admin/Product"),
            ("Loại sản phẩm", "/Admin/ProductType"),
            ("Thêm mới", "")
        };

        ProductTypeViewModel viewModel = new()
        {
            IsActive = true
        };

        return View(viewModel);
    }

    // POST: Admin/ProductType/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductTypeViewModel viewModel)
    {
        var validationResult = await _validator.ValidateAsync(viewModel);

        if (await _context.Set<ProductType>().AnyAsync(pt => pt.Slug == viewModel.Slug))
        {
            ModelState.AddModelError(nameof(ProductTypeViewModel.Slug), "Slug đã tồn tại, vui lòng chọn slug khác.");
        }

        if (!validationResult.IsValid || !ModelState.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            ViewData["PageTitle"] = "Thêm loại sản phẩm mới";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
            {
                ("Sản phẩm", "/Admin/Product"),
                ("Loại sản phẩm", "/Admin/ProductType"),
                ("Thêm mới", "")
            };
            return View(viewModel);
        }

        var productType = _mapper.Map<ProductType>(viewModel);

        _context.Add(productType);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Thêm loại sản phẩm thành công!";
        return RedirectToAction(nameof(Index));
    }

    // GET: Admin/ProductType/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var productType = await _context.Set<ProductType>().FindAsync(id);

        if (productType == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<ProductTypeViewModel>(productType);

        ViewData["PageTitle"] = "Chỉnh sửa loại sản phẩm";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Sản phẩm", "/Admin/Product"),
            ("Loại sản phẩm", "/Admin/ProductType"),
            ("Chỉnh sửa", "")
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
            return NotFound();
        }

        var validationResult = await _validator.ValidateAsync(viewModel);

        if (await _context.Set<ProductType>().AnyAsync(pt => pt.Slug == viewModel.Slug && pt.Id != id))
        {
            ModelState.AddModelError("Slug", "Slug đã tồn tại, vui lòng chọn slug khác");
        }

        if (!validationResult.IsValid || !ModelState.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            ViewData["PageTitle"] = "Chỉnh sửa Loại sản phẩm";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
            {
                ("Sản phẩm", "/Admin/Product"),
                ("Loại sản phẩm", "/Admin/ProductType"),
                ("Chỉnh sửa", "")
            };
            return View(viewModel);
        }

        var productType = await _context.Set<ProductType>().FindAsync(id);

        if (productType == null)
        {
            return NotFound();
        }

        _mapper.Map(viewModel, productType);

        try
        {
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật loại sản phẩm thành công!";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ProductTypeExists(id))
            {
                return NotFound();
            }
            else
            {
                TempData["ErrorMessage"] = "Lỗi: Có xung đột xảy ra khi cập nhật. Dữ liệu có thể đã được thay đổi bởi người khác.";
                return View(viewModel);
            }
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "Không thể lưu thay đổi. Vui lòng kiểm tra lại dữ liệu (ví dụ: Slug có thể đã bị trùng).");
            ViewData["PageTitle"] = "Chỉnh sửa Loại sản phẩm";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
            {
                ("Sản phẩm", "/Admin/Product"),
                ("Loại sản phẩm", "/Admin/ProductType"),
                ("Chỉnh sửa", "")
            };
            return View(viewModel);
        }

        return RedirectToAction(nameof(Index));
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
            return Json(new { success = false, message = "Không tìm thấy loại sản phẩm." });
        }

        if (productType.Products != null && productType.Products.Any())
        {
            return Json(new { success = false, message = $"Không thể xóa loại '{productType.Name}'. Vui lòng xóa hoặc chuyển các sản phẩm thuộc loại này trước." });
        }

        _context.Set<ProductType>().Remove(productType);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Xóa loại sản phẩm thành công." });
    }
}

public partial class ProductTypeController
{
    private async Task<bool> ProductTypeExists(int id)
    {
        return await _context.Set<ProductType>().AnyAsync(e => e.Id == id);
    }
}