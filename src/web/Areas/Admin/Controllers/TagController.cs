using System.Reflection.Metadata;
using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Extensions;
using web.Areas.Admin.ViewModels.Tag;
using X.PagedList.EF;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public partial class TagController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public TagController(
    ApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: Admin/Tag
    public async Task<IActionResult> Index(int? page, TagType type = TagType.Product, string? searchTerm = null)
    {
        ViewData["PageTitle"] = $"Quản lý {type.GetDisplayName()}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ($"Thẻ {type.GetDisplayName()}", Url.Action("Index", "Tag", new { area = "Admin", type})),
        };

        var query = _context.Set<Tag>()
            .Where(t => t.Type == type);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(t => t.Name.Contains(searchTerm) ||
                                    (t.Description != null && t.Description.Contains(searchTerm)));
        }

        query = query.Include(t => t.ProductTags)
                    .Include(t => t.ArticleTags)
                    .Include(t => t.ProjectTags)
                    .Include(t => t.GalleryTags);

        int pageSize = 10;
        int pageNumber = page ?? 1;

        // Use AutoMapper's ProjectTo method for efficient mapping in the query
        var pagedList = await query
            .OrderBy(t => t.CreatedAt)
            .ProjectTo<TagListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        ViewBag.SearchTerm = searchTerm;
        ViewBag.TagType = type;
        ViewBag.TagTypes = Enum.GetValues(typeof(TagType))
            .Cast<TagType>()
            .Select(t => new SelectListItem
            {
                Value = t.ToString(),
                Text = t.GetDisplayName(),
                Selected = t.Equals(type)
            })
            .ToList();

        return View(pagedList);
    }

    // GET: Admin/Tag/Create
    public IActionResult Create(TagType type = TagType.Product)
    {
        string typeNameLower = type.GetDisplayName().ToLowerInvariant();
        ViewData["PageTitle"] = $"Thêm {typeNameLower} mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ($"Thẻ {type.GetDisplayName()}", Url.Action("Index", "Tag", new { area = "Admin", type})),
            ($"Thêm {typeNameLower}", "")
        };

        var viewModel = new TagViewModel
        {
            Type = type
        };

        ViewBag.CurrentTagTypeName = type.GetDisplayName();

        return View(viewModel);
    }

    // POST: Admin/Tag/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TagViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var tag = _mapper.Map<Tag>(viewModel);

                _context.Add(tag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { type = tag.Type });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình tạo thẻ. Vui lòng thử lại.");
            }
        }

        string typeDisplayName = viewModel.Type.GetDisplayName();
        ViewData["Title"] = $"Thêm thẻ {typeDisplayName.ToLowerInvariant()} - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Thêm thẻ {typeDisplayName} mới";
        return View(viewModel);
    }

    // GET: Admin/Tag/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var tag = await _context.Set<Tag>().FindAsync(id);

        if (tag == null)
        {
            return NotFound();
        }

        string typeNameLower = tag.Type.GetDisplayName().ToLowerInvariant();
        ViewData["PageTitle"] = $"Chỉnh sửa {typeNameLower}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ($"Thẻ {tag.Type.GetDisplayName()}", Url.Action("Index", "Tag", new { area = "Admin", type = tag.Type })),
            ($"Chỉnh sửa", "")
        };

        var viewModel = _mapper.Map<TagViewModel>(tag);

        ViewBag.CurrentTagTypeName = tag.Type.GetDisplayName();

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

        if (ModelState.IsValid)
        {
            try
            {
                var tagToUpdate = await _context.Set<Tag>().FindAsync(id);
                if (tagToUpdate == null)
                {
                    return NotFound();
                }

                _mapper.Map(viewModel, tagToUpdate);

                _context.Update(tagToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { type = tagToUpdate.Type });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật thẻ. Vui lòng thử lại.");
            }
        }

        string typeDisplayName = viewModel.Type.GetDisplayName();
        ViewData["Title"] = $"Chỉnh sửa thẻ {typeDisplayName.ToLowerInvariant()} - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa thẻ: {viewModel.Name}";
        return View(viewModel);
    }

    // POST: Admin/Tag/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var tag = await _context.Set<Tag>().FindAsync(id);

        if (tag == null)
        {
            return Json(new { success = false, message = "Không tìm thấy thẻ" });
        }

        bool hasItems = false;
        string itemTypeName = string.Empty;

        switch (tag.Type)
        {
            case TagType.Product:
                hasItems = await _context.ProductTags.AnyAsync(pt => pt.TagId == id);
                itemTypeName = TagType.Product.GetDisplayName().ToLowerInvariant();
                break;
            case TagType.Article:
                hasItems = await _context.ArticleTags.AnyAsync(at => at.TagId == id);
                itemTypeName = TagType.Article.GetDisplayName().ToLowerInvariant();
                break;
            case TagType.Project:
                hasItems = await _context.ProjectTags.AnyAsync(prt => prt.TagId == id);
                itemTypeName = TagType.Project.GetDisplayName().ToLowerInvariant();
                break;
            case TagType.Gallery:
                hasItems = await _context.GalleryTags.AnyAsync(gt => gt.TagId == id);
                itemTypeName = TagType.Gallery.GetDisplayName().ToLowerInvariant();
                break;
        }

        if (hasItems)
        {
            return Json(new { success = false, message = $"Không thể xóa thẻ đang được sử dụng bởi ít nhất một {itemTypeName}. Vui lòng gỡ thẻ khỏi các {itemTypeName} trước." });
        }

        _context.Set<Tag>().Remove(tag);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Xóa {tag.Type.GetDisplayName().ToLowerInvariant()} '{tag.Name}' thành công"; // Thêm tên thẻ bị xóa
        return Json(new { success = true, message = $"Xóa {tag.Type.GetDisplayName().ToLowerInvariant()} thành công" });
    }

    private async Task<bool> TagExists(int id)
    {
        return await _context.Set<Tag>().AnyAsync(e => e.Id == id);
    }
}

