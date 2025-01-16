using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("/san-pham")]
public class ProductController : Controller
{
    public IActionResult Index() => View();

    [HttpGet("{identifier:slugOrId}")]
    public async Task<IActionResult> Detail(string identifier)
    {
        // var product = int.TryParse(identifier, out var id)
        //     ? await _productService.GetByIdAsync(id)
        //     : await _productService.GetBySlugAsync(identifier);
        //
        // if (product == null)
        //     return NotFound();
        //
        // // Chuyển hướng về slug chính xác nếu URL không khớp
        // if (!string.Equals(identifier, product.Slug) && !string.Equals(identifier, product.Id.ToString()))
        // {
        //     return RedirectToActionPermanent(nameof(Detail), new { identifier = product.Slug });
        // }
        //
        // return View(product);
        return View();
    }
}