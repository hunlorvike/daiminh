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
public class SettingController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<SettingUpdateViewModel> _validator;


    public SettingController(ApplicationDbContext context, IMapper mapper, IValidator<SettingUpdateViewModel> validator)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
    }

    // GET: Admin/Setting (Displays settings grouped by category for editing)
    public async Task<IActionResult> Index()
    {
        ViewData["PageTitle"] = "Cấu hình hệ thống";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Cấu hình", "")
        };

        var settings = await _context.Set<Setting>()
                                     .OrderBy(s => s.Category)
                                     .ThenBy(s => s.Key)
                                     .ToListAsync();

        var settingViewModels = _mapper.Map<List<SettingViewModel>>(settings);

        var viewModel = new SettingUpdateViewModel
        {
            Settings = settingViewModels
        };

        return View(viewModel);
    }

    // POST: Admin/Setting/Update (Handles the form submission)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(SettingUpdateViewModel viewModel)
    {
        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);

            ViewData["PageTitle"] = "Cấu hình hệ thống";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Cấu hình", "") };

            var originalSettings = await _context.Set<Setting>()
                                                 .OrderBy(s => s.Category)
                                                 .ThenBy(s => s.Key)
                                                 .ToListAsync();
            var originalSettingViewModels = _mapper.Map<List<SettingViewModel>>(originalSettings);

            var submittedValues = viewModel.Settings.ToDictionary(s => s.Id);

            foreach (var originalVm in originalSettingViewModels)
            {
                if (submittedValues.TryGetValue(originalVm.Id, out var submittedVm))
                {
                    originalVm.Value = submittedVm.Value;
                    originalVm.IsActive = submittedVm.IsActive;
                }
            }

            var modelToReturn = new SettingUpdateViewModel { Settings = originalSettingViewModels };
            TempData["ErrorMessage"] = "Dữ liệu không hợp lệ, vui lòng kiểm tra lại.";
            return View("Index", modelToReturn);
        }


        var settingIds = viewModel.Settings.Select(s => s.Id).ToList();

        var dbSettings = await _context.Set<Setting>()
                                       .Where(s => settingIds.Contains(s.Id))
                                       .ToDictionaryAsync(s => s.Id);

        bool changesMade = false;
        foreach (var settingViewModel in viewModel.Settings)
        {
            if (dbSettings.TryGetValue(settingViewModel.Id, out var dbSetting))
            {
                string? currentDbValue = dbSetting.Value;
                bool currentDbActive = dbSetting.IsActive;

                string? submittedValue = settingViewModel.Value;
                bool submittedActive = settingViewModel.IsActive;

                if (currentDbValue != submittedValue || currentDbActive != submittedActive)
                {
                    _mapper.Map(settingViewModel, dbSetting);
                    _context.Entry(dbSetting).State = EntityState.Modified;
                    changesMade = true;
                }
            }
            else
            {
                ModelState.AddModelError("", $"Setting with ID {settingViewModel.Id} not found.");
                return View("Index", viewModel);
            }
        }
        if (changesMade)
        {
            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật cấu hình thành công!";
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["ErrorMessage"] = "Lỗi: Có xung đột xảy ra khi cập nhật. Vui lòng thử lại.";
                return View("Index", viewModel);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi không mong muốn khi lưu cấu hình.";
                return View("Index", viewModel);
            }
        }
        else
        {
            TempData["InfoMessage"] = "Không có thay đổi nào được thực hiện.";
        }

        return RedirectToAction(nameof(Index));
    }
}
