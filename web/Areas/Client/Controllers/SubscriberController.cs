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
        var validationResult = await validator.ValidateAsync(model);
        if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return Json(new
                    {
                        success = false,
                        message = "Dữ liệu rất là không hợp lệ!",
                        errors = ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault())
                    });
            }
        try
            {
            var newSubscriber = _mapper.Map<Subscriber>(model);
            var response = await subscriberService.AddAsync(newSubscriber);
                switch (response)
                {
                    case SuccessResponse<Subscriber> successResponse:
                   
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "Home", new { area = "Client" })
                    });

                case ErrorResponse errorResponse:
                   
                    return Json(new
                    {
                        success = false,
                        message = "Đăng ký không thành công!",
                        errors = errorResponse.Errors
                    });
                default:
                    return Json(new { success = false, message = "Có lỗi xảy ra, vui lòng thử lại!" });
            }
            }
            catch (Exception ex)
            {
          
                return Json(new { success = false, message = ex.Message });
        }
        }
    }

