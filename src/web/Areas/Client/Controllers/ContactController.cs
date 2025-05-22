using AutoMapper;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Constants;
using shared.Enums;
using shared.Models;
using System.Text.Json;
using web.Areas.Client.Validators;
using web.Areas.Client.ViewModels;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public partial class ContactController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ContactController> _logger;
    private readonly IMapper _mapper;

    public ContactController(
        ApplicationDbContext context,
        ILogger<ContactController> logger,
        IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IActionResult Index()
    {
        var viewModel = new ContactViewModel
        {
            SubjectOptions = GetSubjectOptions(),
        };
        return View(viewModel);
    }

    public IActionResult Quote()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactViewModel viewModel)
    {
        var result = await new ContactViewModelValidator().ValidateAsync(viewModel);
        viewModel.SubjectOptions = GetSubjectOptions();
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        Contact contact = _mapper.Map<Contact>(viewModel);
        contact.Status = ContactStatus.New;
        contact.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        contact.UserAgent = HttpContext.Request.Headers["User-Agent"].ToString() ?? string.Empty;
        await _context.Contacts.AddAsync(contact);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi thêm liên hệ");
            ModelState.AddModelError("", "Đã xảy ra lỗi hệ thống khi cập nhật liên hệ.");
            return View(viewModel);
        }

        TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
            new ToastData("Thành công", "Cảm ơn bạn đã gửi liên hệ. Chúng tôi sẽ phản hồi sớm nhất có thể.", ToastType.Success)
        );

        return RedirectToAction(nameof(Index));
    }
}

public partial class ContactController
{
    private List<SelectListItem> GetSubjectOptions()
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Chọn chủ đề --", Disabled = true, Selected = true },
            new SelectListItem { Value = "Hỗ trợ sản phẩm", Text = "Hỗ trợ sản phẩm" },
            new SelectListItem { Value = "Tư vấn kỹ thuật", Text = "Tư vấn kỹ thuật" },
            new SelectListItem { Value = "Báo giá/Đặt hàng", Text = "Báo giá/Đặt hàng" },
            new SelectListItem { Value = "Góp ý khác", Text = "Góp ý khác" }
        };
    }
}