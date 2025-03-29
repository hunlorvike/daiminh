using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Admin.ViewModels.Tag;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class TagController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<TagViewModel> _validator;

    public TagController(
    ApplicationDbContext context,
        IMapper mapper,
        IValidator<TagViewModel> validator)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
    }

    // GET: Admin/Tag
    public async Task<IActionResult> Index(TagType type = TagType.Product, string? searchTerm = null)
    {
        ViewData["PageTitle"] = GetTagTypeTitle(type);
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Thẻ", "")
        };

        var query = _context.Set<Tag>()
            .Include(t => t.ProductTags)
            .Include(t => t.ArticleTags)
            .Include(t => t.ProjectTags)
            .Include(t => t.GalleryTags)
            .Where(t => t.Type == type)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(t => t.Name.Contains(searchTerm) || t.Description.Contains(searchTerm));
        }

        var tags = await query
            .OrderBy(t => t.Name)
        .ToListAsync();

        var viewModels = _mapper.Map<List<TagListItemViewModel>>(tags);

        ViewBag.SearchTerm = searchTerm;
        ViewBag.TagType = type;
        ViewBag.TagTypes = Enum.GetValues(typeof(TagType))
            .Cast<TagType>()
            .Select(t => new { Value = t, Text = GetTagTypeTitle(t) });

        return View(viewModels);
    }

    // GET: Admin/Tag/Create
    public IActionResult Create(TagType type = TagType.Product)
    {
        ViewData["PageTitle"] = $"Thêm {GetTagTypeName(type).ToLower()} mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Thẻ", "/Admin/Tag"),
            ($"Thêm {GetTagTypeName(type).ToLower()}", "")
        };

        var viewModel = new TagViewModel
        {
            Type = type
        };

        ViewBag.TagType = type;
        ViewBag.TagTypeName = GetTagTypeName(type);

        return View(viewModel);
    }

    // POST: Admin/Tag/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TagViewModel viewModel)
    {
        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);

            ViewBag.TagType = viewModel.Type;
            ViewBag.TagTypeName = GetTagTypeName(viewModel.Type);

            return View(viewModel);
        }

        // Check if slug is unique for this tag type
        if (await _context.Set<Tag>().AnyAsync(t => t.Slug == viewModel.Slug && t.Type == viewModel.Type))
        {
            ModelState.AddModelError("Slug", "Slug đã tồn tại cho loại thẻ này, vui lòng chọn slug khác");

            ViewBag.TagType = viewModel.Type;
            ViewBag.TagTypeName = GetTagTypeName(viewModel.Type);

            return View(viewModel);
        }

        var tag = _mapper.Map<Tag>(viewModel);

        _context.Add(tag);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Thêm {GetTagTypeName(viewModel.Type).ToLower()} thành công";
        return RedirectToAction(nameof(Index), new { type = viewModel.Type });
    }

    // GET: Admin/Tag/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var tag = await _context.Set<Tag>().FindAsync(id);

        if (tag == null)
        {
            return NotFound();
        }

        ViewData["PageTitle"] = $"Chỉnh sửa {GetTagTypeName(tag.Type).ToLower()}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Thẻ", "/Admin/Tag"),
            ($"Chỉnh sửa", "")
        };

        var viewModel = _mapper.Map<TagViewModel>(tag);

        ViewBag.TagType = tag.Type;
        ViewBag.TagTypeName = GetTagTypeName(tag.Type);

        return View(viewModel);
    }

    // POST: Admin/Tag/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TagViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);

            ViewBag.TagType = viewModel.Type;
            ViewBag.TagTypeName = GetTagTypeName(viewModel.Type);

            return View(viewModel);
        }

        // Check if slug is unique for this tag type (excluding current tag)
        if (await _context.Set<Tag>().AnyAsync(t => t.Slug == viewModel.Slug && t.Type == viewModel.Type && t.Id != id))
        {
            ModelState.AddModelError("Slug", "Slug đã tồn tại cho loại thẻ này, vui lòng chọn slug khác");

            ViewBag.TagType = viewModel.Type;
            ViewBag.TagTypeName = GetTagTypeName(viewModel.Type);

            return View(viewModel);
        }

        try
        {
            var tag = await _context.Set<Tag>().FindAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, tag);

            _context.Update(tag);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Cập nhật {GetTagTypeName(viewModel.Type).ToLower()} thành công";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await TagExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToAction(nameof(Index), new { type = viewModel.Type });
    }

    // POST: Admin/Tag/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var tag = await _context.Set<Tag>()
            .Include(t => t.ProductTags)
            .Include(t => t.ArticleTags)
            .Include(t => t.ProjectTags)
            .Include(t => t.GalleryTags)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tag == null)
        {
            return Json(new { success = false, message = "Không tìm thấy thẻ" });
        }

        // Check if tag has associated items
        bool hasItems = false;
        string itemType = "";

        switch (tag.Type)
        {
            case TagType.Product:
                hasItems = tag.ProductTags != null && tag.ProductTags.Any();
                itemType = "sản phẩm";
                break;
            case TagType.Article:
                hasItems = tag.ArticleTags != null && tag.ArticleTags.Any();
                itemType = "bài viết";
                break;
            case TagType.Project:
                hasItems = tag.ProjectTags != null && tag.ProjectTags.Any();
                itemType = "dự án";
                break;
            case TagType.Gallery:
                hasItems = tag.GalleryTags != null && tag.GalleryTags.Any();
                itemType = "thư viện";
                break;
        }

        if (hasItems)
        {
            return Json(new { success = false, message = $"Không thể xóa thẻ có chứa {itemType}. Vui lòng xóa hoặc gỡ thẻ khỏi các {itemType} trước." });
        }

        _context.Set<Tag>().Remove(tag);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Xóa thẻ thành công" });
    }

    private async Task<bool> TagExists(int id)
    {
        return await _context.Set<Tag>().AnyAsync(e => e.Id == id);
    }

    private string GetTagTypeTitle(TagType type)
    {
        return type switch
        {
            TagType.Product => "Thẻ sản phẩm",
            TagType.Article => "Thẻ bài viết",
            TagType.Project => "Thẻ dự án",
            TagType.Gallery => "Thẻ thư viện",
            _ => "Thẻ"
        };
    }

    private string GetTagTypeName(TagType type)
    {
        return type switch
        {
            TagType.Product => "Thẻ sản phẩm",
            TagType.Article => "Thẻ bài viết",
            TagType.Project => "Thẻ dự án",
            TagType.Gallery => "Thẻ thư viện",
            _ => "Thẻ"
        };
    }
}

