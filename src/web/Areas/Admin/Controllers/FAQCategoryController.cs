using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.FAQ;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
//[Authorize(Roles = "Admin")]
public class FAQCategoryController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<FAQCategoryViewModel> _categoryValidator;

    public FAQCategoryController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<FAQCategoryViewModel> categoryValidator)
    {
        _context = context;
        _mapper = mapper;
        _categoryValidator = categoryValidator;
    }

    // GET: Admin/FAQCategory
    public async Task<IActionResult> Index(string? searchTerm = null)
    {
        ViewData["PageTitle"] = "Danh mục FAQ";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("FAQ", "/Admin/FAQ"),
            ("Danh mục", "")
        };

        var query = _context.Set<FAQCategory>()
            .Include(c => c.FAQs)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c => c.Name.Contains(searchTerm) || c.Description.Contains(searchTerm));
        }

        var categories = await query
            .OrderBy(c => c.OrderIndex)
            .ToListAsync();

        var viewModels = _mapper.Map<List<FAQCategoryListItemViewModel>>(categories);

        ViewBag.SearchTerm = searchTerm;

        return View(viewModels);
    }

    // GET: Admin/FAQCategory/Create
    public IActionResult Create()
    {
        ViewData["PageTitle"] = "Thêm danh mục mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("FAQ", "/Admin/FAQ"),
            ("Danh mục", "/Admin/FAQCategory"),
            ("Thêm mới", "")
        };

        var viewModel = new FAQCategoryViewModel
        {
            IsActive = true,
            OrderIndex = 0
        };

        return View(viewModel);
    }

    // POST: Admin/FAQCategory/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FAQCategoryViewModel viewModel)
    {
        var validationResult = await _categoryValidator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            return View(viewModel);
        }

        // Check if slug is unique
        if (await _context.Set<FAQCategory>().AnyAsync(c => c.Slug == viewModel.Slug))
        {
            ModelState.AddModelError("Slug", "Slug đã tồn tại, vui lòng chọn slug khác");
            return View(viewModel);
        }

        var category = _mapper.Map<FAQCategory>(viewModel);

        _context.Add(category);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Thêm danh mục FAQ thành công";
        return RedirectToAction(nameof(Index));
    }

    // GET: Admin/FAQCategory/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _context.Set<FAQCategory>().FindAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        ViewData["PageTitle"] = "Chỉnh sửa danh mục";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("FAQ", "/Admin/FAQ"),
            ("Danh mục", "/Admin/FAQCategory"),
            ("Chỉnh sửa", "")
        };

        var viewModel = _mapper.Map<FAQCategoryViewModel>(category);

        return View(viewModel);
    }

    // POST: Admin/FAQCategory/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, FAQCategoryViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        var validationResult = await _categoryValidator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            return View(viewModel);
        }

        // Check if slug is unique (excluding current category)
        if (await _context.Set<FAQCategory>().AnyAsync(c => c.Slug == viewModel.Slug && c.Id != id))
        {
            ModelState.AddModelError("Slug", "Slug đã tồn tại, vui lòng chọn slug khác");
            return View(viewModel);
        }

        try
        {
            var category = await _context.Set<FAQCategory>().FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, category);

            _context.Update(category);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật danh mục FAQ thành công";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CategoryExists(id))
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

    // POST: Admin/FAQCategory/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Set<FAQCategory>()
            .Include(c => c.FAQs)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
        {
            return Json(new { success = false, message = "Không tìm thấy danh mục" });
        }

        if (category.FAQs != null && category.FAQs.Any())
        {
            return Json(new { success = false, message = "Không thể xóa danh mục có chứa câu hỏi. Vui lòng xóa hoặc chuyển các câu hỏi sang danh mục khác trước." });
        }

        _context.Set<FAQCategory>().Remove(category);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Xóa danh mục thành công" });
    }

    private async Task<bool> CategoryExists(int id)
    {
        return await _context.Set<FAQCategory>().AnyAsync(e => e.Id == id);
    }

    // POST: Admin/FAQCategory/ToggleActive/5
    [HttpPost]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var category = await _context.Set<FAQCategory>().FindAsync(id);

        if (category == null)
        {
            return Json(new { success = false, message = "Không tìm thấy danh mục" });
        }

        category.IsActive = !category.IsActive;
        await _context.SaveChangesAsync();

        return Json(new { success = true, active = category.IsActive });
    }
}

