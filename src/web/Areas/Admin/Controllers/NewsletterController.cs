using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Newsletter;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class NewsletterController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<NewsletterViewModel> _validator;

    public NewsletterController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<NewsletterViewModel> validator)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
    }

    // GET: Admin/Newsletter
    public async Task<IActionResult> Index(bool? isActive = null, string? searchTerm = null)
    {
        ViewData["PageTitle"] = "Quản lý đăng ký nhận tin";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Đăng ký nhận tin", "")
        };

        IQueryable<Newsletter> query = _context.Set<Newsletter>().AsQueryable();

        if (isActive.HasValue)
        {
            query = query.Where(n => n.IsActive == isActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(n =>
                n.Email.Contains(searchTerm) ||
                (n.Name != null && n.Name.Contains(searchTerm)));
        }

        var newsletters = await query
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        var viewModels = _mapper.Map<List<NewsletterListItemViewModel>>(newsletters);

        ViewBag.IsActiveFilter = isActive;
        ViewBag.SearchTerm = searchTerm;
        ViewBag.ActiveCount = await _context.Set<Newsletter>().CountAsync(n => n.IsActive);
        ViewBag.InactiveCount = await _context.Set<Newsletter>().CountAsync(n => !n.IsActive);

        return View(viewModels);
    }

    // GET: Admin/Newsletter/Details/5
    public async Task<IActionResult> Details(int id)
    {
        ViewData["PageTitle"] = "Chi tiết đăng ký nhận tin";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Đăng ký nhận tin", "/Admin/Newsletter"),
            ("Chi tiết", "")
        };

        var newsletter = await _context.Set<Newsletter>().FindAsync(id);

        if (newsletter == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<NewsletterViewModel>(newsletter);

        return View(viewModel);
    }

    // GET: Admin/Newsletter/Create
    public IActionResult Create()
    {
        ViewData["PageTitle"] = "Thêm đăng ký nhận tin";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Đăng ký nhận tin", "/Admin/Newsletter"),
            ("Thêm mới", "")
        };

        return View(new NewsletterViewModel());
    }

    // POST: Admin/Newsletter/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NewsletterViewModel viewModel)
    {
        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);

            ViewData["PageTitle"] = "Thêm đăng ký nhận tin";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
            {
                ("Đăng ký nhận tin", "/Admin/Newsletter"),
                ("Thêm mới", "")
            };

            return View(viewModel);
        }

        // Kiểm tra email đã tồn tại chưa
        if (await _context.Set<Newsletter>().AnyAsync(n => n.Email == viewModel.Email))
        {
            ModelState.AddModelError("Email", "Email này đã đăng ký nhận tin");

            ViewData["PageTitle"] = "Thêm đăng ký nhận tin";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
            {
                ("Đăng ký nhận tin", "/Admin/Newsletter"),
                ("Thêm mới", "")
            };

            return View(viewModel);
        }

        var newsletter = _mapper.Map<Newsletter>(viewModel);
        newsletter.CreatedAt = DateTime.UtcNow;
        newsletter.ConfirmedAt = DateTime.UtcNow; // Đăng ký từ admin mặc định đã xác nhận

        _context.Set<Newsletter>().Add(newsletter);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Thêm đăng ký nhận tin thành công";
        return RedirectToAction(nameof(Index));
    }

    // GET: Admin/Newsletter/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["PageTitle"] = "Sửa đăng ký nhận tin";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Đăng ký nhận tin", "/Admin/Newsletter"),
            ("Sửa", "")
        };

        var newsletter = await _context.Set<Newsletter>().FindAsync(id);

        if (newsletter == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<NewsletterViewModel>(newsletter);

        return View(viewModel);
    }

    // POST: Admin/Newsletter/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, NewsletterViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);

            ViewData["PageTitle"] = "Sửa đăng ký nhận tin";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
            {
                ("Đăng ký nhận tin", "/Admin/Newsletter"),
                ("Sửa", "")
            };

            return View(viewModel);
        }

        try
        {
            var newsletter = await _context.Set<Newsletter>().FindAsync(id);

            if (newsletter == null)
            {
                return NotFound();
            }

            // Kiểm tra email đã tồn tại chưa (nếu thay đổi email)
            if (newsletter.Email != viewModel.Email &&
                await _context.Set<Newsletter>().AnyAsync(n => n.Email == viewModel.Email))
            {
                ModelState.AddModelError("Email", "Email này đã đăng ký nhận tin");

                ViewData["PageTitle"] = "Sửa đăng ký nhận tin";
                ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
                {
                    ("Đăng ký nhận tin", "/Admin/Newsletter"),
                    ("Sửa", "")
                };

                return View(viewModel);
            }

            newsletter.Email = viewModel.Email;
            newsletter.Name = viewModel.Name;
            newsletter.IsActive = viewModel.IsActive;

            // Cập nhật trạng thái đăng ký/hủy đăng ký
            if (newsletter.IsActive && !viewModel.IsActive)
            {
                newsletter.UnsubscribedAt = DateTime.UtcNow;
            }
            else if (!newsletter.IsActive && viewModel.IsActive)
            {
                newsletter.UnsubscribedAt = null;
                if (newsletter.ConfirmedAt == null)
                {
                    newsletter.ConfirmedAt = DateTime.UtcNow;
                }
            }

            newsletter.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật đăng ký nhận tin thành công";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await NewsletterExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
    }

    // POST: Admin/Newsletter/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var newsletter = await _context.Set<Newsletter>().FindAsync(id);

        if (newsletter == null)
        {
            return Json(new { success = false, message = "Không tìm thấy đăng ký nhận tin" });
        }

        _context.Set<Newsletter>().Remove(newsletter);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Xóa đăng ký nhận tin thành công" });
    }

    // POST: Admin/Newsletter/ToggleStatus/5
    [HttpPost]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var newsletter = await _context.Set<Newsletter>().FindAsync(id);

        if (newsletter == null)
        {
            return Json(new { success = false, message = "Không tìm thấy đăng ký nhận tin" });
        }

        newsletter.IsActive = !newsletter.IsActive;

        if (newsletter.IsActive)
        {
            newsletter.UnsubscribedAt = null;
            if (newsletter.ConfirmedAt == null)
            {
                newsletter.ConfirmedAt = DateTime.UtcNow;
            }
        }
        else
        {
            newsletter.UnsubscribedAt = DateTime.UtcNow;
        }

        newsletter.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Json(new
        {
            success = true,
            active = newsletter.IsActive,
            message = newsletter.IsActive
                ? "Kích hoạt đăng ký nhận tin thành công"
                : "Hủy đăng ký nhận tin thành công"
        });
    }

    // GET: Admin/Newsletter/Export
    public async Task<IActionResult> Export(bool? isActive = null, string? searchTerm = null)
    {
        IQueryable<Newsletter> query = _context.Set<Newsletter>().AsQueryable();

        if (isActive.HasValue)
        {
            query = query.Where(n => n.IsActive == isActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(n =>
                n.Email.Contains(searchTerm) ||
                (n.Name != null && n.Name.Contains(searchTerm)));
        }

        var newsletters = await query
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        // Tạo file Excel
        using (var package = new OfficeOpenXml.ExcelPackage())
        {
            // Worksheet dữ liệu chi tiết
            var detailsSheet = package.Workbook.Worksheets.Add("Danh sách đăng ký");

            // Thiết lập header
            detailsSheet.Cells[1, 1].Value = "ID";
            detailsSheet.Cells[1, 2].Value = "Email";
            detailsSheet.Cells[1, 3].Value = "Tên";
            detailsSheet.Cells[1, 4].Value = "Trạng thái";
            detailsSheet.Cells[1, 5].Value = "Ngày đăng ký";
            detailsSheet.Cells[1, 6].Value = "Ngày xác nhận";
            detailsSheet.Cells[1, 7].Value = "Ngày hủy đăng ký";
            detailsSheet.Cells[1, 8].Value = "IP Address";
            detailsSheet.Cells[1, 9].Value = "User Agent";

            // Định dạng header
            using (var range = detailsSheet.Cells[1, 1, 1, 9])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                range.Style.Font.Color.SetColor(System.Drawing.Color.Black);
            }

            // Điền dữ liệu
            for (int i = 0; i < newsletters.Count; i++)
            {
                var newsletter = newsletters[i];
                int row = i + 2; // Bắt đầu từ dòng 2 (sau header)

                detailsSheet.Cells[row, 1].Value = newsletter.Id;
                detailsSheet.Cells[row, 2].Value = newsletter.Email;
                detailsSheet.Cells[row, 3].Value = newsletter.Name;
                detailsSheet.Cells[row, 4].Value = newsletter.IsActive ? "Đang hoạt động" : "Đã hủy đăng ký";
                detailsSheet.Cells[row, 5].Value = newsletter.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss");
                detailsSheet.Cells[row, 6].Value = newsletter.ConfirmedAt?.ToString("dd/MM/yyyy HH:mm:ss");
                detailsSheet.Cells[row, 7].Value = newsletter.UnsubscribedAt?.ToString("dd/MM/yyyy HH:mm:ss");
                detailsSheet.Cells[row, 8].Value = newsletter.IpAddress;
                detailsSheet.Cells[row, 9].Value = newsletter.UserAgent;

                // Định dạng trạng thái với màu sắc
                var statusCell = detailsSheet.Cells[row, 4];
                if (newsletter.IsActive)
                {
                    statusCell.Style.Font.Color.SetColor(System.Drawing.Color.Green);
                }
                else
                {
                    statusCell.Style.Font.Color.SetColor(System.Drawing.Color.Red);
                }
            }

            // Tự động điều chỉnh độ rộng cột
            detailsSheet.Cells.AutoFitColumns();

            // Giới hạn độ rộng tối đa cho một số cột
            detailsSheet.Column(9).Width = 50; // User Agent

            // Tạo worksheet tổng hợp
            var summarySheet = package.Workbook.Worksheets.Add("Tổng hợp");

            // Tiêu đề báo cáo
            summarySheet.Cells[1, 1].Value = "BÁO CÁO TỔNG HỢP ĐĂNG KÝ NHẬN TIN";
            using (var range = summarySheet.Cells[1, 1, 1, 5])
            {
                range.Merge = true;
                range.Style.Font.Bold = true;
                range.Style.Font.Size = 16;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }

            // Thông tin báo cáo
            summarySheet.Cells[3, 1].Value = "Ngày xuất báo cáo:";
            summarySheet.Cells[3, 2].Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            if (isActive.HasValue)
            {
                summarySheet.Cells[4, 1].Value = "Lọc theo trạng thái:";
                summarySheet.Cells[4, 2].Value = isActive.Value ? "Đang hoạt động" : "Đã hủy đăng ký";
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                summarySheet.Cells[5, 1].Value = "Từ khóa tìm kiếm:";
                summarySheet.Cells[5, 2].Value = searchTerm;
            }

            summarySheet.Cells[6, 1].Value = "Tổng số đăng ký:";
            summarySheet.Cells[6, 2].Value = newsletters.Count;

            // Thống kê theo trạng thái
            summarySheet.Cells[8, 1].Value = "THỐNG KÊ THEO TRẠNG THÁI";
            using (var range = summarySheet.Cells[8, 1, 8, 5])
            {
                range.Merge = true;
                range.Style.Font.Bold = true;
                range.Style.Font.Size = 14;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }

            // Header thống kê
            summarySheet.Cells[10, 1].Value = "Trạng thái";
            summarySheet.Cells[10, 2].Value = "Số lượng";
            summarySheet.Cells[10, 3].Value = "Tỷ lệ";

            using (var range = summarySheet.Cells[10, 1, 10, 3])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            // Dữ liệu thống kê
            var activeCount = newsletters.Count(n => n.IsActive);
            var inactiveCount = newsletters.Count(n => !n.IsActive);
            var totalCount = newsletters.Count;

            // Đang hoạt động
            summarySheet.Cells[11, 1].Value = "Đang hoạt động";
            summarySheet.Cells[11, 2].Value = activeCount;
            summarySheet.Cells[11, 3].Value = totalCount > 0 ? (double)activeCount / totalCount : 0;
            summarySheet.Cells[11, 3].Style.Numberformat.Format = "0.00%";
            summarySheet.Cells[11, 1].Style.Font.Color.SetColor(System.Drawing.Color.Green);

            // Đã hủy đăng ký
            summarySheet.Cells[12, 1].Value = "Đã hủy đăng ký";
            summarySheet.Cells[12, 2].Value = inactiveCount;
            summarySheet.Cells[12, 3].Value = totalCount > 0 ? (double)inactiveCount / totalCount : 0;
            summarySheet.Cells[12, 3].Style.Numberformat.Format = "0.00%";
            summarySheet.Cells[12, 1].Style.Font.Color.SetColor(System.Drawing.Color.Red);

            // Tổng
            summarySheet.Cells[13, 1].Value = "Tổng cộng";
            summarySheet.Cells[13, 2].Value = totalCount;
            summarySheet.Cells[13, 3].Value = 1;
            summarySheet.Cells[13, 3].Style.Numberformat.Format = "0.00%";
            summarySheet.Cells[13, 1].Style.Font.Bold = true;
            summarySheet.Cells[13, 2].Style.Font.Bold = true;

            // Tạo biểu đồ
            if (totalCount > 0)
            {
                var chart = summarySheet.Drawings.AddChart("pieChart", OfficeOpenXml.Drawing.Chart.eChartType.Pie);
                chart.SetPosition(10, 0, 4, 0); // Vị trí của biểu đồ
                chart.SetSize(500, 300); // Kích thước biểu đồ

                // Dữ liệu cho biểu đồ
                var series = chart.Series.Add(summarySheet.Cells[11, 2, 12, 2], summarySheet.Cells[11, 1, 12, 1]);
                series.Header = "Phân bố trạng thái";

                // Tiêu đề biểu đồ
                chart.Title.Text = "Biểu đồ phân bố trạng thái đăng ký nhận tin";
                chart.Legend.Position = OfficeOpenXml.Drawing.Chart.eLegendPosition.Bottom;
            }

            // Thêm thống kê theo thời gian
            summarySheet.Cells[15, 1].Value = "THỐNG KÊ THEO THỜI GIAN";
            using (var range = summarySheet.Cells[15, 1, 15, 5])
            {
                range.Merge = true;
                range.Style.Font.Bold = true;
                range.Style.Font.Size = 14;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }

            // Header thống kê thời gian
            summarySheet.Cells[17, 1].Value = "Khoảng thời gian";
            summarySheet.Cells[17, 2].Value = "Số lượng đăng ký";
            summarySheet.Cells[17, 3].Value = "Số lượng hủy";

            using (var range = summarySheet.Cells[17, 1, 17, 3])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            // Lấy thời gian hiện tại
            var now = DateTime.UtcNow;

            // Thống kê theo thời gian
            // Hôm nay
            var today = now.Date;
            var todaySignups = newsletters.Count(n => n.CreatedAt.Date == today);
            var todayUnsubscribes = newsletters.Count(n => n.UnsubscribedAt.HasValue && n.UnsubscribedAt.Value.Date == today);

            // 7 ngày qua
            var last7Days = now.AddDays(-7);
            var last7DaysSignups = newsletters.Count(n => n.CreatedAt >= last7Days);
            var last7DaysUnsubscribes = newsletters.Count(n => n.UnsubscribedAt.HasValue && n.UnsubscribedAt.Value >= last7Days);

            // 30 ngày qua
            var last30Days = now.AddDays(-30);
            var last30DaysSignups = newsletters.Count(n => n.CreatedAt >= last30Days);
            var last30DaysUnsubscribes = newsletters.Count(n => n.UnsubscribedAt.HasValue && n.UnsubscribedAt.Value >= last30Days);

            // Tháng này
            var thisMonth = new DateTime(now.Year, now.Month, 1);
            var thisMonthSignups = newsletters.Count(n => n.CreatedAt >= thisMonth);
            var thisMonthUnsubscribes = newsletters.Count(n => n.UnsubscribedAt.HasValue && n.UnsubscribedAt.Value >= thisMonth);

            // Điền dữ liệu thống kê thời gian
            summarySheet.Cells[18, 1].Value = "Hôm nay";
            summarySheet.Cells[18, 2].Value = todaySignups;
            summarySheet.Cells[18, 3].Value = todayUnsubscribes;

            summarySheet.Cells[19, 1].Value = "7 ngày qua";
            summarySheet.Cells[19, 2].Value = last7DaysSignups;
            summarySheet.Cells[19, 3].Value = last7DaysUnsubscribes;

            summarySheet.Cells[20, 1].Value = "30 ngày qua";
            summarySheet.Cells[20, 2].Value = last30DaysSignups;
            summarySheet.Cells[20, 3].Value = last30DaysUnsubscribes;

            summarySheet.Cells[21, 1].Value = "Tháng này";
            summarySheet.Cells[21, 2].Value = thisMonthSignups;
            summarySheet.Cells[21, 3].Value = thisMonthUnsubscribes;

            // Điều chỉnh độ rộng cột cho sheet tổng hợp
            summarySheet.Cells.AutoFitColumns();

            // Tạo tên file với timestamp
            string fileName = $"Bao_cao_dang_ky_nhan_tin_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            // Trả về file Excel
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileContents = package.GetAsByteArray();

            return File(fileContents, contentType, fileName);
        }
    }

    private async Task<bool> NewsletterExists(int id)
    {
        return await _context.Set<Newsletter>().AnyAsync(e => e.Id == id);
    }
}
