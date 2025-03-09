using application.Interfaces;
using AutoMapper;
using domain.Constants;
using domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shared.Enums;
using shared.Extensions;
using shared.Models;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Models.Contact;
using web.Areas.Admin.Requests.Contact;
using web.Attributes;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class ContactController(
    IContactService contactService,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class ContactController
{
    public async Task<IActionResult> Index()
    {
        var contacts = await contactService.GetAllAsync();
        List<ContactViewModel> models = _mapper.Map<List<ContactViewModel>>(contacts);
        return View(models);
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var contact = await contactService.GetByIdAsync(id);
        if (contact == null) return NotFound();
        var request = _mapper.Map<ContactUpdateRequest>(contact);

        ViewBag.ContactStatus = EnumExtensions.ToSelectList<ContactStatus>();
        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Details(int id)
    {
        var contact = await contactService.GetByIdAsync(id);
        if (contact == null) return NotFound();
        var contactDetail = _mapper.Map<ContactViewModel>(contact);
        return PartialView("_Detail.Modal", contactDetail);
    }
}

public partial class ContactController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ContactUpdateRequest model)
    {
        var validator = GetValidator<ContactUpdateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var contact = await contactService.GetByIdAsync(model.Id);
            if (contact == null) return NotFound();

            _mapper.Map(model, contact);

            var response = await contactService.UpdateAsync(model.Id, contact);

            switch (response)
            {
                case SuccessResponse<Contact> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "Contact", new { area = "Admin" })
                    });
                case SuccessResponse<Contact> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "Contact", new { area = "Admin" });
                case ErrorResponse errorResponse when Request.IsAjaxRequest():
                    return BadRequest(errorResponse);
                case ErrorResponse errorResponse:
                {
                    return BadRequest(errorResponse);
                }
            }

            return PartialView("_Edit.Modal", model);
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Success = false,
                Errors = ex.Message
            });
        }
    }
}