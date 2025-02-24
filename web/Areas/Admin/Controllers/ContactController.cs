using AutoMapper;
using core.Attributes;
using core.Common.Constants;
using core.Common.Enums;
using core.Common.Extensions;
using Core.Common.Extensions;
using Core.Common.Models;
using core.Entities;
using core.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Models.Contact;
using web.Areas.Admin.Requests.Contact;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class ContactController(
        IContactService contactService,
        IMapper mapper,
        IServiceProvider serviceProvider,
        IConfiguration configuration
    )
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class ContactController
{
    public async Task<IActionResult> Index()
    {
        List<Contact> contacts = await contactService.GetAllAsync();
        List<ContactViewModel> models = _mapper.Map<List<ContactViewModel>>(contacts);
        return View(models);
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        Contact? contact = await contactService.GetByIdAsync(id);
        if (contact == null) return NotFound();
        ContactUpdateRequest request = _mapper.Map<ContactUpdateRequest>(contact);

        ViewBag.ContactStatus = EnumExtensions.ToSelectList<ContactStatus>(request.ContactStatus);
        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Details(int id)
    {
        Contact? contact = await contactService.GetByIdAsync(id);
        if (contact == null) return NotFound();
        ContactViewModel contactDetail = _mapper.Map<ContactViewModel>(contact);
        return PartialView("_Detail.Modal", contactDetail);
    }
}

public partial class ContactController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ContactUpdateRequest request)
    {
        var validator = GetValidator<ContactUpdateRequest>();

        if (await this.ValidateAndReturnView(validator, request))
        {
            ViewBag.ContactStatus = EnumExtensions.ToSelectList<ContactStatus>(request.ContactStatus);
            return PartialView("_Edit.Modal", request);
        }

        try
        {
            var contact = await contactService.GetByIdAsync(request.Id);
            if (contact == null) return NotFound();

            _mapper.Map(request, contact);

            var response = await contactService.UpdateAsync(request.Id, contact);

            switch (response)
            {
                case SuccessResponse<Contact> successResponse:
                    ViewData["SuccessMessage"] = successResponse.Message;
                    if (Request.IsAjaxRequest())
                        return Json(new { redirectUrl = Url.Action("Index", "Contact", new { area = "Admin" }) });
                    return RedirectToAction("Index", "Contact", new { area = "Admin" });

                case ErrorResponse errorResponse:
                    foreach (var error in errorResponse.Errors) ModelState.AddModelError(error.Key, error.Value);

                    return PartialView("_Edit.Modal", request);

                default:
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    return PartialView("_Edit.Modal", request);
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return PartialView("_Edit.Modal", request);
        }
    }
}