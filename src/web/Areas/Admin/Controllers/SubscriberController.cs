using AutoMapper;

using domain.Entities;

using infrastructure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using shared.Attributes;
using shared.Constants;
using shared.Extensions;
using shared.Models;

using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Requests.Subscriber;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class SubscriberController(
    ApplicationDbContext dbContext,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache);

public partial class SubscriberController
{
    public async Task<IActionResult> Index()
    {
        var subscribers = await dbContext.Subscribers
            .AsNoTracking()
            .Where(x => x.DeletedAt == null)
            .ToListAsync();

        return View(subscribers);
    }

    [AjaxOnly]
    public async Task<IActionResult> Details(int id)
    {
        var subscriber = await dbContext.Subscribers
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null) ?? throw new NotFoundException("Subscriber not found.");
        return PartialView("_Detail.Modal", subscriber);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var subscriber = await dbContext.Subscribers
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null) ?? throw new NotFoundException("Subscriber not found.");
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

            dbContext.Subscribers.Remove(subscriber!);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<Subscriber>(subscriber!, "Xóa người đăng ký thành công (đã ẩn).");

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
            throw new SystemException2("Error deleting subscriber.", ex);
        }
    }
}