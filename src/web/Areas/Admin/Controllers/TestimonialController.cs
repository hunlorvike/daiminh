using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using FluentValidation;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Testimonial;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public partial class TestimonialController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<TestimonialController> _logger;

    public TestimonialController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<TestimonialController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/Testimonial
    public async Task<IActionResult> Index(TestimonialFilterViewModel filter, int page = 1, int pageSize = 10)
    {
        filter ??= new TestimonialFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IQueryable<Testimonial> query = _context.Set<Testimonial>().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(t => t.ClientName.ToLower().Contains(lowerSearchTerm)
                                  || (t.ClientCompany != null && t.ClientCompany.ToLower().Contains(lowerSearchTerm))
                                  || t.Content.ToLower().Contains(lowerSearchTerm));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(t => t.IsActive == filter.IsActive.Value);
        }

        if (filter.Rating.HasValue)
        {
            query = query.Where(t => t.Rating == filter.Rating.Value);
        }

        query = query.OrderBy(t => t.OrderIndex).ThenByDescending(t => t.UpdatedAt);

        IPagedList<TestimonialListItemViewModel> testimonialsPaged = await query
            .ProjectTo<TestimonialListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, currentPageSize);

        filter.StatusOptions = GetStatusOptions(filter.IsActive);
        filter.RatingOptions = GetRatingOptions(filter.Rating);

        TestimonialIndexViewModel viewModel = new()
        {
            Testimonials = testimonialsPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/Testimonial/Create
    public IActionResult Create()
    {
        TestimonialViewModel viewModel = new()
        {
            IsActive = true,
            Rating = 5,
            OrderIndex = 0
        };
        return View(viewModel);
    }

    // POST: Admin/Testimonial/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TestimonialViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create Testimonial validation failed.");
            return View(viewModel);
        }

        Testimonial testimonial = _mapper.Map<Testimonial>(viewModel);
        _context.Add(testimonial);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Testimonial created successfully (ID: {TestimonialId})", testimonial.Id);
            TempData["SuccessMessage"] = "Thêm đánh giá thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating testimonial.");
            ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi thêm đánh giá.");
            return View(viewModel);
        }
    }

    // GET: Admin/Testimonial/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        Testimonial? testimonial = await _context.Set<Testimonial>()
                                              .AsNoTracking()
                                              .FirstOrDefaultAsync(t => t.Id == id);

        if (testimonial == null)
        {
            _logger.LogWarning("Edit Testimonial GET: Testimonial with ID {Id} not found.", id);
            return NotFound();
        }

        TestimonialViewModel viewModel = _mapper.Map<TestimonialViewModel>(testimonial);
        return View(viewModel);
    }

    // POST: Admin/Testimonial/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TestimonialViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return BadRequest("ID mismatch.");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Edit Testimonial POST: Validation failed for ID {Id}.", id);
            return View(viewModel);
        }

        Testimonial? testimonial = await _context.Set<Testimonial>().FirstOrDefaultAsync(t => t.Id == id);

        if (testimonial == null)
        {
            _logger.LogWarning("Edit Testimonial POST: Testimonial with ID {Id} not found for update.", id);
            TempData["ErrorMessage"] = "Không tìm thấy đánh giá để cập nhật.";
            return RedirectToAction(nameof(Index));
        }

        _mapper.Map(viewModel, testimonial);
        _context.Entry(testimonial).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Testimonial updated successfully (ID: {TestimonialId})", testimonial.Id);
            TempData["SuccessMessage"] = "Cập nhật đánh giá thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error updating testimonial ID {Id}.", id);
            ModelState.AddModelError("", "Lỗi xung đột dữ liệu. Dữ liệu có thể đã được thay đổi bởi người khác. Vui lòng tải lại trang và thử lại.");
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating testimonial ID {Id}.", id);
            ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật đánh giá.");
            return View(viewModel);
        }
    }

    // POST: Admin/Testimonial/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        Testimonial? testimonial = await _context.Set<Testimonial>().FindAsync(id);
        if (testimonial == null)
        {
            _logger.LogWarning("Delete Testimonial POST: Testimonial with ID {Id} not found.", id);
            return Json(new { success = false, message = "Không tìm thấy đánh giá." });
        }

        try
        {
            string clientName = testimonial.ClientName;
            _context.Remove(testimonial);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Testimonial deleted successfully (ID: {TestimonialId})", id);
            return Json(new { success = true, message = $"Xóa đánh giá của '{clientName}' thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting testimonial ID {Id}.", id);
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa đánh giá." });
        }
    }
}

public partial class TestimonialController
{
    private List<SelectListItem> GetStatusOptions(bool? selectedValue)
    {
        return new List<SelectListItem>
        {
            new() { Value = "", Text = "Tất cả trạng thái", Selected = !selectedValue.HasValue },
            new() { Value = "true", Text = "Đang hiển thị", Selected = selectedValue.HasValue && selectedValue.Value },
            new() { Value = "false", Text = "Đang ẩn", Selected = selectedValue.HasValue && !selectedValue.Value }
        };
    }

    private List<SelectListItem> GetRatingOptions(int? selectedValue)
    {
        var items = new List<SelectListItem>
        {
            new() { Value = "", Text = "Tất cả xếp hạng", Selected = !selectedValue.HasValue }
        };
        for (int i = 5; i >= 1; i--)
        {
            items.Add(new SelectListItem
            {
                Value = i.ToString(),
                Text = $"{i} sao",
                Selected = selectedValue.HasValue && selectedValue.Value == i
            });
        }
        return items;
    }
}