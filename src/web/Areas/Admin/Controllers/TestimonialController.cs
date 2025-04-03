using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Testimonial;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class TestimonialController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<TestimonialViewModel> _validator;

    public TestimonialController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<TestimonialViewModel> validator)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
    }

    // GET: Admin/Testimonial
    public async Task<IActionResult> Index(string? searchTerm = null)
    {
        ViewData["PageTitle"] = "Quản lý đánh giá khách hàng";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Đánh giá khách hàng", "")
        };

        var query = _context.Set<Testimonial>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(t =>
                t.ClientName.Contains(searchTerm) ||
                t.ClientCompany.Contains(searchTerm) ||
                t.Content.Contains(searchTerm) ||
                t.ProjectReference.Contains(searchTerm));
        }

        var testimonials = await query
            .OrderBy(t => t.OrderIndex)
            .ToListAsync();

        var viewModels = _mapper.Map<List<TestimonialListItemViewModel>>(testimonials);

        ViewBag.SearchTerm = searchTerm;

        return View(viewModels);
    }

    // GET: Admin/Testimonial/Create
    public IActionResult Create()
    {
        ViewData["PageTitle"] = "Thêm đánh giá khách hàng mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Đánh giá khách hàng", "/Admin/Testimonial"),
            ("Thêm mới", "")
        };

        var viewModel = new TestimonialViewModel
        {
            IsActive = true,
            OrderIndex = 0,
            Rating = 5
        };

        return View(viewModel);
    }

    // POST: Admin/Testimonial/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TestimonialViewModel viewModel)
    {
        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            return View(viewModel);
        }

        var testimonial = _mapper.Map<Testimonial>(viewModel);

        _context.Add(testimonial);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Thêm đánh giá khách hàng thành công";
        return RedirectToAction(nameof(Index));
    }

    // GET: Admin/Testimonial/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var testimonial = await _context.Set<Testimonial>().FindAsync(id);

        if (testimonial == null)
        {
            return NotFound();
        }

        ViewData["PageTitle"] = "Chỉnh sửa đánh giá khách hàng";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Đánh giá khách hàng", "/Admin/Testimonial"),
            ("Chỉnh sửa", "")
        };

        var viewModel = _mapper.Map<TestimonialViewModel>(testimonial);

        return View(viewModel);
    }

    // POST: Admin/Testimonial/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TestimonialViewModel viewModel)
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
            var testimonial = await _context.Set<Testimonial>().FindAsync(id);

            if (testimonial == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, testimonial);

            _context.Update(testimonial);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật đánh giá khách hàng thành công";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await TestimonialExists(id))
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

    // POST: Admin/Testimonial/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var testimonial = await _context.Set<Testimonial>().FindAsync(id);

        if (testimonial == null)
        {
            return Json(new { success = false, message = "Không tìm thấy đánh giá khách hàng" });
        }

        _context.Set<Testimonial>().Remove(testimonial);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Xóa đánh giá khách hàng thành công" });
    }

    // POST: Admin/Testimonial/ToggleActive/5
    [HttpPost]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var testimonial = await _context.Set<Testimonial>().FindAsync(id);

        if (testimonial == null)
        {
            return Json(new { success = false, message = "Không tìm thấy đánh giá khách hàng" });
        }

        testimonial.IsActive = !testimonial.IsActive;
        await _context.SaveChangesAsync();

        return Json(new { success = true, active = testimonial.IsActive });
    }

    private async Task<bool> TestimonialExists(int id)
    {
        return await _context.Set<Testimonial>().AnyAsync(e => e.Id == id);
    }
}

