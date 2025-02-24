using AutoMapper;
using core.Common.Extensions;
using Core.Common.Models;
using core.Entities;
using core.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> Create(ContactCreateRequest request)
    {
        var validator = GetValidator<ContactCreateRequest>();

        if (await this.ValidateAndReturnView(validator, request)) return View("Index", request);

        try
        {
            var model = _mapper.Map<Contact>(request);

            var response = await contactService.AddAsync(model);

            switch (response)
            {
                case SuccessResponse<Contact> successResponse:
                    TempData["SuccessMessage"] = "Cảm ơn bạn đã liên hệ! Chúng tôi sẽ phản hồi sớm.";

                    return RedirectToAction("Index");
                case ErrorResponse errorResponse:
                {
                    foreach (var error in errorResponse.Errors) ModelState.AddModelError(error.Key, error.Value);

                    return View("Index", request);
                }
                default:

                    return View("Index", request);
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View("Index", request);
        }
    }
}