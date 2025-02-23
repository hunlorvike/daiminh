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
public class ContactController(
    IContactService contactService,
    IServiceProvider serviceProvider,
    IConfiguration configuration) : DaiminhController(serviceProvider, configuration)
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContactCreateRequest request)
    {
        var validator = GetValidator<ContactCreateRequest>();

        if (await this.ValidateAndReturnView(validator, request)) return View("Index", request);

        try
        {
            Contact model = new()
            {
                Name = request.Name ?? string.Empty,
                Email = request.Email ?? string.Empty,
                Phone = request.Phone ?? string.Empty,
                Message = request.Message ?? string.Empty
            };

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