using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.ProductType;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductTypeController : Controller
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
            query = query.Where(pt => pt.Name.Contains(searchTerm) ||
                                    (pt.Description != null && pt.Description.Contains(searchTerm)));
        }

        if (isActive.HasValue)
        {
            query = query.Where(pt => pt.IsActive == isActive.Value);
        }

        var productTypes = await query
            .OrderBy(pt => pt.Name)
            .ToListAsync();

        var viewModels = _mapper.Map<List<ProductTypeListItemViewModel>>(productTypes);

        // Add product count to each view model
        foreach (var viewModel in viewModels)
        {
            var productType = productTypes.First(pt => pt.Id == viewModel.Id);
            viewModel.ProductCount = productType.Products?.Count ?? 0;
        }

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

        var viewModel = new ProductTypeViewModel
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

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            return View(viewModel);
        }

        // Check if slug is unique
        if (await _context.Set<ProductType>().AnyAsync(pt => pt.Slug == viewModel.Slug))
        {
            ModelState.AddModelError("Slug", "Slug đã tồn tại, vui lòng chọn slug khác");
            return View(viewModel);
        }

        var productType = _mapper.Map<ProductType>(viewModel);

        _context.Add(productType);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Thêm loại sản phẩm thành công";
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

        ViewData["PageTitle"] = "Chỉnh sửa loại sản phẩm";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Sản phẩm", "/Admin/Product"),
            ("Loại sản phẩm", "/Admin/ProductType"),
            ("Chỉnh sửa", "")
        };

        var viewModel = _mapper.Map<ProductTypeViewModel>(productType);

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

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            return View(viewModel);
        }

        // Check if slug is unique (excluding current product type)
        if (await _context.Set<ProductType>().AnyAsync(pt => pt.Slug == viewModel.Slug && pt.Id != id))
        {
            ModelState.AddModelError("Slug", "Slug đã tồn tại, vui lòng chọn slug khác");
            return View(viewModel);
        }

        try
        {
            var productType = await _context.Set<ProductType>().FindAsync(id);

            if (productType == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, productType);

            _context.Update(productType);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật loại sản phẩm thành công";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ProductTypeExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
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
            return Json(new { success = false, message = "Không tìm thấy loại sản phẩm" });
        }

        // Check if there are products using this product type
        if (productType.Products != null && productType.Products.Any())
        {
            return Json(new { success = false, message = $"Không thể xóa loại sản phẩm này vì đang được sử dụng bởi {productType.Products.Count} sản phẩm" });
        }

        _context.Set<ProductType>().Remove(productType);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Xóa loại sản phẩm thành công" });
    }

    // POST: Admin/ProductType/ToggleActive/5
    [HttpPost]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var productType = await _context.Set<ProductType>().FindAsync(id);

        if (productType == null)
        {
            return Json(new { success = false, message = "Không tìm thấy loại sản phẩm" });
        }

        productType.IsActive = !productType.IsActive;
        await _context.SaveChangesAsync();

        return Json(new { success = true, active = productType.IsActive });
    }

    private async Task<bool> ProductTypeExists(int id)
    {
        return await _context.Set<ProductType>().AnyAsync(e => e.Id == id);
    }
}