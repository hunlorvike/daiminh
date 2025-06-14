using Microsoft.AspNetCore.Mvc;
using web.Areas.Client.Services.Interfaces; // Thêm using

namespace web.Areas.Client.Controllers;

[Area("Client")]
public class SearchController : Controller
{
    private readonly IClientSearchService _searchService;

    public SearchController(IClientSearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet("search")] // Sử dụng Attribute Routing cho URL đẹp
    public async Task<IActionResult> Index(string q)
    {
        var viewModel = await _searchService.SearchAsync(q);
        ViewData["SearchQuery"] = q; // Truyền từ khóa tìm kiếm sang View
        return View(viewModel);
    }

    // Xóa action Search cũ đi
}