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
public class ContactController(
    IContactService contactService,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(serviceProvider, configuration)
{
    public async Task<IActionResult> Index()
    {
        var response = await contactService.GetAllAsync();
        List<ContactViewModel> viewModels = response.Select(c => new ContactViewModel
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Phone = c.Phone,
            Status = c.Status.ToString(),
            CreatedAt = c.CreatedAt
        }).ToList();
        return View(viewModels);
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var response = await contactService.GetByIdAsync(id);
        if (response == null) return NotFound();
        ContactUpdateRequest request = new()
        {
            Id = response.Id,
            ContactStatus = response.Status
        };

        ViewBag.ContactStatus = EnumExtensions.ToSelectList<ContactStatus>(request.ContactStatus);
        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Details(int id)
    {
        var response = await contactService.GetByIdAsync(id);
        if (response == null) return NotFound();

        var contactDetail = new ContactViewModel()
        {
            Id = response.Id,
            Name = response.Name,
            Email = response.Email,
            Phone = response.Phone,
            Message = response.Message,
            Status = response.Status.ToString()
        };

        return PartialView("_Detail.Modal", contactDetail);
    }

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

            contact.Status = request.ContactStatus;

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