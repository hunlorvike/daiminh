using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using web.Areas.Admin.ViewModels.Newsletter;
using X.PagedList.EF;
using System.Drawing;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
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
    public async Task<IActionResult> Index(string? searchTerm = null, bool? isActive = null, int page = 1, int pageSize = 20)
    {
        ViewData["Title"] = "Quản lý Đăng ký Bản tin - Hệ thống quản trị";
        ViewData["PageTitle"] = "Danh sách Đăng ký Bản tin";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Đăng ký Bản tin", Url.Action(nameof(Index))) };

        int pageNumber = page;

        var query = _context.Set<Newsletter>().AsNoTracking();

        // Filtering
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string lowerSearchTerm = searchTerm.Trim().ToLower();
            query = query.Where(n => n.Email.ToLower().Contains(lowerSearchTerm)
                                  || (n.Name != null && n.Name.ToLower().Contains(lowerSearchTerm)));
        }
        if (isActive.HasValue)
        {
            if (isActive.Value)
            {
                query = query.Where(n => n.UnsubscribedAt == null);
            }
            else
            {
                query = query.Where(n => n.UnsubscribedAt != null);
            }
        }

        // Ordering & Pagination
        var newslettersPaged = await query
            .OrderByDescending(n => n.CreatedAt)
            .ProjectTo<NewsletterListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        ViewBag.SearchTerm = searchTerm;
        ViewBag.SelectedIsActive = isActive.HasValue ? isActive.Value.ToString().ToLower() : "";
        ViewBag.ActiveStatusList = new List<SelectListItem>
            {
                new SelectListItem { Value = "true", Text = "Đang hoạt động (Chưa hủy)" },
                new SelectListItem { Value = "false", Text = "Đã hủy đăng ký" }
            };

        return View(newslettersPaged);
    }

    // GET: Admin/Newsletter/Create
    public IActionResult Create()
    {
        ViewData["Title"] = "Thêm đăng ký Newsletter - Hệ thống quản trị";
        ViewData["PageTitle"] = "Thêm đăng ký mới (Thủ công)";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Đăng ký Bản tin", Url.Action(nameof(Index))),
            ("Thêm mới", "")
        };

        var viewModel = new NewsletterViewModel { IsActive = true };
        return View(viewModel);
    }

    // POST: Admin/Newsletter/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NewsletterViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var newsletter = _mapper.Map<Newsletter>(viewModel);
                if (newsletter.IsActive)
                {
                    newsletter.UnsubscribedAt = null;
                    newsletter.ConfirmedAt = DateTime.UtcNow;
                }
                else
                {
                    newsletter.UnsubscribedAt = DateTime.UtcNow;
                    newsletter.ConfirmedAt = null;
                }

                _context.Add(newsletter);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Manual Newsletter subscription for '{Email}' created successfully by {User}.", newsletter.Email, User.Identity?.Name ?? "Unknown");
                TempData["success"] = $"Thêm đăng ký cho '{newsletter.Email}' thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error creating newsletter subscription for '{Email}'. Check unique constraints.", viewModel.Email);
                if (ex.InnerException?.Message.Contains("UNIQUE constraint failed: newsletters.email", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    ModelState.AddModelError(nameof(viewModel.Email), "Địa chỉ email này đã được đăng ký.");
                }
                else
                {
                    ModelState.AddModelError("", "Lỗi cơ sở dữ liệu khi thêm đăng ký.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating newsletter subscription for '{Email}'.", viewModel.Email);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn.");
            }
        }
        else
        {
            _logger.LogWarning("Failed to create newsletter subscription for '{Email}'. Model state is invalid.", viewModel.Email);
        }

        ViewData["Title"] = "Thêm đăng ký Newsletter - Hệ thống quản trị";
        ViewData["PageTitle"] = "Thêm đăng ký mới (Thủ công)";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Đăng ký Bản tin", Url.Action(nameof(Index))), ("Thêm mới", "") };
        return View(viewModel);
    }

    // GET: Admin/Newsletter/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var newsletter = await _context.Set<Newsletter>().AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
        if (newsletter == null)
        {
            _logger.LogWarning("Edit GET: Newsletter subscription with ID {NewsletterId} not found.", id);
            return NotFound();
        }

        var viewModel = _mapper.Map<NewsletterViewModel>(newsletter);
        viewModel.IsActive = newsletter.UnsubscribedAt == null;

        ViewData["Title"] = "Chỉnh sửa đăng ký Newsletter - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa đăng ký: {newsletter.Email}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Đăng ký Bản tin", Url.Action(nameof(Index))),
            ($"Chỉnh sửa: {newsletter.Email}", "")
        };
        return View(viewModel);
    }

    // POST: Admin/Newsletter/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, NewsletterViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            _logger.LogWarning("Edit POST: ID mismatch. Route ID: {RouteId}, ViewModel ID: {ViewModelId}", id, viewModel.Id);
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            var newsletterToUpdate = await _context.Set<Newsletter>().FindAsync(id);
            if (newsletterToUpdate == null)
            {
                _logger.LogWarning("Edit POST: Newsletter subscription with ID {NewsletterId} not found for update.", id);
                TempData["error"] = "Không tìm thấy đăng ký để cập nhật.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _mapper.Map(viewModel, newsletterToUpdate);

                if (newsletterToUpdate.IsActive)
                {
                    newsletterToUpdate.UnsubscribedAt = null;
                    newsletterToUpdate.ConfirmedAt = DateTime.UtcNow;
                }
                else
                {
                    newsletterToUpdate.UnsubscribedAt = DateTime.UtcNow;
                    newsletterToUpdate.ConfirmedAt = null;
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("Newsletter subscription for '{Email}' (ID: {NewsletterId}) updated successfully by {User}.", newsletterToUpdate.Email, id, User.Identity?.Name ?? "Unknown");
                TempData["success"] = $"Cập nhật đăng ký cho '{newsletterToUpdate.Email}' thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating newsletter subscription for '{Email}' (ID: {NewsletterId}). Check unique constraints.", viewModel.Email, id);
                if (ex.InnerException?.Message.Contains("UNIQUE constraint failed: newsletters.email", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    ModelState.AddModelError(nameof(viewModel.Email), "Địa chỉ email này đã được đăng ký bởi một người khác.");
                }
                else
                {
                    ModelState.AddModelError("", "Lỗi cơ sở dữ liệu khi cập nhật đăng ký.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating newsletter subscription for '{Email}' (ID: {NewsletterId}).", viewModel.Email, id);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật.");
            }
        }
        else
        {
            _logger.LogWarning("Failed to update newsletter subscription for '{Email}' (ID: {NewsletterId}). Model state is invalid.", viewModel.Email, id);
        }


        ViewData["Title"] = "Chỉnh sửa đăng ký Newsletter - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa đăng ký: {viewModel.Email}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Đăng ký Bản tin", Url.Action(nameof(Index))), ($"Chỉnh sửa: {viewModel.Email}", "") };
        return View(viewModel);
    }

    // POST: Admin/Newsletter/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var newsletter = await _context.Set<Newsletter>().FindAsync(id);
        if (newsletter == null)
        {
            _logger.LogWarning("Delete POST: Newsletter subscription with ID {NewsletterId} not found.", id);
            return Json(new { success = false, message = "Không tìm thấy đăng ký." });
        }

        try
        {
            string email = newsletter.Email;
            _context.Remove(newsletter);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Newsletter subscription for '{Email}' (ID: {NewsletterId}) deleted successfully by {User}.", email, id, User.Identity?.Name ?? "Unknown");
            return Json(new { success = true, message = $"Xóa đăng ký của '{email}' thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting Newsletter subscription ID {NewsletterId} for email {Email}.", id, newsletter.Email);
            return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa đăng ký." });
        }
    }

    // GET: Admin/Newsletter/ExportExcel
    public async Task<IActionResult> ExportExcel(string? searchTerm = null, string? status = null)
    {
        var query = _context.Set<Newsletter>().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string lowerSearchTerm = searchTerm.Trim().ToLower();
            query = query.Where(n => n.Email.ToLower().Contains(lowerSearchTerm)
                                  || (n.Name != null && n.Name.ToLower().Contains(lowerSearchTerm)));
        }
        if (!string.IsNullOrEmpty(status))
        {
            if (bool.TryParse(status, out bool statusBool))
            {
                if (statusBool) query = query.Where(n => n.UnsubscribedAt == null);
                else query = query.Where(n => n.UnsubscribedAt != null);
            }
        }

        var data = await query
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new
            {
                n.Email,
                n.Name,
                Status = n.UnsubscribedAt == null ? "Hoạt động" : "Đã hủy",
                n.CreatedAt,
                n.ConfirmedAt,
                n.UnsubscribedAt
            })
            .ToListAsync();

        _logger.LogInformation("Exporting {Count} newsletter subscriptions to Excel.", data.Count);

        ExcelPackage.License.SetNonCommercialPersonal("Dai Minh Viet Nam");

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Newsletter_Subscriptions");

            // --- Headers ---
            worksheet.Cells[1, 1].Value = "Email";
            worksheet.Cells[1, 2].Value = "Tên";
            worksheet.Cells[1, 3].Value = "Trạng thái";
            worksheet.Cells[1, 4].Value = "Ngày đăng ký";
            worksheet.Cells[1, 5].Value = "Ngày xác nhận";
            worksheet.Cells[1, 6].Value = "Ngày hủy";

            // --- Header Formatting ---
            using (var range = worksheet.Cells[1, 1, 1, 6])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            // --- Data ---
            if (data.Any())
            {
                worksheet.Cells[2, 1].LoadFromCollection(data, false);

                // --- Date Formatting ---
                worksheet.Column(4).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
                worksheet.Column(5).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
                worksheet.Column(6).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
            }

            // --- AutoFit Columns ---
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            // --- Prepare File Download ---
            var stream = new MemoryStream();
            await package.SaveAsAsync(stream);
            stream.Position = 0;

            string excelName = $"NewsletterSubscriptions_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(stream, contentType, excelName);
        }
    }


}
