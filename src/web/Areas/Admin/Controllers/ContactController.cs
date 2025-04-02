using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Admin.ViewModels.Contact;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ContactController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<ContactViewModel> _validator;

    public ContactController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<ContactViewModel> validator)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
    }

    // GET: Admin/Contact
    public async Task<IActionResult> Index(ContactStatus? status = null, string? searchTerm = null)
    {
        ViewData["PageTitle"] = "Quản lý liên hệ";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Liên hệ", "")
        };

        IQueryable<Contact> query = _context.Set<Contact>().AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(c => c.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c =>
                c.FullName.Contains(searchTerm) ||
                c.Email.Contains(searchTerm) ||
                c.Subject.Contains(searchTerm) ||
                c.Message.Contains(searchTerm));
        }

        var contacts = await query
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        var viewModels = _mapper.Map<List<ContactListItemViewModel>>(contacts);

        ViewBag.StatusFilter = status;
        ViewBag.SearchTerm = searchTerm;
        ViewBag.NewCount = await _context.Set<Contact>().CountAsync(c => c.Status == ContactStatus.New);
        ViewBag.InProgressCount = await _context.Set<Contact>().CountAsync(c => c.Status == ContactStatus.InProgress);
        ViewBag.CompletedCount = await _context.Set<Contact>().CountAsync(c => c.Status == ContactStatus.Completed);
        ViewBag.SpamCount = await _context.Set<Contact>().CountAsync(c => c.Status == ContactStatus.Spam);

        return View(viewModels);
    }

    // GET: Admin/Contact/Details/5
    public async Task<IActionResult> Details(int id)
    {
        ViewData["PageTitle"] = "Chi tiết liên hệ";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Liên hệ", "/Admin/Contact"),
            ("Chi tiết", "")
        };

        var contact = await _context.Set<Contact>().FindAsync(id);

        if (contact == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<ContactViewModel>(contact);

        return View(viewModel);
    }

    // GET: Admin/Contact/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["PageTitle"] = "Cập nhật liên hệ";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Liên hệ", "/Admin/Contact"),
            ("Cập nhật", "")
        };

        var contact = await _context.Set<Contact>().FindAsync(id);

        if (contact == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<ContactViewModel>(contact);

        return View(viewModel);
    }

    // POST: Admin/Contact/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ContactViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);

            ViewData["PageTitle"] = "Cập nhật liên hệ";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
            {
                ("Liên hệ", "/Admin/Contact"),
                ("Cập nhật", "")
            };

            return View(viewModel);
        }

        try
        {
            var contact = await _context.Set<Contact>().FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            // Cập nhật các trường có thể chỉnh sửa
            contact.Status = viewModel.Status;
            contact.AdminNotes = viewModel.AdminNotes;
            contact.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật liên hệ thành công";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ContactExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
    }

    // POST: Admin/Contact/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var contact = await _context.Set<Contact>().FindAsync(id);

        if (contact == null)
        {
            return Json(new { success = false, message = "Không tìm thấy liên hệ" });
        }

        _context.Set<Contact>().Remove(contact);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Xóa liên hệ thành công" });
    }

    // POST: Admin/Contact/ChangeStatus/5
    [HttpPost]
    public async Task<IActionResult> ChangeStatus(int id, ContactStatus status)
    {
        var contact = await _context.Set<Contact>().FindAsync(id);

        if (contact == null)
        {
            return Json(new { success = false, message = "Không tìm thấy liên hệ" });
        }

        contact.Status = status;
        contact.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        string statusText = status switch
        {
            ContactStatus.New => "Mới",
            ContactStatus.InProgress => "Đang xử lý",
            ContactStatus.Completed => "Đã xử lý",
            ContactStatus.Spam => "Spam",
            _ => status.ToString()
        };

        return Json(new { success = true, message = $"Đã chuyển trạng thái thành {statusText}" });
    }

    // GET: Admin/Contact/Export
    public async Task<IActionResult> Export(ContactStatus? status = null, string? searchTerm = null)
    {
        // Xây dựng query tương tự như trong Index
        IQueryable<Contact> query = _context.Set<Contact>().AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(c => c.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c =>
                c.FullName.Contains(searchTerm) ||
                c.Email.Contains(searchTerm) ||
                c.Subject.Contains(searchTerm) ||
                c.Message.Contains(searchTerm));
        }

        var contacts = await query
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        // Tạo file Excel
        using (var package = new OfficeOpenXml.ExcelPackage())
        {
            // Worksheet dữ liệu chi tiết
            var detailsSheet = package.Workbook.Worksheets.Add("Chi tiết liên hệ");

            // Thiết lập header
            detailsSheet.Cells[1, 1].Value = "ID";
            detailsSheet.Cells[1, 2].Value = "Họ tên";
            detailsSheet.Cells[1, 3].Value = "Email";
            detailsSheet.Cells[1, 4].Value = "Điện thoại";
            detailsSheet.Cells[1, 5].Value = "Tiêu đề";
            detailsSheet.Cells[1, 6].Value = "Nội dung";
            detailsSheet.Cells[1, 7].Value = "Công ty";
            detailsSheet.Cells[1, 8].Value = "Chi tiết dự án";
            detailsSheet.Cells[1, 9].Value = "Trạng thái";
            detailsSheet.Cells[1, 10].Value = "Ghi chú admin";
            detailsSheet.Cells[1, 11].Value = "IP";
            detailsSheet.Cells[1, 12].Value = "User Agent";
            detailsSheet.Cells[1, 13].Value = "Ngày tạo";
            detailsSheet.Cells[1, 14].Value = "Ngày cập nhật";

            // Định dạng header
            using (var range = detailsSheet.Cells[1, 1, 1, 14])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                range.Style.Font.Color.SetColor(System.Drawing.Color.Black);
            }

            // Điền dữ liệu
            for (int i = 0; i < contacts.Count; i++)
            {
                var contact = contacts[i];
                int row = i + 2; // Bắt đầu từ dòng 2 (sau header)

                detailsSheet.Cells[row, 1].Value = contact.Id;
                detailsSheet.Cells[row, 2].Value = contact.FullName;
                detailsSheet.Cells[row, 3].Value = contact.Email;
                detailsSheet.Cells[row, 4].Value = contact.Phone;
                detailsSheet.Cells[row, 5].Value = contact.Subject;
                detailsSheet.Cells[row, 6].Value = contact.Message;
                detailsSheet.Cells[row, 7].Value = contact.CompanyName;
                detailsSheet.Cells[row, 8].Value = contact.ProjectDetails;
                detailsSheet.Cells[row, 9].Value = GetStatusDisplayName(contact.Status);
                detailsSheet.Cells[row, 10].Value = contact.AdminNotes;
                detailsSheet.Cells[row, 11].Value = contact.IpAddress;
                detailsSheet.Cells[row, 12].Value = contact.UserAgent;
                detailsSheet.Cells[row, 13].Value = contact.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss");
                detailsSheet.Cells[row, 14].Value = contact.UpdatedAt?.ToString("dd/MM/yyyy HH:mm:ss");

                // Định dạng trạng thái với màu sắc
                var statusCell = detailsSheet.Cells[row, 9];
                switch (contact.Status)
                {
                    case ContactStatus.New:
                        statusCell.Style.Font.Color.SetColor(System.Drawing.Color.Blue);
                        break;
                    case ContactStatus.InProgress:
                        statusCell.Style.Font.Color.SetColor(System.Drawing.Color.Orange);
                        break;
                    case ContactStatus.Completed:
                        statusCell.Style.Font.Color.SetColor(System.Drawing.Color.Green);
                        break;
                    case ContactStatus.Spam:
                        statusCell.Style.Font.Color.SetColor(System.Drawing.Color.Red);
                        break;
                }
            }

            // Tự động điều chỉnh độ rộng cột
            detailsSheet.Cells.AutoFitColumns();

            // Giới hạn độ rộng tối đa cho một số cột
            detailsSheet.Column(6).Width = 50; // Nội dung
            detailsSheet.Column(8).Width = 50; // Chi tiết dự án
            detailsSheet.Column(10).Width = 50; // Ghi chú admin
            detailsSheet.Column(12).Width = 50; // User Agent

            // Tạo worksheet tổng hợp
            var summarySheet = package.Workbook.Worksheets.Add("Tổng hợp");

            // Tiêu đề báo cáo
            summarySheet.Cells[1, 1].Value = "BÁO CÁO TỔNG HỢP LIÊN HỆ";
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

            if (status.HasValue)
            {
                summarySheet.Cells[4, 1].Value = "Lọc theo trạng thái:";
                summarySheet.Cells[4, 2].Value = GetStatusDisplayName(status.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                summarySheet.Cells[5, 1].Value = "Từ khóa tìm kiếm:";
                summarySheet.Cells[5, 2].Value = searchTerm;
            }

            summarySheet.Cells[6, 1].Value = "Tổng số liên hệ:";
            summarySheet.Cells[6, 2].Value = contacts.Count;

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
            var newCount = contacts.Count(c => c.Status == ContactStatus.New);
            var inProgressCount = contacts.Count(c => c.Status == ContactStatus.InProgress);
            var completedCount = contacts.Count(c => c.Status == ContactStatus.Completed);
            var spamCount = contacts.Count(c => c.Status == ContactStatus.Spam);
            var totalCount = contacts.Count;

            // Mới
            summarySheet.Cells[11, 1].Value = "Mới";
            summarySheet.Cells[11, 2].Value = newCount;
            summarySheet.Cells[11, 3].Value = totalCount > 0 ? (double)newCount / totalCount : 0;
            summarySheet.Cells[11, 3].Style.Numberformat.Format = "0.00%";
            summarySheet.Cells[11, 1].Style.Font.Color.SetColor(System.Drawing.Color.Blue);

            // Đang xử lý
            summarySheet.Cells[12, 1].Value = "Đang xử lý";
            summarySheet.Cells[12, 2].Value = inProgressCount;
            summarySheet.Cells[12, 3].Value = totalCount > 0 ? (double)inProgressCount / totalCount : 0;
            summarySheet.Cells[12, 3].Style.Numberformat.Format = "0.00%";
            summarySheet.Cells[12, 1].Style.Font.Color.SetColor(System.Drawing.Color.Orange);

            // Đã xử lý
            summarySheet.Cells[13, 1].Value = "Đã xử lý";
            summarySheet.Cells[13, 2].Value = completedCount;
            summarySheet.Cells[13, 3].Value = totalCount > 0 ? (double)completedCount / totalCount : 0;
            summarySheet.Cells[13, 3].Style.Numberformat.Format = "0.00%";
            summarySheet.Cells[13, 1].Style.Font.Color.SetColor(System.Drawing.Color.Green);

            // Spam
            summarySheet.Cells[14, 1].Value = "Spam";
            summarySheet.Cells[14, 2].Value = spamCount;
            summarySheet.Cells[14, 3].Value = totalCount > 0 ? (double)spamCount / totalCount : 0;
            summarySheet.Cells[14, 3].Style.Numberformat.Format = "0.00%";
            summarySheet.Cells[14, 1].Style.Font.Color.SetColor(System.Drawing.Color.Red);

            // Tổng
            summarySheet.Cells[15, 1].Value = "Tổng cộng";
            summarySheet.Cells[15, 2].Value = totalCount;
            summarySheet.Cells[15, 3].Value = 1;
            summarySheet.Cells[15, 3].Style.Numberformat.Format = "0.00%";
            summarySheet.Cells[15, 1].Style.Font.Bold = true;
            summarySheet.Cells[15, 2].Style.Font.Bold = true;

            // Tạo biểu đồ
            if (totalCount > 0)
            {
                var chart = summarySheet.Drawings.AddChart("pieChart", OfficeOpenXml.Drawing.Chart.eChartType.Pie);
                chart.SetPosition(10, 0, 4, 0); // Vị trí của biểu đồ
                chart.SetSize(500, 300); // Kích thước biểu đồ

                // Dữ liệu cho biểu đồ
                var series = chart.Series.Add(summarySheet.Cells[11, 2, 14, 2], summarySheet.Cells[11, 1, 14, 1]);
                series.Header = "Phân bố trạng thái";

                // Tiêu đề biểu đồ
                chart.Title.Text = "Biểu đồ phân bố trạng thái liên hệ";
                chart.Legend.Position = OfficeOpenXml.Drawing.Chart.eLegendPosition.Bottom;
            }

            // Điều chỉnh độ rộng cột cho sheet tổng hợp
            summarySheet.Cells.AutoFitColumns();

            // Tạo tên file với timestamp
            string fileName = $"Bao_cao_lien_he_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            // Trả về file Excel
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileContents = package.GetAsByteArray();

            return File(fileContents, contentType, fileName);
        }
    }

    private string GetStatusDisplayName(ContactStatus status)
    {
        return status switch
        {
            ContactStatus.New => "Mới",
            ContactStatus.InProgress => "Đang xử lý",
            ContactStatus.Completed => "Đã xử lý",
            ContactStatus.Spam => "Spam",
            _ => status.ToString()
        };
    }

    private async Task<bool> ContactExists(int id)
    {
        return await _context.Set<Contact>().AnyAsync(e => e.Id == id);
    }
}
