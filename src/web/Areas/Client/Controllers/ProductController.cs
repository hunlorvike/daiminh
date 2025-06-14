using Microsoft.AspNetCore.Mvc;
using web.Areas.Client.Services.Interfaces; // Thêm using
using web.Areas.Client.ViewModels; // Thêm using

namespace web.Areas.Client.Controllers;

[Area("Client")]
public class ProductController : Controller
{
    private readonly IProductClientService _productService;
    private readonly ILogger<ProductController> _logger;
    private const int PageSize = 12; // 12 sản phẩm mỗi trang

    public ProductController(IProductClientService productService, ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet("san-pham")]
    public async Task<IActionResult> Index(ProductFilterViewModel filter, int page = 1)
    {
        var viewModel = await _productService.GetProductIndexViewModelAsync(filter, page, PageSize);
        return View(viewModel);
    }

    [HttpGet("san-pham/{slug}")] // Cập nhật Route Attribute cho rõ ràng
    public async Task<IActionResult> Detail(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return BadRequest();
        }

        var viewModel = await _productService.GetProductDetailBySlugAsync(slug);

        if (viewModel == null)
        {
            _logger.LogWarning("Yêu cầu trang sản phẩm không tồn tại với slug: {Slug}", slug);
            return NotFound(); // Trả về trang lỗi 404
        }

        return View(viewModel);
    }
}