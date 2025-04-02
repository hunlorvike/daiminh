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

        using (var package = new OfficeOpenXml.ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Liên hệ");

            // Thiết lập header
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Họ tên";
            worksheet.Cells[1, 3].Value = "Email";
            worksheet.Cells[1, 4].Value = "Điện thoại";
            worksheet.Cells[1, 5].Value = "Tiêu đề";
            worksheet.Cells[1, 6].Value = "Nội dung";
            worksheet.Cells[1, 7].Value = "Công ty";
            worksheet.Cells[1, 8].Value = "Chi tiết dự án";
            worksheet.Cells[1, 9].Value = "Trạng thái";
            worksheet.Cells[1, 10].Value = "Ghi chú admin";
            worksheet.Cells[1, 11].Value = "IP";
            worksheet.Cells[1, 12].Value = "User Agent";
            worksheet.Cells[1, 13].Value = "Ngày tạo";
            worksheet.Cells[1, 14].Value = "Ngày cập nhật";

            // Định dạng header
            using (var range = worksheet.Cells[1, 1, 1, 14])
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

                worksheet.Cells[row, 1].Value = contact.Id;
                worksheet.Cells[row, 2].Value = contact.FullName;
                worksheet.Cells[row, 3].Value = contact.Email;
                worksheet.Cells[row, 4].Value = contact.Phone;
                worksheet.Cells[row, 5].Value = contact.Subject;
                worksheet.Cells[row, 6].Value = contact.Message;
                worksheet.Cells[row, 7].Value = contact.CompanyName;
                worksheet.Cells[row, 8].Value = contact.ProjectDetails;
                worksheet.Cells[row, 9].Value = GetStatusDisplayName(contact.Status);
                worksheet.Cells[row, 10].Value = contact.AdminNotes;
                worksheet.Cells[row, 11].Value = contact.IpAddress;
                worksheet.Cells[row, 12].Value = contact.UserAgent;
                worksheet.Cells[row, 13].Value = contact.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss");
                worksheet.Cells[row, 14].Value = contact.UpdatedAt?.ToString("dd/MM/yyyy HH:mm:ss");

                // Định dạng trạng thái với màu sắc
                var statusCell = worksheet.Cells[row, 9];
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
            worksheet.Cells.AutoFitColumns();

            // Giới hạn độ rộng tối đa cho một số cột
            worksheet.Column(6).Width = 50; // Nội dung
            worksheet.Column(8).Width = 50; // Chi tiết dự án
            worksheet.Column(10).Width = 50; // Ghi chú admin
            worksheet.Column(12).Width = 50; // User Agent

            // Tạo tên file với timestamp
            string fileName = $"Lien_he_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

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
