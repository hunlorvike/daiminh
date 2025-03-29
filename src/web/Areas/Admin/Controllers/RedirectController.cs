using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Admin.ViewModels.Redirect;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class RedirectController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<RedirectViewModel> _validator;

    public RedirectController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<RedirectViewModel> validator)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
    }

    // GET: Admin/Redirect
    public async Task<IActionResult> Index(string? searchTerm = null, RedirectType? type = null, bool? isActive = null)
    {
        ViewData["PageTitle"] = "Quản lý chuyển hướng URL";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Chuyển hướng URL", "")
        };

        var query = _context.Set<Redirect>().AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(r =>
                r.SourceUrl.Contains(searchTerm) ||
                r.TargetUrl.Contains(searchTerm) ||
                r.Notes != null && r.Notes.Contains(searchTerm));
        }

        if (type.HasValue)
        {
            query = query.Where(r => r.Type == type.Value);
        }

        if (isActive.HasValue)
        {
            query = query.Where(r => r.IsActive == isActive.Value);
        }

        var redirects = await query
            .OrderByDescending(r => r.UpdatedAt)
            .ToListAsync();

        var viewModels = _mapper.Map<List<RedirectListItemViewModel>>(redirects);

        ViewBag.SearchTerm = searchTerm;
        ViewBag.SelectedType = type;
        ViewBag.SelectedIsActive = isActive;

        return View(viewModels);
    }

    // GET: Admin/Redirect/Create
    public IActionResult Create()
    {
        ViewData["PageTitle"] = "Thêm chuyển hướng mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Chuyển hướng URL", "/Admin/Redirect"),
            ("Thêm mới", "")
        };

        var viewModel = new RedirectViewModel
        {
            IsActive = true,
            Type = RedirectType.Permanent
        };

        return View(viewModel);
    }

    // POST: Admin/Redirect/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RedirectViewModel viewModel)
    {
        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            return View(viewModel);
        }

        // Check if source URL already exists
        if (await _context.Set<Redirect>().AnyAsync(r => r.SourceUrl == viewModel.SourceUrl && r.IsRegex == viewModel.IsRegex))
        {
            ModelState.AddModelError("SourceUrl", "URL nguồn này đã tồn tại trong hệ thống");
            return View(viewModel);
        }

        var redirect = _mapper.Map<Redirect>(viewModel);

        _context.Add(redirect);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Thêm chuyển hướng thành công";
        return RedirectToAction(nameof(Index));
    }

    // GET: Admin/Redirect/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var redirect = await _context.Set<Redirect>().FindAsync(id);

        if (redirect == null)
        {
            return NotFound();
        }

        ViewData["PageTitle"] = "Chỉnh sửa chuyển hướng";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Chuyển hướng URL", "/Admin/Redirect"),
            ("Chỉnh sửa", "")
        };

        var viewModel = _mapper.Map<RedirectViewModel>(redirect);

        return View(viewModel);
    }

    // POST: Admin/Redirect/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, RedirectViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            return View(viewModel);
        }

        // Check if source URL already exists (excluding current redirect)
        if (await _context.Set<Redirect>().AnyAsync(r => r.SourceUrl == viewModel.SourceUrl && r.IsRegex == viewModel.IsRegex && r.Id != id))
        {
            ModelState.AddModelError("SourceUrl", "URL nguồn này đã tồn tại trong hệ thống");
            return View(viewModel);
        }

        try
        {
            var redirect = await _context.Set<Redirect>().FindAsync(id);

            if (redirect == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, redirect);

            _context.Update(redirect);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật chuyển hướng thành công";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await RedirectExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: Admin/Redirect/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var redirect = await _context.Set<Redirect>().FindAsync(id);

        if (redirect == null)
        {
            return Json(new { success = false, message = "Không tìm thấy chuyển hướng" });
        }

        _context.Set<Redirect>().Remove(redirect);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Xóa chuyển hướng thành công" });
    }

    // POST: Admin/Redirect/ToggleActive/5
    [HttpPost]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var redirect = await _context.Set<Redirect>().FindAsync(id);

        if (redirect == null)
        {
            return Json(new { success = false, message = "Không tìm thấy chuyển hướng" });
        }

        redirect.IsActive = !redirect.IsActive;
        await _context.SaveChangesAsync();

        return Json(new { success = true, active = redirect.IsActive });
    }

    // POST: Admin/Redirect/ResetHitCount/5
    [HttpPost]
    public async Task<IActionResult> ResetHitCount(int id)
    {
        var redirect = await _context.Set<Redirect>().FindAsync(id);

        if (redirect == null)
        {
            return Json(new { success = false, message = "Không tìm thấy chuyển hướng" });
        }

        redirect.HitCount = 0;
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Đã đặt lại số lần truy cập" });
    }

    // POST: Admin/Redirect/ImportFromCsv
    [HttpPost]
    public async Task<IActionResult> ImportFromCsv(IFormFile csvFile)
    {
        if (csvFile == null || csvFile.Length == 0)
        {
            TempData["ErrorMessage"] = "Vui lòng chọn file CSV để nhập";
            return RedirectToAction(nameof(Index));
        }

        if (!csvFile.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
        {
            TempData["ErrorMessage"] = "File phải có định dạng CSV";
            return RedirectToAction(nameof(Index));
        }

        var redirects = new List<Redirect>();
        var errors = new List<string>();
        int lineNumber = 0;
        int successCount = 0;

        using (var reader = new StreamReader(csvFile.OpenReadStream()))
        {
            // Skip header line
            await reader.ReadLineAsync();
            lineNumber++;

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                lineNumber++;

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var values = line.Split(',');
                if (values.Length < 2)
                {
                    errors.Add($"Dòng {lineNumber}: Không đủ cột dữ liệu");
                    continue;
                }

                var sourceUrl = values[0].Trim();
                var targetUrl = values[1].Trim();

                if (string.IsNullOrWhiteSpace(sourceUrl) || string.IsNullOrWhiteSpace(targetUrl))
                {
                    errors.Add($"Dòng {lineNumber}: URL nguồn hoặc URL đích không được để trống");
                    continue;
                }

                // Check if source URL already exists
                if (await _context.Set<Redirect>().AnyAsync(r => r.SourceUrl == sourceUrl) ||
                    redirects.Any(r => r.SourceUrl == sourceUrl))
                {
                    errors.Add($"Dòng {lineNumber}: URL nguồn '{sourceUrl}' đã tồn tại");
                    continue;
                }

                var redirect = new Redirect
                {
                    SourceUrl = sourceUrl,
                    TargetUrl = targetUrl,
                    Type = RedirectType.Permanent,
                    IsRegex = false,
                    IsActive = true,
                    Notes = values.Length > 2 ? values[2].Trim() : null
                };

                redirects.Add(redirect);
                successCount++;
            }
        }

        if (redirects.Any())
        {
            await _context.AddRangeAsync(redirects);
            await _context.SaveChangesAsync();
        }

        if (errors.Any())
        {
            TempData["ErrorMessage"] = $"Nhập {successCount} chuyển hướng thành công. Có {errors.Count} lỗi: {string.Join("; ", errors)}";
        }
        else
        {
            TempData["SuccessMessage"] = $"Nhập {successCount} chuyển hướng thành công";
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Admin/Redirect/ExportToCsv
    public async Task<IActionResult> ExportToCsv()
    {
        var redirects = await _context.Set<Redirect>()
            .OrderBy(r => r.SourceUrl)
            .ToListAsync();

        var csv = new System.Text.StringBuilder();
        csv.AppendLine("SourceUrl,TargetUrl,Type,IsRegex,IsActive,HitCount,Notes");

        foreach (var redirect in redirects)
        {
            var notes = string.IsNullOrEmpty(redirect.Notes) ? "" : redirect.Notes.Replace(",", "\\,");
            csv.AppendLine($"{redirect.SourceUrl},{redirect.TargetUrl},{redirect.Type},{redirect.IsRegex},{redirect.IsActive},{redirect.HitCount},{notes}");
        }

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
        return File(bytes, "text/csv", $"redirects-{DateTime.Now:yyyyMMdd}.csv");
    }

    private async Task<bool> RedirectExists(int id)
    {
        return await _context.Set<Redirect>().AnyAsync(e => e.Id == id);
    }
}

