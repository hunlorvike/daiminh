using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Constants;
using shared.Enums;
using shared.Extensions;
using shared.Models;
using System.Security.Claims;
using System.Text.Json;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme", Policy = PermissionConstants.AdminAccess)]
public partial class ArticleController : Controller
{
    private readonly IArticleService _articleService;
    private readonly ICategoryService _categoryService;
    private readonly ITagService _tagService;
    private readonly IMapper _mapper;
    private readonly ILogger<ArticleController> _logger;
    private readonly IValidator<ArticleViewModel> _articleViewModelValidator;

    public ArticleController(
        IArticleService articleService,
        ICategoryService categoryService,
        ITagService tagService,
        IMapper mapper,
        ILogger<ArticleController> logger,
        IValidator<ArticleViewModel> articleViewModelValidator)
    {
        _articleService = articleService ?? throw new ArgumentNullException(nameof(articleService));
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        _tagService = tagService ?? throw new ArgumentNullException(nameof(tagService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _articleViewModelValidator = articleViewModelValidator ?? throw new ArgumentNullException(nameof(articleViewModelValidator));
    }

    // GET: Admin/Article
    [Authorize(Policy = PermissionConstants.ArticleView)]
    public async Task<IActionResult> Index(ArticleFilterViewModel filter, int page = 1, int pageSize = 25)
    {
        filter ??= new ArticleFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 25;

        IPagedList<ArticleListItemViewModel> articlesPaged = await _articleService.GetPagedArticlesAsync(filter, pageNumber, currentPageSize);

        filter.CategoryOptions = await _categoryService.GetParentCategorySelectListAsync(CategoryType.Article, filter.CategoryId);
        filter.StatusOptions = GetPublishStatusSelectList(filter.Status);
        filter.IsFeaturedOptions = GetYesNoSelectList(filter.IsFeatured, "Tất cả");

        ArticleIndexViewModel viewModel = new()
        {
            Articles = articlesPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/Article/Create
    [Authorize(Policy = PermissionConstants.ArticleCreate)]
    public async Task<IActionResult> Create()
    {
        ArticleViewModel viewModel = new()
        {
            IsFeatured = false,
            Status = PublishStatus.Draft,
            PublishedAt = DateTime.Now,
            SitemapPriority = 0.5,
            SitemapChangeFrequency = "monthly",
            AuthorName = User.Identity?.Name
        };

        await PopulateViewModelSelectListsAsync(viewModel);

        return View(viewModel);
    }

    // POST: Admin/Article/Create
    [Authorize(Policy = PermissionConstants.ArticleCreate)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ArticleViewModel viewModel)
    {
        var result = await _articleViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            await PopulateViewModelSelectListsAsync(viewModel);
            return View(viewModel);
        }

        var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var authorName = User.Identity?.Name;


        var createResult = await _articleService.CreateArticleAsync(viewModel, authorId, authorName);

        if (createResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", createResult.Message ?? "Thêm bài viết thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in createResult.Errors)
            {
                if (error.Contains("Slug", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.Slug), error);
                }
                else if (error.Contains("danh mục", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.CategoryId), error);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            if (!createResult.Errors.Any() && !string.IsNullOrEmpty(createResult.Message))
            {
                ModelState.AddModelError(string.Empty, createResult.Message);
            }


            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", createResult.Message ?? $"Không thể thêm bài viết '{viewModel.Title}'.", ToastType.Error)
            );
            await PopulateViewModelSelectListsAsync(viewModel);
            return View(viewModel);
        }
    }

    // GET: Admin/Article/Edit/5
    [Authorize(Policy = PermissionConstants.ArticleEdit)]
    public async Task<IActionResult> Edit(int id)
    {
        ArticleViewModel? viewModel = await _articleService.GetArticleByIdAsync(id);

        if (viewModel == null)
        {
            _logger.LogWarning("Article not found for editing: ID {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                 new ToastData("Lỗi", "Không tìm thấy bài viết để chỉnh sửa.", ToastType.Error)
             );
            return RedirectToAction(nameof(Index));
        }

        await PopulateViewModelSelectListsAsync(viewModel);

        return View(viewModel);
    }

    // POST: Admin/Article/Edit/5
    [Authorize(Policy = PermissionConstants.ArticleEdit)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ArticleViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var result = await _articleViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            await PopulateViewModelSelectListsAsync(viewModel);
            return View(viewModel);
        }

        var updateResult = await _articleService.UpdateArticleAsync(viewModel);

        if (updateResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", updateResult.Message ?? "Cập nhật bài viết thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in updateResult.Errors)
            {
                if (error.Contains("Slug", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.Slug), error);
                }
                else if (error.Contains("danh mục", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.CategoryId), error);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            if (!updateResult.Errors.Any() && !string.IsNullOrEmpty(updateResult.Message))
            {
                ModelState.AddModelError(string.Empty, updateResult.Message);
            }

            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", updateResult.Message ?? $"Không thể cập nhật bài viết '{viewModel.Title}'.", ToastType.Error)
            );
            await PopulateViewModelSelectListsAsync(viewModel);
            return View(viewModel);
        }
    }

    // POST: Admin/Article/Delete/5
    [Authorize(Policy = PermissionConstants.ArticleDelete)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteResult = await _articleService.DeleteArticleAsync(id);

        if (deleteResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", deleteResult.Message ?? "Xóa bài viết thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", deleteResult.Message ?? "Đã xảy ra lỗi không mong muốn khi xóa bài viết.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }
    }
}

public partial class ArticleController
{
    private async Task PopulateViewModelSelectListsAsync(ArticleViewModel viewModel)
    {
        viewModel.CategoryOptions = await _categoryService.GetParentCategorySelectListAsync(CategoryType.Article, viewModel.CategoryId);
        viewModel.StatusOptions = GetPublishStatusSelectList(viewModel.Status);
        viewModel.TagOptions = await _tagService.GetTagSelectListAsync(TagType.Article, viewModel.SelectedTagIds);
    }

    private List<SelectListItem> GetPublishStatusSelectList(PublishStatus? selectedStatus)
    {
        var items = Enum.GetValues(typeof(PublishStatus))
            .Cast<PublishStatus>()
            .Select(t => new SelectListItem
            {
                Value = ((int)t).ToString(),
                Text = t.GetDisplayName(),
                Selected = selectedStatus.HasValue && t == selectedStatus.Value
            })
            .OrderBy(t => t.Text)
            .ToList();

        items.Insert(0, new SelectListItem { Value = "", Text = "Tất cả trạng thái", Selected = !selectedStatus.HasValue });

        return items;
    }

    private List<SelectListItem> GetYesNoSelectList(bool? selectedValue, string allText = "Tất cả")
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = allText, Selected = !selectedValue.HasValue },
            new SelectListItem { Value = "true", Text = "Có", Selected = selectedValue == true },
            new SelectListItem { Value = "false", Text = "Không", Selected = selectedValue == false }
        };
    }
}
