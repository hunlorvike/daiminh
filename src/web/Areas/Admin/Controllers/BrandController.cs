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
        ViewData["PageTitle"] = "Quản lý thương hiệu";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Thương hiệu", "")
        };

        var query = _context.Set<Brand>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(b =>
                b.Name.Contains(searchTerm) ||
                b.Slug.Contains(searchTerm) ||
                b.Description.Contains(searchTerm));
        }

        var brands = await query
            .Include(b => b.Products)
            .OrderBy(b => b.Name)
            .ToListAsync();

        var viewModels = _mapper.Map<List<BrandListItemViewModel>>(brands);

        ViewBag.SearchTerm = searchTerm;

        return View(viewModels);
    }

    // GET: Admin/Brand/Create
    public IActionResult Create()
    {
        ViewData["PageTitle"] = "Thêm thương hiệu mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Thương hiệu", "/Admin/Brand"),
            ("Thêm mới", "")
        };

        var viewModel = new BrandViewModel
        {
            IsActive = true
        };

        return View(viewModel);
    }

    // POST: Admin/Brand/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BrandViewModel viewModel)
    {
        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            return View(viewModel);
        }

        var brand = _mapper.Map<Brand>(viewModel);

        _context.Add(brand);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Thêm thương hiệu thành công";
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

        ViewData["PageTitle"] = "Chỉnh sửa thương hiệu";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Thương hiệu", "/Admin/Brand"),
            ("Chỉnh sửa", "")
        };

        var viewModel = _mapper.Map<BrandViewModel>(brand);

        return View(viewModel);
    }

    // POST: Admin/Brand/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BrandViewModel viewModel)
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

        try
        {
            var brand = await _context.Set<Brand>().FindAsync(id);

            if (brand == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, brand);

            _context.Update(brand);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật thương hiệu thành công";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await BrandExists(id))
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

    // POST: Admin/Brand/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var brand = await _context.Set<Brand>().FindAsync(id);

        if (brand == null)
        {
            return Json(new { success = false, message = "Không tìm thấy thương hiệu" });
        }

        _context.Set<Brand>().Remove(brand);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Xóa thương hiệu thành công" });
    }

    // POST: Admin/Brand/ToggleActive/5
    [HttpPost]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var brand = await _context.Set<Brand>().FindAsync(id);

        if (brand == null)
        {
            return Json(new { success = false, message = "Không tìm thấy thương hiệu" });
        }

        brand.IsActive = !brand.IsActive;
        await _context.SaveChangesAsync();

        return Json(new { success = true, active = brand.IsActive });
    }

    private async Task<bool> BrandExists(int id)
    {
        return await _context.Set<Brand>().AnyAsync(e => e.Id == id);
    }
} 