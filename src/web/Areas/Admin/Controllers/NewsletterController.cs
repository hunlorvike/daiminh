using AutoMapper;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // For SelectList if needed for filter
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Newsletter;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize] // Define roles if necessary
public class NewsletterController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<NewsletterController> _logger;

    public NewsletterController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<NewsletterController> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: Admin/Newsletter
    public async Task<IActionResult> Index(string? searchTerm = null, bool? isActive = null)
    {
        ViewData["PageTitle"] = "Quản lý Đăng ký Bản tin";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Đăng ký Bản tin", "") };

        var query = _context.Set<Newsletter>().AsQueryable();

        // Filtering
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(n => n.Email.Contains(searchTerm) || (n.Name != null && n.Name.Contains(searchTerm)));
        }
        if (isActive.HasValue)
        {
            query = query.Where(n => n.IsActive == isActive.Value);
        }

        // Ordering
        var newsletters = await query.OrderByDescending(n => n.CreatedAt).ToListAsync();
        var viewModels = _mapper.Map<List<NewsletterListItemViewModel>>(newsletters);

        // Filter Data
        ViewBag.SearchTerm = searchTerm;
        ViewBag.SelectedIsActive = isActive;
        // Optional: Create SelectList for IsActive filter
        ViewBag.ActiveStatusList = new List<SelectListItem>
            {
                new SelectListItem { Value = "true", Text = "Đang hoạt động" },
                new SelectListItem { Value = "false", Text = "Đã hủy" }
            };


        return View(viewModels);
    }

    // POST: Admin/Newsletter/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var newsletter = await _context.Set<Newsletter>().FindAsync(id);
        if (newsletter == null)
        {
            return Json(new { success = false, message = "Không tìm thấy đăng ký." });
        }

        _context.Remove(newsletter);
        try
        {
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Xóa đăng ký thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting Newsletter subscription ID {NewsletterId}.", id);
            return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa đăng ký." });
        }
    }
}