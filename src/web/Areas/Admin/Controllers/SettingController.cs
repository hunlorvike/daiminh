using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shared.Constants;
using shared.Enums;
using shared.Models;
using System.Text.Json;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme", Policy = "AdminAccess")]
public partial class SettingController : Controller
{
    private readonly ISettingService _settingService;
    private readonly IMapper _mapper;
    private readonly ILogger<SettingController> _logger;
    private readonly IValidator<SettingViewModel> _settingValidator;

    public SettingController(
        ISettingService settingService,
        IMapper mapper,
        ILogger<SettingController> logger,
        IValidator<SettingViewModel> settingValidator)
    {
        _settingService = settingService ?? throw new ArgumentNullException(nameof(settingService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settingValidator = settingValidator ?? throw new ArgumentNullException(nameof(settingValidator));
    }

    // GET: Admin/Setting
    public async Task<IActionResult> Index(string? searchTerm = null)
    {
        try
        {
            var viewModel = await _settingService.GetSettingsIndexViewModelAsync(searchTerm);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading settings index.");
            var emptyViewModel = new SettingsIndexViewModel
            {
                SettingGroups = new Dictionary<string, List<SettingViewModel>>(),
                SearchTerm = searchTerm
            };
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                 new ToastData("Lỗi", "Không thể tải cài đặt hệ thống.", ToastType.Error)
             );
            return View(emptyViewModel);
        }
    }

    // POST: Admin/Setting/Update
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(SettingsIndexViewModel viewModel)
    {
        var allSettingsFromForm = viewModel.SettingGroups.SelectMany(g => g.Value).ToList();

        bool hasValidationError = false;
        foreach (var group in viewModel.SettingGroups)
        {
            for (int i = 0; i < group.Value.Count; i++)
            {
                var settingVM = group.Value[i];
                var validationResult = await _settingValidator.ValidateAsync(settingVM);
                if (!validationResult.IsValid)
                {
                    validationResult.AddToModelState(ModelState, $"SettingGroups[{group.Key}][{i}]");
                    hasValidationError = true;
                }
            }
        }

        if (hasValidationError || !ModelState.IsValid)
        {
            _logger.LogWarning("Setting update failed due to ViewModel validation errors.");
            var freshModel = await _settingService.GetSettingsIndexViewModelAsync(viewModel.SearchTerm);
            MergeInputWithFreshData(viewModel, freshModel);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Cập nhật thất bại. Vui lòng kiểm tra lại.", ToastType.Error)
            );
            return View("Index", freshModel);
        }

        var updateResult = await _settingService.UpdateSettingsAsync(allSettingsFromForm);

        if (updateResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", updateResult.Message ?? "Cập nhật cài đặt thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index), new { viewModel.SearchTerm });
        }
        else
        {
            _logger.LogError("Setting update failed in service. Message: {Message}", updateResult.Message);
            foreach (var error in updateResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            if (!updateResult.Errors.Any() && !string.IsNullOrEmpty(updateResult.Message))
            {
                ModelState.AddModelError(string.Empty, updateResult.Message);
            }


            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", updateResult.Message ?? "Lỗi hệ thống khi cập nhật.", ToastType.Error)
            );

            var freshModel = await _settingService.GetSettingsIndexViewModelAsync(viewModel.SearchTerm);
            MergeInputWithFreshData(viewModel, freshModel);
            return View("Index", freshModel);
        }
    }
}

public partial class SettingController
{
    private void MergeInputWithFreshData(SettingsIndexViewModel source, SettingsIndexViewModel destination)
    {
        var sourceSettingsDict = source.SettingGroups.SelectMany(g => g.Value).ToDictionary(s => s.Id);

        foreach (var group in destination.SettingGroups)
        {
            foreach (var settingVM in group.Value)
            {
                if (sourceSettingsDict.TryGetValue(settingVM.Id, out var sourceSettingVM))
                {
                    settingVM.Value = sourceSettingVM.Value;
                }
            }
        }
    }

}
