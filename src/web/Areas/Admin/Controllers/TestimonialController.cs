using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Testimonial;
using X.PagedList.EF;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class TestimonialController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<TestimonialController> _logger;

    public TestimonialController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<TestimonialController> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: Admin/Testimonial
    public async Task<IActionResult> Index(string? searchTerm = null, int page = 1, int pageSize = 15)
    {
        ViewData["Title"] = "Quản lý đánh giá khách hàng - Hệ thống quản trị";
        ViewData["PageTitle"] = "Danh sách đánh giá khách hàng";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Đánh giá khách hàng", Url.Action(nameof(Index))) // Link to self for reset
        };

        int pageNumber = page;

        var query = _context.Set<Testimonial>().AsNoTracking(); // Use AsNoTracking

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string lowerSearchTerm = searchTerm.Trim().ToLower();
            query = query.Where(t =>
                t.ClientName.ToLower().Contains(lowerSearchTerm) ||
                (t.ClientCompany != null && t.ClientCompany.ToLower().Contains(lowerSearchTerm)) ||
                t.Content.ToLower().Contains(lowerSearchTerm) || // Search content
                (t.ProjectReference != null && t.ProjectReference.ToLower().Contains(lowerSearchTerm)));
        }

        var testimonialsPaged = await query
            .OrderBy(t => t.OrderIndex)
            .ThenByDescending(t => t.CreatedAt) // Secondary sort
            .ProjectTo<TestimonialListItemViewModel>(_mapper.ConfigurationProvider) // Project efficiently
            .ToPagedListAsync(pageNumber, pageSize); // Paginate

        ViewBag.SearchTerm = searchTerm;

        return View(testimonialsPaged); // Pass paged list
    }

    // GET: Admin/Testimonial/Create
    public IActionResult Create()
    {
        ViewData["Title"] = "Thêm đánh giá khách hàng - Hệ thống quản trị";
        ViewData["PageTitle"] = "Thêm đánh giá khách hàng mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Đánh giá khách hàng", Url.Action(nameof(Index))),
            ("Thêm mới", "") // Active
        };

        var viewModel = new TestimonialViewModel
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
        // Rely on FluentValidation middleware
        if (ModelState.IsValid)
        {
            try
            {
                var testimonial = _mapper.Map<Testimonial>(viewModel);
                // Audit fields (CreatedBy) likely set by interceptor/base logic

                _context.Add(testimonial);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Testimonial for '{ClientName}' created successfully by {User}.", testimonial.ClientName, User.Identity?.Name ?? "Unknown");
                TempData["success"] = $"Thêm đánh giá của '{testimonial.ClientName}' thành công."; // Use standard key
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating testimonial for '{ClientName}'.", viewModel.ClientName);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi thêm đánh giá.");
            }
        }
        else
        {
            _logger.LogWarning("Failed to create testimonial for '{ClientName}'. Model state is invalid.", viewModel.ClientName);
        }

        // If failed, redisplay form
        ViewData["Title"] = "Thêm đánh giá khách hàng - Hệ thống quản trị";
        ViewData["PageTitle"] = "Thêm đánh giá khách hàng mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Đánh giá khách hàng", Url.Action(nameof(Index))), ("Thêm mới", "") };
        return View(viewModel);
    }

    // GET: Admin/Testimonial/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var testimonial = await _context.Set<Testimonial>().AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        if (testimonial == null)
        {
            _logger.LogWarning("Edit GET: Testimonial with ID {TestimonialId} not found.", id);
            return NotFound();
        }

        var viewModel = _mapper.Map<TestimonialViewModel>(testimonial);

        ViewData["Title"] = "Chỉnh sửa đánh giá khách hàng - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa đánh giá: {testimonial.ClientName}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Đánh giá khách hàng", Url.Action(nameof(Index))),
            ($"Chỉnh sửa: {testimonial.ClientName}", "") // Active
        };

        return View(viewModel);
    }

    // POST: Admin/Testimonial/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TestimonialViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            _logger.LogWarning("Edit POST: ID mismatch. Route ID: {RouteId}, ViewModel ID: {ViewModelId}", id, viewModel.Id);
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            var testimonialToUpdate = await _context.Set<Testimonial>().FindAsync(id);
            if (testimonialToUpdate == null)
            {
                _logger.LogWarning("Edit POST: Testimonial with ID {TestimonialId} not found for update.", id);
                TempData["error"] = "Không tìm thấy đánh giá để cập nhật.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _mapper.Map(viewModel, testimonialToUpdate);
                // Audit fields (UpdatedBy) likely set by interceptor

                await _context.SaveChangesAsync();

                _logger.LogInformation("Testimonial for '{ClientName}' (ID: {TestimonialId}) updated successfully by {User}.", testimonialToUpdate.ClientName, id, User.Identity?.Name ?? "Unknown");
                TempData["success"] = $"Cập nhật đánh giá của '{testimonialToUpdate.ClientName}' thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) // Catch broader exceptions
            {
                _logger.LogError(ex, "Error updating testimonial for '{ClientName}' (ID: {TestimonialId}).", viewModel.ClientName, id);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật đánh giá.");
            }
        }
        else
        {
            _logger.LogWarning("Failed to update testimonial for '{ClientName}' (ID: {TestimonialId}). Model state is invalid.", viewModel.ClientName, id);
        }


        // If failed, redisplay form
        ViewData["Title"] = "Chỉnh sửa đánh giá khách hàng - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa đánh giá: {viewModel.ClientName}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Đánh giá khách hàng", Url.Action(nameof(Index))), ($"Chỉnh sửa: {viewModel.ClientName}", "") };
        return View(viewModel);
    }

    // POST: Admin/Testimonial/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var testimonial = await _context.Set<Testimonial>().FindAsync(id);
        if (testimonial == null)
        {
            _logger.LogWarning("Delete POST: Testimonial with ID {TestimonialId} not found.", id);
            // Standard JSON response for AJAX
            return Json(new { success = false, message = "Không tìm thấy đánh giá." });
        }

        try
        {
            string clientName = testimonial.ClientName;
            _context.Set<Testimonial>().Remove(testimonial);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Testimonial for '{ClientName}' (ID: {TestimonialId}) deleted successfully by {User}.", clientName, id, User.Identity?.Name ?? "Unknown");
            return Json(new { success = true, message = $"Xóa đánh giá của '{clientName}' thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting testimonial for '{ClientName}' (ID: {TestimonialId}).", testimonial.ClientName, id);
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa đánh giá." });
        }
    }
}
// --- END OF FILE TestimonialController.cs ---