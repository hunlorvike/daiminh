using Microsoft.AspNetCore.Mvc;
using web.Areas.Client.Services.Interfaces;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public class BrandController : Controller
{
    private readonly IBrandClientService _brandService;
    private readonly ILogger<BrandController> _logger;
    private const int PageSize = 12; // Số sản phẩm trên mỗi trang

    public BrandController(IBrandClientService brandService, ILogger<BrandController> logger)
    {
        _brandService = brandService;
        _logger = logger;
    }

    // Trang chi tiết một thương hiệu
    [HttpGet("thuong-hieu/{slug}")]
    public async Task<IActionResult> Detail(string slug, int page = 1)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return BadRequest();
        }

        var viewModel = await _brandService.GetBrandDetailBySlugAsync(slug, page, PageSize);

        if (viewModel == null)
        {
            _logger.LogWarning("Yêu cầu trang thương hiệu không tồn tại với slug: {Slug}", slug);
            return NotFound();
        }

        return View(viewModel);
    }
}