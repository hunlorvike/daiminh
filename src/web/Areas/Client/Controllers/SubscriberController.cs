using AutoMapper;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using shared.Extensions;
using shared.Models;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Client.Models.Home;
using web.Areas.Client.Requests.Subscriber;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("/Subscriber")]
public partial class SubscriberController(
    ApplicationDbContext context,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache);

public partial class SubscriberController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(HomeViewModel model)
    {
        var validator = GetValidator<SubscriberCreateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model.Subscriber);
        if (result != null) return result;

        try
        {
            var newSubscriber = _mapper.Map<Subscriber>(model.Subscriber);
            await context.Subscribers.AddAsync(newSubscriber);
            await context.SaveChangesAsync();

            var successResponse = new SuccessResponse<Subscriber>(newSubscriber, "Đăng ký nhận tin thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "Home", new { area = "Client" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "Home", new { area = "Client" });
        }
        catch (Exception ex)
        {
            throw new SystemException2("Error creating subscriber.", ex);

        }
    }
}