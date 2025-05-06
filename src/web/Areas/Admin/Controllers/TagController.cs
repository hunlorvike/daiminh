using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using FluentValidation;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Extensions;
using web.Areas.Admin.ViewModels.Tag;
using X.PagedList;
using X.PagedList.EF;
using System.Text.Json;
using shared.Models;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public partial class TagController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<TagController> _logger;

    public TagController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<TagController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    // GET: Admin/Tag
    public async Task<IActionResult> Index(TagFilterViewModel filter, int page = 1, int pageSize = 10)
    {
        filter ??= new TagFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 10;

        IQueryable<Tag> query = _context.Set<Tag>();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(t => t.Name.ToLower().Contains(lowerSearchTerm) ||
                                    (t.Description != null && t.Description.ToLower().Contains(lowerSearchTerm)));
        }

        if (filter.Type.HasValue)
        {
            query = query.Where(c => c.Type == filter.Type.Value);
        }

        query = query.Include(t => t.ProductTags)
                     .Include(t => t.ArticleTags);


        IPagedList<TagListItemViewModel> pagedList = await query
            .OrderBy(t => t.Name)
            .ProjectTo<TagListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, currentPageSize);

        filter.TagTypes = GetTagTypesSelectList(filter.Type);

        TagIndexViewModel viewModel = new()
        {
            Tags = pagedList,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/Tag/Create
    public IActionResult Create()
    {
        TagViewModel viewModel = new()
        {
            TagTypes = GetTagTypesSelectList()
        };

        return View(viewModel);
    }

    // POST: Admin/Tag/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TagViewModel viewModel)
    {
        var validationResult = await new TagViewModelValidator(_context).ValidateAsync(viewModel);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            viewModel.TagTypes = GetTagTypesSelectList(viewModel.Type);
            return View(viewModel);
        }

        var tag = _mapper.Map<Tag>(viewModel);
        _context.Add(tag);

        try
        {
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Thêm thẻ '{tag.Name}' thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo thẻ mới '{Name}'", viewModel.Name);
            ModelState.AddModelError("", "Đã xảy ra lỗi hệ thống khi tạo thẻ.");
        }

        TempData["ToastMessage"] = JsonSerializer.Serialize(
            new ToastData("Lỗi", $"Không thể thêm thẻ '{viewModel.Name}'.", ToastType.Error)
        );
        viewModel.TagTypes = GetTagTypesSelectList(viewModel.Type);
        return View(viewModel);
    }


    // GET: Admin/Tag/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        Tag? tag = await _context.Set<Tag>().AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);

        if (tag == null)
        {
            return NotFound();
        }

        TagViewModel viewModel = _mapper.Map<TagViewModel>(tag);
        viewModel.TagTypes = GetTagTypesSelectList(tag.Type);
        return View(viewModel);
    }

    // POST: Admin/Tag/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TagViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var validationResult = await new TagViewModelValidator(_context).ValidateAsync(viewModel);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            viewModel.TagTypes = GetTagTypesSelectList(viewModel.Type);
            return View(viewModel);
        }

        var tag = await _context.Set<Tag>().FirstOrDefaultAsync(t => t.Id == id);
        if (tag == null)
        {
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy thẻ để cập nhật.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        _mapper.Map(viewModel, tag);

        try
        {
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Cập nhật thẻ '{tag.Name}' thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật thẻ '{Name}'", viewModel.Name);
            ModelState.AddModelError("", "Đã xảy ra lỗi hệ thống khi cập nhật thẻ.");
        }

        TempData["ToastMessage"] = JsonSerializer.Serialize(
            new ToastData("Lỗi", $"Không thể cập nhật thẻ '{viewModel.Name}'.", ToastType.Error)
        );
        viewModel.TagTypes = GetTagTypesSelectList(viewModel.Type);
        return View(viewModel);
    }


    // POST: Admin/Tag/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var tag = await _context.Set<Tag>()
            .Where(t => t.Id == id)
            .Select(t => new
            {
                t.Id,
                t.Name,
                t.Type,
                ProductCount = t.ProductTags!.Count(),
                ArticleCount = t.ArticleTags!.Count(),
            })
            .FirstOrDefaultAsync();

        if (tag == null)
        {
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy thẻ.", ToastType.Error)
            );
            return Json(new { success = false, message = "Không tìm thấy thẻ." });
        }

        int totalItems = tag.ProductCount + tag.ArticleCount;
        string itemTypeName = tag.Type.GetDisplayName().ToLowerInvariant();

        if (totalItems > 0)
        {
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", $"Không thể xóa thẻ '{tag.Name}' vì đang được sử dụng bởi {totalItems} {itemTypeName}.", ToastType.Error)
            );
            return Json(new { success = false, message = $"Không thể xóa thẻ '{tag.Name}' vì đang được sử dụng bởi {totalItems} {itemTypeName}." });
        }

        Tag tagToDelete = new() { Id = id };
        _context.Entry(tagToDelete).State = EntityState.Deleted;

        try
        {
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Xóa thẻ '{tag.Name}' thành công.", ToastType.Success)
            );
            return Json(new { success = true, message = $"Xóa thẻ '{tag.Name}' thành công." });
        }
        catch (Exception)
        {
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Đã xảy ra lỗi không mong muốn khi xóa thẻ.", ToastType.Error)
            );
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa thẻ." });
        }
    }
}

public partial class TagController
{
    private List<SelectListItem> GetTagTypesSelectList(TagType? selectedType = null)
    {
        List<SelectListItem> tagTypes = Enum.GetValues(typeof(TagType))
            .Cast<TagType>()
            .Select(t => new SelectListItem
            {
                Value = ((int)t).ToString(),
                Text = t.GetDisplayName(),
                Selected = selectedType.HasValue && t == selectedType.Value
            })
            .OrderBy(t => t.Text)
            .ToList();

        List<SelectListItem> items = new()
        {
            new SelectListItem
            {
                Value = "",
                Text = "Tất cả loại thẻ",
                Selected = !selectedType.HasValue
            }
        };

        items.AddRange(tagTypes);

        return items;
    }
}