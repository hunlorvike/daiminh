using AutoMapper;

using domain.Entities;

using infrastructure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using shared.Attributes;
using shared.Constants;
using shared.Extensions;
using shared.Models;

using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Models.Subscriber;
using web.Areas.Admin.Requests.Subscriber;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class SubscriberController(
    ApplicationDbContext dbContext,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class SubscriberController
{
    public async Task<IActionResult> Index()
    {
        var subscribers = await dbContext.Subscribers
            .AsNoTracking()
            .Where(x => x.DeletedAt == null)
            .ToListAsync();

        List<SubscriberViewModel> models = _mapper.Map<List<SubscriberViewModel>>(subscribers);
        return View(models);
    }

    [AjaxOnly]
    public async Task<IActionResult> Details(int id)
    {
        var subscriber = await dbContext.Subscribers
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null);

        if (subscriber == null) return NotFound();
        var subscriberDetail = _mapper.Map<SubscriberViewModel>(subscriber);
        return PartialView("_Detail.Modal", subscriberDetail);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var subscriber = await dbContext.Subscribers
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null);

        if (subscriber == null) return NotFound();
        var request = _mapper.Map<SubscriberDeleteRequest>(subscriber);
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
            var subscriber = await dbContext.Subscribers
                .FirstOrDefaultAsync(s => s.Id == model.Id && s.DeletedAt == null);

            if (subscriber == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "General", ["Người đăng ký không tồn tại hoặc đã bị xóa."] }
                };

                if (Request.IsAjaxRequest())
                {
                    return BadRequest(new ErrorResponse(errors));
                }

                return BadRequest(new ErrorResponse(errors));
            }

            dbContext.Subscribers.Remove(subscriber);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<Subscriber>(subscriber, "Xóa người đăng ký thành công (đã ẩn).");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "Subscriber", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "Subscriber", new { area = "Admin" });
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