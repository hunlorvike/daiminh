using application.Interfaces;
using AutoMapper;
using domain.Entities;
using Microsoft.AspNetCore.Mvc;
using shared.Extensions;
using shared.Models;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Client.Requests.Contact;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("/lien-he")]
public partial class ContactController(
    IContactService contactService,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration) : DaiminhController(mapper, serviceProvider, configuration);

public partial class ContactController
{
    public IActionResult Index()
    {
        return View();
    }
}

public partial class ContactController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContactCreateRequest model)
    {
        var validator = GetValidator<ContactCreateRequest>();

        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var contact = _mapper.Map<Contact>(model);

            var response = await contactService.AddAsync(contact);

            switch (response)
            {
                case SuccessResponse<Contact> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "Contact", new { area = "Client" })
                    });
                case SuccessResponse<Contact> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "Contact", new { area = "Client" });
                case ErrorResponse errorResponse when Request.IsAjaxRequest():
                    return BadRequest(errorResponse);
                case ErrorResponse errorResponse:
                    {
                        return BadRequest(errorResponse);
                    }
            }

            return RedirectToAction("Index", "Contact", new { area = "Client" });
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