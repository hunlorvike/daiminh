using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Models;
using System.Text.Json;
using web.Areas.Admin.Validators.AttributeValue;
using web.Areas.Admin.ViewModels.AttributeValue;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public partial class AttributeValueController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<AttributeValueController> _logger;

    public AttributeValueController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<AttributeValueController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/AttributeValue
    public async Task<IActionResult> Index(AttributeValueFilterViewModel filter, int page = 1, int pageSize = 25)
    {
        filter ??= new AttributeValueFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 25;

        IQueryable<AttributeValue> query = _context.Set<AttributeValue>()
                                                .Include(av => av.Attribute)
                                                .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(av => av.Value.ToLower().Contains(lowerSearchTerm) ||
                                      (av.Slug != null && av.Slug.ToLower().Contains(lowerSearchTerm)));
        }

        if (filter.AttributeId.HasValue)
        {
            query = query.Where(av => av.AttributeId == filter.AttributeId.Value);
        }

        query = query.OrderBy(av => av.Attribute!.Name)
                     .ThenBy(av => av.Value);

        var valuesPaged = await query.ProjectTo<AttributeValueListItemViewModel>(_mapper.ConfigurationProvider)
                                     .ToPagedListAsync(pageNumber, currentPageSize);

        filter.AttributeOptions = await LoadAttributeSelectListAsync(filter.AttributeId);

        AttributeValueIndexViewModel viewModel = new()
        {
            AttributeValues = valuesPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/AttributeValue/Create
    public async Task<IActionResult> Create()
    {
        AttributeValueViewModel viewModel = new();
        await PopulateViewModelSelectListsAsync(viewModel);
        return View(viewModel);
    }

    // POST: Admin/AttributeValue/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AttributeValueViewModel viewModel)
    {
        var result = await new AttributeValueViewModelValidator(_context).ValidateAsync(viewModel);
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            await PopulateViewModelSelectListsAsync(viewModel);
            return View(viewModel);
        }

        var attributeValue = _mapper.Map<AttributeValue>(viewModel);
        _context.Add(attributeValue);

        try
        {
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Thêm giá trị '{attributeValue.Value}' thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index), new { AttributeId = attributeValue.AttributeId });
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo giá trị thuộc tính.");
            if (ex.InnerException?.Message.Contains("slug", StringComparison.OrdinalIgnoreCase) == true)
            {
                ModelState.AddModelError(nameof(viewModel.Slug), "Slug này đã tồn tại.");
            }
            else
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi cơ sở dữ liệu khi lưu.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi tạo giá trị thuộc tính.");
            ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi lưu.");
        }

        TempData["ToastMessage"] = JsonSerializer.Serialize(
            new ToastData("Lỗi", $"Không thể thêm giá trị '{viewModel.Value}'.", ToastType.Error)
        );
        await PopulateViewModelSelectListsAsync(viewModel);
        return View(viewModel);
    }

    // GET: Admin/AttributeValue/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        AttributeValue? attributeValue = await _context.Set<AttributeValue>()
                                                     .AsNoTracking()
                                                     .FirstOrDefaultAsync(av => av.Id == id);

        if (attributeValue == null)
        {
            return NotFound();
        }

        AttributeValueViewModel viewModel = _mapper.Map<AttributeValueViewModel>(attributeValue);
        await PopulateViewModelSelectListsAsync(viewModel);
        return View(viewModel);
    }

    // POST: Admin/AttributeValue/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AttributeValueViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var result = await new AttributeValueViewModelValidator(_context).ValidateAsync(viewModel);
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            await PopulateViewModelSelectListsAsync(viewModel);
            return View(viewModel);
        }

        var attributeValue = await _context.Set<AttributeValue>().FirstOrDefaultAsync(av => av.Id == id);
        if (attributeValue == null)
        {
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy giá trị thuộc tính để cập nhật.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        _mapper.Map(viewModel, attributeValue);

        try
        {
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Cập nhật giá trị '{attributeValue.Value}' thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index), new { AttributeId = attributeValue.AttributeId });
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật giá trị thuộc tính ID {Id}", id);
            if (ex.InnerException?.Message.Contains("slug", StringComparison.OrdinalIgnoreCase) == true)
            {
                ModelState.AddModelError(nameof(viewModel.Slug), "Slug đã tồn tại.");
            }
            else
            {
                ModelState.AddModelError("", "Lỗi cơ sở dữ liệu khi cập nhật.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi cập nhật giá trị thuộc tính ID {Id}", id);
            ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn.");
        }

        TempData["ToastMessage"] = JsonSerializer.Serialize(
            new ToastData("Lỗi", $"Không thể cập nhật giá trị '{viewModel.Value}'.", ToastType.Error)
        );
        await PopulateViewModelSelectListsAsync(viewModel);
        return View(viewModel);
    }

    // POST: Admin/AttributeValue/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var attributeValue = await _context.Set<AttributeValue>().FirstOrDefaultAsync(av => av.Id == id);

        if (attributeValue == null)
        {
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy giá trị thuộc tính.", ToastType.Error)
            );
            return Json(new { success = false, message = "Không tìm thấy giá trị thuộc tính." });
        }

        try
        {
            string value = attributeValue.Value;
            int parentId = attributeValue.AttributeId;

            _context.Remove(attributeValue);
            await _context.SaveChangesAsync();

            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Xóa giá trị '{value}' thành công.", ToastType.Success)
            );
            return Json(new
            {
                success = true,
                message = $"Xóa giá trị '{value}' thành công.",
                redirectUrl = Url.Action(nameof(Index), new { AttributeId = parentId })
            });
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi FK khi xóa giá trị thuộc tính ID {Id}", id);
            if (ex.InnerException?.Message.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) == true)
            {
                TempData["ToastMessage"] = JsonSerializer.Serialize(
                    new ToastData("Lỗi", "Không thể xóa giá trị này vì đang được sử dụng trong sản phẩm.", ToastType.Error)
                );
                return Json(new { success = false, message = "Không thể xóa giá trị này vì đang được sử dụng trong sản phẩm." });
            }
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Lỗi cơ sở dữ liệu khi xóa giá trị.", ToastType.Error)
            );
            return Json(new { success = false, message = "Lỗi cơ sở dữ liệu khi xóa giá trị." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi xóa giá trị thuộc tính ID {Id}", id);
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Đã xảy ra lỗi không mong muốn khi xóa.", ToastType.Error)
            );
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa." });
        }
    }
}

public partial class AttributeValueController
{
    private async Task PopulateViewModelSelectListsAsync(AttributeValueViewModel viewModel)
    {
        viewModel.AttributeOptions = await LoadAttributeSelectListAsync(viewModel.AttributeId);
    }

    private async Task<List<SelectListItem>> LoadAttributeSelectListAsync(int? selectedValue = null)
    {
        var attributes = await _context.Set<domain.Entities.Attribute>()
                         .OrderBy(a => a.Name)
                         .AsNoTracking()
                         .Select(a => new { a.Id, a.Name })
                         .ToListAsync();

        var items = new List<SelectListItem>
        {
             new SelectListItem { Value = "", Text = "-- Chọn thuộc tính cha --", Selected = !selectedValue.HasValue }
        };

        items.AddRange(attributes.Select(a => new SelectListItem
        {
            Value = a.Id.ToString(),
            Text = a.Name,
            Selected = selectedValue.HasValue && a.Id == selectedValue.Value
        }));

        return items;
    }
}