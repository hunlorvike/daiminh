using AutoMapper;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Setting;
using System.Diagnostics;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class SettingController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<SettingController> _logger;

    public SettingController(ApplicationDbContext context, IMapper mapper, ILogger<SettingController> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: Admin/Setting
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Cấu hình hệ thống - Hệ thống quản trị";
        ViewData["PageTitle"] = "Cấu hình hệ thống";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Cấu hình", "")
        };

        var settings = await _context.Set<Setting>()
                                     .AsNoTracking()
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

    // POST: Admin/Setting/Update
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(SettingUpdateViewModel viewModel)
    {

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Settings update failed due to model validation errors.");
            TempData["error"] = "Dữ liệu không hợp lệ, vui lòng kiểm tra lại các trường báo lỗi.";
            ViewData["Title"] = "Cấu hình hệ thống - Hệ thống quản trị";
            ViewData["PageTitle"] = "Cấu hình hệ thống";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Cấu hình", "") };
            return View("Index", viewModel);
        }

        var stopwatch = Stopwatch.StartNew();
        int updatedCount = 0;
        bool errorOccurred = false;

        var settingIds = viewModel.Settings.Select(s => s.Id).ToList();
        var dbSettingsDict = await _context.Set<Setting>()
                                           .Where(s => settingIds.Contains(s.Id))
                                           .ToDictionaryAsync(s => s.Id);

        foreach (var settingViewModel in viewModel.Settings)
        {
            if (dbSettingsDict.TryGetValue(settingViewModel.Id, out var dbSetting))
            {
                if (dbSetting.Value != settingViewModel.Value || dbSetting.IsActive != settingViewModel.IsActive)
                {
                    _mapper.Map(settingViewModel, dbSetting);
                    updatedCount++;
                }
            }
            else
            {
                _logger.LogWarning("Setting update: Setting with ID {SettingId} submitted but not found in database.", settingViewModel.Id);
                ModelState.AddModelError("", $"Không tìm thấy cài đặt với ID {settingViewModel.Id}.");
                errorOccurred = true; 
            }
        }

        if (errorOccurred)
        {
            TempData["error"] = "Đã xảy ra lỗi, một số cài đặt không tìm thấy.";
            ViewData["Title"] = "Cấu hình hệ thống - Hệ thống quản trị";
            ViewData["PageTitle"] = "Cấu hình hệ thống";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Cấu hình", "") };
            return View("Index", viewModel);
        }


        if (updatedCount > 0)
        {
            try
            {
                var affectedRows = await _context.SaveChangesAsync();
                stopwatch.Stop();
                _logger.LogInformation("Updated {UpdateCount} settings successfully in {ElapsedMs}ms by {User}.", updatedCount, stopwatch.ElapsedMilliseconds, User.Identity?.Name ?? "Unknown");
                TempData["success"] = $"Cập nhật {updatedCount} cấu hình thành công!";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Concurrency error updating settings after {ElapsedMs}ms.", stopwatch.ElapsedMilliseconds);
                TempData["error"] = "Lỗi: Có xung đột xảy ra khi lưu. Vui lòng tải lại trang và thử lại.";
                ViewData["Title"] = "Cấu hình hệ thống - Hệ thống quản trị";
                ViewData["PageTitle"] = "Cấu hình hệ thống";
                ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Cấu hình", "") };
                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Error saving settings update after {ElapsedMs}ms.", stopwatch.ElapsedMilliseconds);
                TempData["error"] = "Đã xảy ra lỗi không mong muốn khi lưu cấu hình.";
                ViewData["Title"] = "Cấu hình hệ thống - Hệ thống quản trị";
                ViewData["PageTitle"] = "Cấu hình hệ thống";
                ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Cấu hình", "") };
                return View("Index", viewModel);
            }
        }
        else
        {
            TempData["info"] = "Không có thay đổi nào được thực hiện.";
        }

        return RedirectToAction(nameof(Index));
    }
}
