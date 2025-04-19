using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Setting;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public partial class SettingController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<SettingController> _logger;
    private readonly IValidator<SettingViewModel> _settingValidator;

    public SettingController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<SettingController> logger,
        IValidator<SettingViewModel> settingValidator)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settingValidator = settingValidator ?? throw new ArgumentNullException(nameof(settingValidator));
    }

    // GET: Admin/Setting
    public async Task<IActionResult> Index(string? searchTerm = null)
    {
        var viewModel = await BuildSettingsIndexViewModelAsync(searchTerm);
        return View(viewModel);
    }

    // POST: Admin/Setting/Update
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(SettingsIndexViewModel viewModel)
    {
        bool hasError = false;

        // --- Manual Validation ---
        foreach (var group in viewModel.SettingGroups)
        {
            for (int i = 0; i < group.Value.Count; i++)
            {
                var settingVM = group.Value[i];
                var validationResult = await _settingValidator.ValidateAsync(settingVM);
                if (!validationResult.IsValid)
                {
                    validationResult.AddToModelState(ModelState, $"SettingGroups[{group.Key}][{i}]");
                    hasError = true;
                }
            }
        }

        if (hasError || !ModelState.IsValid) 
        {
            // Cần load lại dữ liệu gốc cho các trường readonly nếu view bị trả về
            var freshViewModel = await BuildSettingsIndexViewModelAsync(viewModel.SearchTerm);
            // Cập nhật lại giá trị người dùng đã nhập vào freshViewModel để không bị mất dữ liệu form
            foreach (var updatedGroup in viewModel.SettingGroups)
            {
                if (freshViewModel.SettingGroups.TryGetValue(updatedGroup.Key, out var freshGroup))
                {
                    foreach (var updatedSetting in updatedGroup.Value)
                    {
                        var freshSetting = freshGroup.FirstOrDefault(s => s.Id == updatedSetting.Id);
                        if (freshSetting != null)
                        {
                            freshSetting.Value = updatedSetting.Value;
                        }
                    }
                }
            }
            TempData["ErrorMessage"] = "Cập nhật thất bại. Vui lòng kiểm tra lại các giá trị đã nhập.";
            return View("Index", freshViewModel);
        }

        try
        {
            var settingIdsToUpdate = viewModel.SettingGroups
                                            .SelectMany(g => g.Value.Select(s => s.Id))
                                            .ToList();

            var settingsInDb = await _context.Set<Setting>()
                                           .Where(s => settingIdsToUpdate.Contains(s.Id))
                                           .ToListAsync();

            var settingsDict = settingsInDb.ToDictionary(s => s.Id);
            bool changed = false;

            foreach (var group in viewModel.SettingGroups)
            {
                foreach (var settingVM in group.Value)
                {
                    if (settingsDict.TryGetValue(settingVM.Id, out var settingEntity))
                    {
                        if (settingEntity.Value != settingVM.Value)
                        {
                            settingEntity.Value = settingVM.Value;
                            _context.Entry(settingEntity).State = EntityState.Modified;
                            changed = true;
                            _logger.LogInformation("Updating Setting Key: {Key}, New Value: {Value}", settingEntity.Key, settingVM.Value);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Setting with ID {SettingId} not found in database during update.", settingVM.Id);
                    }
                }
            }

            if (changed)
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật cài đặt thành công!";
            }
            else
            {
                TempData["InfoMessage"] = "Không có thay đổi nào được lưu.";
            }

            return RedirectToAction(nameof(Index), new { searchTerm = viewModel.SearchTerm });
        }
        catch (DbUpdateException ex)
        {
            ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu cài đặt. Vui lòng thử lại.");
            var freshViewModel = await BuildSettingsIndexViewModelAsync(viewModel.SearchTerm);
            foreach (var updatedGroup in viewModel.SettingGroups)
            {
                if (freshViewModel.SettingGroups.TryGetValue(updatedGroup.Key, out var freshGroup))
                {
                    foreach (var updatedSetting in updatedGroup.Value)
                    {
                        var freshSetting = freshGroup.FirstOrDefault(s => s.Id == updatedSetting.Id);
                        if (freshSetting != null)
                        {
                            freshSetting.Value = updatedSetting.Value;
                        }
                    }
                }
            }
            TempData["ErrorMessage"] = "Đã xảy ra lỗi khi lưu cài đặt.";
            return View("Index", freshViewModel);
        }
    }
}

public partial class SettingController
{
    private async Task<SettingsIndexViewModel> BuildSettingsIndexViewModelAsync(string? searchTerm)
    {
        IQueryable<Setting> query = _context.Set<Setting>()
                                        .Where(s => s.IsActive)
                                        .OrderBy(s => s.Category)
                                        .ThenBy(s => s.Key)
                                        .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string lowerSearchTerm = searchTerm.Trim().ToLower();
            query = query.Where(s => s.Key.ToLower().Contains(lowerSearchTerm) ||
                                    (s.Description != null && s.Description.ToLower().Contains(lowerSearchTerm)));
        }

        var allSettings = await query.ToListAsync();

        var groupedSettings = allSettings
            .Select(s => _mapper.Map<SettingViewModel>(s))
            .GroupBy(svm => allSettings.First(s => s.Id == svm.Id).Category)
            .ToDictionary(g => g.Key, g => g.ToList());

        var viewModel = new SettingsIndexViewModel
        {
            SettingGroups = groupedSettings,
            SearchTerm = searchTerm
        };

        return viewModel;
    }
}