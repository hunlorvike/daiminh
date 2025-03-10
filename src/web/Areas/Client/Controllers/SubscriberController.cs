using application.Interfaces;
using AutoMapper;
using domain.Entities;
using Microsoft.AspNetCore.Mvc;
using shared.Extensions;
using shared.Models;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Client.Requests.Subscriber;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("/Subscriber")]
public partial class SubscriberController(
    ISubscriberService subscriberService,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class SubscriberController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SubscriberCreateRequest model)
    {
        var validator = GetValidator<SubscriberCreateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var newSubscriber = _mapper.Map<Subscriber>(model);
            var response = await subscriberService.AddAsync(newSubscriber);
            switch (response)
            {
                case SuccessResponse<Subscriber> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "Home", new { area = "Client" })
                    });
                case SuccessResponse<Subscriber> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "Home", new { area = "Client" });
                case ErrorResponse errorResponse when Request.IsAjaxRequest():
                    return BadRequest(errorResponse);
                case ErrorResponse errorResponse:
                    {
                        return BadRequest(errorResponse);
                    }
            }

            return RedirectToAction("Index", "Home", new { area = "Client" });
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