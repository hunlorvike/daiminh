using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.Services;
using web.Areas.Admin.ViewModels.Seo;


namespace web.Areas.Admin.Controllers;

[Area("Admin")]
public class SeoController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ISeoService _seoService;
    private readonly IValidator<SeoSettingsViewModel> _settingsValidator;
    private readonly IValidator<SeoGeneralSettingsViewModel> _generalSettingsValidator;
    private readonly IValidator<GoogleSearchConsoleImportViewModel> _importValidator;

    public SeoController(
        ApplicationDbContext context,
        IMapper mapper,
        ISeoService seoService,
        IValidator<SeoSettingsViewModel> settingsValidator,
        IValidator<SeoGeneralSettingsViewModel> generalSettingsValidator,
        IValidator<GoogleSearchConsoleImportViewModel> importValidator)
    {
        _context = context;
        _mapper = mapper;
        _seoService = seoService;
        _settingsValidator = settingsValidator;
        _generalSettingsValidator = generalSettingsValidator;
        _importValidator = importValidator;
    }

    #region Settings

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var settings = await _seoService.GetAllSettingsAsync();
        return View(settings);
    }

    [HttpGet("settings")]
    public async Task<IActionResult> Settings()
    {
        var model = await _seoService.GetGeneralSettingsAsync();
        return View(model);
    }

    [HttpPost("settings")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Settings(SeoGeneralSettingsViewModel model)
    {
        var validationResult = await _generalSettingsValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return View(model);
        }

        await _seoService.UpdateGeneralSettingsAsync(model);
        TempData["SuccessMessage"] = "Cài đặt SEO đã được cập nhật thành công.";
        return RedirectToAction(nameof(Settings));
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View(new SeoSettingsViewModel { IsActive = true });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SeoSettingsViewModel model)
    {
        var validationResult = await _settingsValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return View(model);
        }

        // Kiểm tra xem khóa đã tồn tại chưa
        var existingSetting = await _context.Set<SeoSettings>()
            .FirstOrDefaultAsync(s => s.Key == model.Key);

        if (existingSetting != null)
        {
            ModelState.AddModelError("Key", "Khóa cài đặt này đã tồn tại.");
            return View(model);
        }

        await _seoService.CreateSettingAsync(model);
        TempData["SuccessMessage"] = "Cài đặt SEO đã được tạo thành công.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _seoService.GetSettingByIdAsync(id);
        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SeoSettingsViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        var validationResult = await _settingsValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return View(model);
        }

        // Kiểm tra xem khóa đã tồn tại chưa (nếu đã thay đổi)
        var existingSetting = await _context.Set<SeoSettings>()
            .FirstOrDefaultAsync(s => s.Key == model.Key && s.Id != id);

        if (existingSetting != null)
        {
            ModelState.AddModelError("Key", "Khóa cài đặt này đã tồn tại.");
            return View(model);
        }

        await _seoService.UpdateSettingAsync(model);
        TempData["SuccessMessage"] = "Cài đặt SEO đã được cập nhật thành công.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _seoService.DeleteSettingAsync(id);
        TempData["SuccessMessage"] = "Cài đặt SEO đã được xóa thành công.";
        return RedirectToAction(nameof(Index));
    }

    #endregion

    #region Analytics

    [HttpGet("analytics")]
    public async Task<IActionResult> Analytics(
        string? entityType = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        if (!startDate.HasValue)
        {
            startDate = DateTime.Now.AddDays(-30);
        }

        if (!endDate.HasValue)
        {
            endDate = DateTime.Now;
        }

        ViewBag.EntityTypes = await _context.Set<SeoAnalytics>()
            .Select(a => a.EntityType)
            .Distinct()
            .ToListAsync();

        ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
        ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");
        ViewBag.SelectedEntityType = entityType;

        var summary = await _seoService.GetAnalyticsSummaryAsync(entityType, startDate, endDate);
        return View(summary);
    }

    [HttpGet("analytics/details")]
    public async Task<IActionResult> AnalyticsDetails(
        string? entityType = null,
        int? entityId = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        if (!startDate.HasValue)
        {
            startDate = DateTime.Now.AddDays(-30);
        }

        if (!endDate.HasValue)
        {
            endDate = DateTime.Now;
        }

        ViewBag.EntityTypes = await _context.Set<SeoAnalytics>()
            .Select(a => a.EntityType)
            .Distinct()
            .ToListAsync();

        ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
        ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");
        ViewBag.SelectedEntityType = entityType;
        ViewBag.SelectedEntityId = entityId;

        var analytics = await _seoService.GetAnalyticsAsync(entityType, entityId, startDate, endDate);
        return View(analytics);
    }

    [HttpGet("analytics/import")]
    public IActionResult ImportAnalytics()
    {
        var model = new GoogleSearchConsoleImportViewModel
        {
            StartDate = DateTime.Now.AddDays(-30),
            EndDate = DateTime.Now
        };
        return View(model);
    }

    [HttpPost("analytics/import")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ImportAnalytics(GoogleSearchConsoleImportViewModel model)
    {
        var validationResult = await _importValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return View(model);
        }

        try
        {
            await _seoService.ImportFromGoogleSearchConsoleAsync(
                model.JsonFile,
                model.StartDate,
                model.EndDate,
                model.OverwriteExistingData);

            TempData["SuccessMessage"] = "Dữ liệu từ Google Search Console đã được nhập thành công.";
            return RedirectToAction(nameof(Analytics));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Lỗi khi nhập dữ liệu: {ex.Message}");
            return View(model);
        }
    }

    [HttpGet("analytics/export")]
    public async Task<IActionResult> ExportAnalytics(
        string? entityType = null,
        int? entityId = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        if (!startDate.HasValue)
        {
            startDate = DateTime.Now.AddDays(-30);
        }

        if (!endDate.HasValue)
        {
            endDate = DateTime.Now;
        }

        var fileData = await _seoService.ExportAnalyticsToExcelAsync(entityType, entityId, startDate, endDate);
        var fileName = $"seo-analytics-{DateTime.Now:yyyyMMdd}.xlsx";
        return File(fileData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }

    #endregion
}
