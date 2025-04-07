using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Brand;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class BrandController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<BrandViewModel> _validator;

    public BrandController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<BrandViewModel> validator)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
    }

    // GET: Admin/Brand
    public async Task<IActionResult> Index(string? searchTerm = null)
    {
        ViewData["PageTitle"] = "Quản lý Thương hiệu";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Thương hiệu", "") // Active
        };

        var query = _context.Set<Brand>()
                            .Include(b => b.Products) // Include Products for count
                            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            // Search by Name or Website
            query = query.Where(b => b.Name.Contains(searchTerm) || (b.Website != null && b.Website.Contains(searchTerm)));
        }

        var brands = await query.OrderBy(b => b.Name).ToListAsync();
        var viewModels = _mapper.Map<List<BrandListItemViewModel>>(brands);

        ViewBag.SearchTerm = searchTerm;
        return View(viewModels);
    }

    // GET: Admin/Brand/Create
    public IActionResult Create()
    {
        ViewData["PageTitle"] = "Thêm Thương hiệu mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Thương hiệu", Url.Action(nameof(Index))),
            ("Thêm mới", "") // Active
        };

        // Set default values for SEO fields if desired
        var viewModel = new BrandViewModel
        {
            IsActive = true,
            SitemapPriority = 0.6, // Example default
            SitemapChangeFrequency = "weekly", // Example default
            OgType = "product.group", // More specific OG type?
            TwitterCard = "summary"
        };

        return View(viewModel);
    }

    // POST: Admin/Brand/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BrandViewModel viewModel)
    {
        var validationResult = await _validator.ValidateAsync(viewModel);

        // Check Slug uniqueness
        if (await _context.Set<Brand>().AnyAsync(b => b.Slug == viewModel.Slug))
        {
            ModelState.AddModelError(nameof(BrandViewModel.Slug), "Slug đã tồn tại.");
        }

        if (!validationResult.IsValid || !ModelState.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            ViewData["PageTitle"] = "Thêm Thương hiệu mới";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Thương hiệu", Url.Action(nameof(Index))), ("Thêm mới", "") };
            return View(viewModel);
        }

        var brand = _mapper.Map<Brand>(viewModel);

        _context.Add(brand);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Thêm thương hiệu thành công!";
        return RedirectToAction(nameof(Index));
    }

    // GET: Admin/Brand/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var brand = await _context.Set<Brand>().FindAsync(id);
        if (brand == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<BrandViewModel>(brand);

        ViewData["PageTitle"] = "Chỉnh sửa Thương hiệu";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Thương hiệu", Url.Action(nameof(Index))),
            ("Chỉnh sửa", "") // Active
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
            return BadRequest();
        }

        var validationResult = await _validator.ValidateAsync(viewModel);

        // Check Slug uniqueness (excluding self)
        if (await _context.Set<Brand>().AnyAsync(b => b.Slug == viewModel.Slug && b.Id != id))
        {
            ModelState.AddModelError(nameof(BrandViewModel.Slug), "Slug đã tồn tại.");
        }

        if (!validationResult.IsValid || !ModelState.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            ViewData["PageTitle"] = "Chỉnh sửa Thương hiệu";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Thương hiệu", Url.Action(nameof(Index))), ("Chỉnh sửa", "") };
            return View(viewModel);
        }

        var brand = await _context.Set<Brand>().FindAsync(id);
        if (brand == null)
        {
            return NotFound();
        }

        _mapper.Map(viewModel, brand); // Map ALL fields from ViewModel

        try
        {
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Cập nhật thương hiệu thành công!";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await BrandExists(id)) return NotFound();
            TempData["ErrorMessage"] = "Lỗi: Xung đột dữ liệu khi cập nhật.";
            return View(viewModel);
        }
        catch (DbUpdateException) // Catch potential unique constraint errors
        {
            ModelState.AddModelError("", "Không thể lưu thay đổi. Vui lòng kiểm tra lại dữ liệu.");
            ViewData["PageTitle"] = "Chỉnh sửa Thương hiệu";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Thương hiệu", Url.Action(nameof(Index))), ("Chỉnh sửa", "") };
            return View(viewModel);
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: Admin/Brand/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var brand = await _context.Set<Brand>()
                                .Include(b => b.Products) // Must include Products
                                .FirstOrDefaultAsync(b => b.Id == id);

        if (brand == null)
        {
            return Json(new { success = false, message = "Không tìm thấy thương hiệu." });
        }

        if (brand.Products != null && brand.Products.Any())
        {
            return Json(new { success = false, message = $"Không thể xóa thương hiệu '{brand.Name}'. Vui lòng xóa hoặc bỏ liên kết các sản phẩm thuộc thương hiệu này trước." });
        }

        _context.Remove(brand);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Xóa thương hiệu thành công." });
    }

    private async Task<bool> BrandExists(int id)
    {
        return await _context.Set<Brand>().AnyAsync(e => e.Id == id);
    }
}