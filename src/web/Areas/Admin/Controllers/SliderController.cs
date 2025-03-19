using AutoMapper;

using domain.Entities;

using infrastructure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using shared.Attributes;
using shared.Constants;
using shared.Enums;
using shared.Extensions;
using shared.Models;

using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Models.Slider;
using web.Areas.Admin.Requests.Slider;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public class SliderController(
    ApplicationDbContext dbContext,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration) : DaiminhController(mapper, serviceProvider, configuration)
{
    public async Task<IActionResult> Index()
    {
        var sliders = await dbContext.Sliders
            .AsNoTracking()
            .Where(x => x.DeletedAt == null)
            .ToListAsync();

        List<SliderViewModel> models = _mapper.Map<List<SliderViewModel>>(sliders);
        return View(models);
    }

    [AjaxOnly]
    public IActionResult Create()
    {
        ViewBag.OverlayPositionList = Enum.GetValues(typeof(OverlayPosition))
            .Cast<OverlayPosition>()
            .Select(op => new SelectListItem
            {
                Value = ((int)op).ToString(),
                Text = op.ToString()
            })
            .ToList();
        return PartialView("_Create.Modal", new SliderCreateRequest());
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var slider = await dbContext.Sliders
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null);

        if (slider == null) return NotFound();
        var request = _mapper.Map<SliderUpdateRequest>(slider);

        ViewBag.OverlayPositionList = Enum.GetValues(typeof(OverlayPosition))
            .Cast<OverlayPosition>()
            .Select(op => new SelectListItem
            {
                Value = ((int)op).ToString(),
                Text = op.ToString(),
                Selected = request.OverlayPosition == op
            })
            .ToList();

        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var slider = await dbContext.Sliders
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null);

        if (slider == null) return NotFound();
        var request = _mapper.Map<SliderDeleteRequest>(slider);
        return PartialView("_Delete.Modal", request);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SliderCreateRequest model)
    {
        var validator = GetValidator<SliderCreateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var newSlider = _mapper.Map<Slider>(model);
            await dbContext.Sliders.AddAsync(newSlider);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<Slider>(newSlider, "Thêm slider mới thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "Slider", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "Slider", new { area = "Admin" });
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SliderUpdateRequest model)
    {
        var validator = GetValidator<SliderUpdateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var slider = await dbContext.Sliders
                .FirstOrDefaultAsync(s => s.Id == model.Id && s.DeletedAt == null);

            if (slider == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "General", ["Slider không tồn tại hoặc đã bị xóa."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

            _mapper.Map(model, slider);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<Slider>(slider, "Cập nhật slider thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "Slider", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "Slider", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            if (Request.IsAjaxRequest())
                return Json(new
                {
                    success = false,
                    error = ex.Message
                });

            ModelState.AddModelError("", ex.Message);
            return PartialView("_Edit.Modal", model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(SliderDeleteRequest model)
    {
        try
        {
            var slider = await dbContext.Sliders
                .FirstOrDefaultAsync(s => s.Id == model.Id && s.DeletedAt == null);

            if (slider == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "General", ["Slider không tồn tại hoặc đã bị xóa."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

            dbContext.Sliders.Remove(slider);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<Slider>(slider, "Xóa slider thành công (đã ẩn).");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "Slider", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "Slider", new { area = "Admin" });
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