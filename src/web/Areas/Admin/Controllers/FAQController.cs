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
public class FAQController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<FAQViewModel> _faqValidator;

    public FAQController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<FAQViewModel> faqValidator)
    {
        _context = context;
        _mapper = mapper;
        _faqValidator = faqValidator;
    }

    // GET: Admin/FAQ
    public async Task<IActionResult> Index(string? searchTerm = null, int? categoryId = null)
    {
        ViewData["PageTitle"] = "Quản lý câu hỏi thường gặp";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("FAQ", "")
        };

        var query = _context.Set<FAQ>()
            .Include(f => f.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(f => f.Question.Contains(searchTerm) || f.Answer.Contains(searchTerm));
        }

        if (categoryId.HasValue && categoryId.Value > 0)
        {
            query = query.Where(f => f.CategoryId == categoryId.Value);
        }

        var faqs = await query
            .OrderBy(f => f.CategoryId)
            .ThenBy(f => f.OrderIndex)
            .ToListAsync();

        var faqViewModels = _mapper.Map<List<FAQListItemViewModel>>(faqs);

        // Get categories for filter dropdown
        ViewBag.Categories = await _context.Set<FAQCategory>()
            .Where(c => c.IsActive)
            .OrderBy(c => c.OrderIndex)
            .Select(c => new FAQCategoryViewModel
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();

        ViewBag.SelectedCategoryId = categoryId;
        ViewBag.SearchTerm = searchTerm;

        return View(faqViewModels);
    }

    // GET: Admin/FAQ/Create
    public async Task<IActionResult> Create()
    {
        ViewData["PageTitle"] = "Thêm câu hỏi mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("FAQ", "/Admin/FAQ"),
            ("Thêm mới", "")
        };

        var viewModel = new FAQViewModel
        {
            IsActive = true,
            OrderIndex = 0
        };

        // Load categories for dropdown
        viewModel.Categories = await _context.Set<FAQCategory>()
            .Where(c => c.IsActive)
            .OrderBy(c => c.OrderIndex)
            .Select(c => new FAQCategoryViewModel
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();

        return View(viewModel);
    }

    // POST: Admin/FAQ/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FAQViewModel viewModel)
    {
        var validationResult = await _faqValidator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);

            // Reload categories for dropdown
            viewModel.Categories = await _context.Set<FAQCategory>()
                .Where(c => c.IsActive)
                .OrderBy(c => c.OrderIndex)
                .Select(c => new FAQCategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return View(viewModel);
        }

        var faq = _mapper.Map<FAQ>(viewModel);

        _context.Add(faq);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Thêm câu hỏi thành công";
        return RedirectToAction(nameof(Index));
    }

    // GET: Admin/FAQ/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var faq = await _context.Set<FAQ>().FindAsync(id);

        if (faq == null)
        {
            return NotFound();
        }

        ViewData["PageTitle"] = "Chỉnh sửa câu hỏi";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("FAQ", "/Admin/FAQ"),
            ("Chỉnh sửa", "")
        };

        var viewModel = _mapper.Map<FAQViewModel>(faq);

        // Load categories for dropdown
        viewModel.Categories = await _context.Set<FAQCategory>()
            .Where(c => c.IsActive)
            .OrderBy(c => c.OrderIndex)
            .Select(c => new FAQCategoryViewModel
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();

        return View(viewModel);
    }

    // POST: Admin/FAQ/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, FAQViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        var validationResult = await _faqValidator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);

            // Reload categories for dropdown
            viewModel.Categories = await _context.Set<FAQCategory>()
                .Where(c => c.IsActive)
                .OrderBy(c => c.OrderIndex)
                .Select(c => new FAQCategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return View(viewModel);
        }

        try
        {
            var faq = await _context.Set<FAQ>().FindAsync(id);

            if (faq == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, faq);

            _context.Update(faq);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật câu hỏi thành công";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await FAQExists(id))
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

    // POST: Admin/FAQ/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var faq = await _context.Set<FAQ>().FindAsync(id);

        if (faq == null)
        {
            return Json(new { success = false, message = "Không tìm thấy câu hỏi" });
        }

        _context.Set<FAQ>().Remove(faq);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Xóa câu hỏi thành công" });
    }

    private async Task<bool> FAQExists(int id)
    {
        return await _context.Set<FAQ>().AnyAsync(e => e.Id == id);
    }

    // POST: Admin/FAQ/ToggleActive/5
    [HttpPost]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var faq = await _context.Set<FAQ>().FindAsync(id);

        if (faq == null)
        {
            return Json(new { success = false, message = "Không tìm thấy câu hỏi" });
        }

        faq.IsActive = !faq.IsActive;
        await _context.SaveChangesAsync();

        return Json(new { success = true, active = faq.IsActive });
    }
}

