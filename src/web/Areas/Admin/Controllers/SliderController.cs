using application.Interfaces;
using application.Services;
using AutoMapper;
using domain.Constants;
using domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;
using shared.Extensions;
using shared.Models;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Models.Slider;
using web.Areas.Admin.Requests.Slider;
using web.Attributes;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class SliderController(
    ISliderService sliderService,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class SliderController
{
    public async Task<IActionResult> Index()
    {
        var sliders = await sliderService.GetAllAsync();
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
        var response = await sliderService.GetByIdAsync(id);
        if (response == null) return NotFound();
        var request = _mapper.Map<SliderUpdateRequest>(response);

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
        var response = await sliderService.GetByIdAsync(id);
        if (response == null) return NotFound();
        var request = _mapper.Map<SliderDeleteRequest>(response);
        return PartialView("_Delete.Modal", request);
    }
}

public partial class SliderController
{
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
            var response = await sliderService.AddAsync(newSlider);

            switch (response)
            {
                case SuccessResponse<Slider> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "Slider", new { area = "Admin" })
                    });
                case SuccessResponse<Slider> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "Slider", new { area = "Admin" });
                case ErrorResponse errorResponse when Request.IsAjaxRequest():
                    return BadRequest(errorResponse);
                case ErrorResponse errorResponse:
                    {
                        return BadRequest(errorResponse);
                    }
            }

            return PartialView("_Create.Modal", model);
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
            var slider = await sliderService.GetByIdAsync(model.Id);
            if (slider == null) return NotFound();

            _mapper.Map(model, slider);

            var response = await sliderService.UpdateAsync(model.Id, slider);

            switch (response)
            {
                case SuccessResponse<Slider> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "Slider", new { area = "Admin" })
                    });
                case SuccessResponse<Slider> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "Slider", new { area = "Admin" });
                case ErrorResponse errorResponse when Request.IsAjaxRequest():
                    return BadRequest(errorResponse);
                case ErrorResponse errorResponse:
                    {
                        return BadRequest(errorResponse);
                    }
            }

            return PartialView("_Edit.Modal", model);
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

    // to do hưng fix delete slider
    // lí do delete nhưng vẫn hiển thị trên list
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(SliderDeleteRequest model)
    {
        try
        {
            var response = await sliderService.DeleteAsync(model.Id);

            switch (response)
            {
                case SuccessResponse<Slider> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "Slider", new { area = "Admin" })
                    });
                case SuccessResponse<Slider> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "Slider", new { area = "Admin" });
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