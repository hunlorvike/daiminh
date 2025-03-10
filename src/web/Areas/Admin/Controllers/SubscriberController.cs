using application.Interfaces;
using AutoMapper;
using domain.Constants;
using domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shared.Extensions;
using shared.Models;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Models.Subscriber;
using web.Areas.Admin.Requests.Subscriber;
using web.Attributes;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class SubscriberController(
    ISubscriberService subscriberService,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class SubscriberController
{
    public async Task<IActionResult> Index()
    {
        var subscriber = await subscriberService.GetAllAsync();
        List<SubscriberViewModel> models = _mapper.Map<List<SubscriberViewModel>>(subscriber);
        return View(models);
    }

    [AjaxOnly]
    public async Task<IActionResult> Details(int id)
    {
        var subscriber = await subscriberService.GetByIdAsync(id);
        if (subscriber == null) return NotFound();
        var subscriberDetail = _mapper.Map<SubscriberViewModel>(subscriber);
        return PartialView("_Detail.Modal", subscriberDetail);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await subscriberService.GetByIdAsync(id);
        if (response == null) return NotFound();
        var request = _mapper.Map<SubscriberDeleteRequest>(response);
        return PartialView("_Delete.Modal", request);
    }
}

public partial class SubscriberController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(SubscriberDeleteRequest model)
    {
        try
        {
            var response = await subscriberService.DeleteAsync(model.Id);

            switch (response)
            {
                case SuccessResponse<Subscriber> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "Subscriber", new { area = "Admin" })
                    });
                case SuccessResponse<Subscriber> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "Subscriber", new { area = "Admin" });
                case ErrorResponse errorResponse when Request.IsAjaxRequest():
                    return BadRequest(errorResponse);
                case ErrorResponse errorResponse:
                    {
                        return BadRequest(errorResponse);
                    }
            }

            return PartialView("_Delete.Modal", model);
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