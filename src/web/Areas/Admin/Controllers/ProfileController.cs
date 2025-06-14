using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shared.Constants;
using shared.Enums;
using shared.Models;
using System.Security.Claims;
using System.Text.Json;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme")]
public class ProfileController : Controller
{
    private readonly IProfileService _profileService;
    private readonly IValidator<ProfileViewModel> _profileValidator;
    private readonly IValidator<ChangePasswordViewModel> _passwordValidator;

    public ProfileController(IProfileService profileService, IValidator<ProfileViewModel> profileValidator, IValidator<ChangePasswordViewModel> passwordValidator)
    {
        _profileService = profileService;
        _profileValidator = profileValidator;
        _passwordValidator = passwordValidator;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Challenge(); // Not logged in
        }

        var viewModel = await _profileService.GetProfileAsync(userId);
        if (viewModel == null)
        {
            return NotFound("Không tìm thấy thông tin người dùng.");
        }

        ViewData["ChangePasswordViewModel"] = new ChangePasswordViewModel();
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile(ProfileViewModel viewModel)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Challenge();

        var validationResult = await _profileValidator.ValidateAsync(viewModel);
        if (!validationResult.IsValid)
        {
            validationResult.Errors.ForEach(e => ModelState.AddModelError("Profile." + e.PropertyName, e.ErrorMessage));
        }

        if (ModelState.IsValid)
        {
            var result = await _profileService.UpdateProfileAsync(userId, viewModel);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                result.Success
                    ? new ToastData("Thành công", result.Message ?? "Cập nhật hồ sơ thành công!", ToastType.Success)
                    : new ToastData("Lỗi", result.Message ?? "Cập nhật hồ sơ thất bại.", ToastType.Error)
            );
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Challenge();

        var validationResult = await _passwordValidator.ValidateAsync(viewModel);
        if (!validationResult.IsValid)
        {
            validationResult.Errors.ForEach(e => ModelState.AddModelError("Password." + e.PropertyName, e.ErrorMessage));
            TempData["ShowPasswordTab"] = true;
        }

        if (ModelState.IsValid)
        {
            var result = await _profileService.ChangePasswordAsync(userId, viewModel);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                result.Success
                    ? new ToastData("Thành công", result.Message ?? "Đổi mật khẩu thành công!", ToastType.Success)
                    : new ToastData("Lỗi", result.Message ?? "Đổi mật khẩu thất bại.", ToastType.Error)
            );

            if (!result.Success)
            {
                TempData["ShowPasswordTab"] = true;
            }
        }
        
        return RedirectToAction(nameof(Index));
    }
}