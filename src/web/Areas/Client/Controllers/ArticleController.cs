using Microsoft.AspNetCore.Mvc;
using web.Areas.Client.Services.Interfaces;
using web.Areas.Client.ViewModels; // Thêm using

namespace web.Areas.Client.Controllers;

[Area("Client")]
public class ArticleController : Controller
{
    private readonly IArticleClientService _articleService;
    private readonly ILogger<ArticleController> _logger;
    private const int PageSize = 6; // 6 bài viết mỗi trang

    public ArticleController(IArticleClientService articleService, ILogger<ArticleController> logger)
    {
        _articleService = articleService;
        _logger = logger;
    }

    [HttpGet("bai-viet")]
    public async Task<IActionResult> Index(int page = 1)
    {
        var articles = await _articleService.GetArticlesAsync(page, PageSize);
        var viewModel = new ArticleIndexViewModel { Articles = articles };
        return View(viewModel);
    }

    [HttpGet("bai-viet/{slug}")]
    public async Task<IActionResult> Detail(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return BadRequest();
        }

        var viewModel = await _articleService.GetArticleBySlugAsync(slug);

        if (viewModel == null)
        {
            return NotFound();
        }

        return View(viewModel);
    }
}