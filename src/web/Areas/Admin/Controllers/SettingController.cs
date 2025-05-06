using System.Text.Json;
using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Models;
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
        // Validate từng setting
        bool hasError = false;
        foreach (var group in viewModel.SettingGroups)
        {
            for (int i = 0; i < group.Value.Count; i++)
            {
                var settingVM = group.Value[i];
                var result = await _settingValidator.ValidateAsync(settingVM);
                if (!result.IsValid)
                {
                    result.AddToModelState(ModelState, $"SettingGroups[{group.Key}][{i}]");
                    hasError = true;
                }
            }
        }

        if (hasError || !ModelState.IsValid)
        {
            var freshModel = await BuildSettingsIndexViewModelAsync(viewModel.SearchTerm);
            MergeInputWithFreshData(viewModel, freshModel);
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Cập nhật thất bại. Vui lòng kiểm tra lại.", ToastType.Error)
            );
            return View("Index", freshModel);
        }

        try
        {
            var settingIds = viewModel.SettingGroups.SelectMany(g => g.Value.Select(s => s.Id)).ToList();
            var settingsInDb = await _context.Set<Setting>().Where(s => settingIds.Contains(s.Id)).ToListAsync();
            var dict = settingsInDb.ToDictionary(s => s.Id);

            bool changed = ApplySettingChanges(viewModel, dict);

            if (changed)
            {
                await _context.SaveChangesAsync();
                TempData["ToastMessage"] = JsonSerializer.Serialize(
                    new ToastData("Thành công", "Cập nhật cài đặt thành công.", ToastType.Success)
                );
            }
            else
            {
                TempData["ToastMessage"] = JsonSerializer.Serialize(
                    new ToastData("Thông báo", "Không có thay đổi nào cần lưu.", ToastType.Info)
                );
                _logger.LogInformation("Không có thay đổi nào trong cập nhật cài đặt.");
            }

            return RedirectToAction(nameof(Index), new { viewModel.SearchTerm });
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi cập nhật cài đặt.");
            ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu cài đặt. Vui lòng thử lại.");

            var freshModel = await BuildSettingsIndexViewModelAsync(viewModel.SearchTerm);
            MergeInputWithFreshData(viewModel, freshModel);
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Lỗi hệ thống khi cập nhật.", ToastType.Error)
            );
            return View("Index", freshModel);
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

    private bool ApplySettingChanges(SettingsIndexViewModel viewModel, Dictionary<int, Setting> settingsDict)
    {
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
                        _logger.LogInformation("Cập nhật Setting: {Key} => {Value}", settingEntity.Key, settingEntity.Value);
                        changed = true;
                    }
                }
                else
                {
                    _logger.LogWarning("Không tìm thấy setting ID {Id} trong DB khi cập nhật.", settingVM.Id);
                }
            }
        }

        return changed;
    }

    private void MergeInputWithFreshData(SettingsIndexViewModel source, SettingsIndexViewModel destination)
    {
        foreach (var inputGroup in source.SettingGroups)
        {
            if (destination.SettingGroups.TryGetValue(inputGroup.Key, out var freshGroup))
            {
                foreach (var input in inputGroup.Value)
                {
                    var match = freshGroup.FirstOrDefault(s => s.Id == input.Id);
                    if (match != null)
                    {
                        match.Value = input.Value;
                    }
                }
            }
        }
    }

}