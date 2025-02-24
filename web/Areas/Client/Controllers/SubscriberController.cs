using AutoMapper;
using core.Common.Extensions;
using core.Entities;
using core.Interfaces.Service;
using Core.Common.Models;
using Microsoft.AspNetCore.Mvc;
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
            if (await this.ValidateAndReturnView(validator, model))
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ!";
                return RedirectToAction("Index", "Home", new { area = "Client" });
            }
        try
            {
            var newSubscriber = _mapper.Map<Subscriber>(model);
            var response = await subscriberService.AddAsync(newSubscriber);
                switch (response)
                {
                    case SuccessResponse<Subscriber> successResponse:
                        TempData["SuccessMessage"] = successResponse.Message;

                        if (Request.IsAjaxRequest())
                            return Json(new { redirectUrl = Url.Action("Index", "Home", new { area = "Client" }) });

                        return RedirectToAction("Index", "Home", new { area = "Client" });
                    case ErrorResponse errorResponse:
                        TempData["ErrorMessage"] = "Đăng ký không thành công!";
                    {
                            foreach (var error in errorResponse.Errors) ModelState.AddModelError(error.Key, error.Value);
                            return RedirectToAction("Index", "Home", new { area = "Client" });
                    }
                    default:
                    TempData["ErrorMessage"] = "Có lỗi xảy ra, vui lòng thử lại!";
                        return RedirectToAction("Index", "Home", new { area = "Client" });
            }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Index", "Home", new { area = "Client" });
        }
        }
    }

