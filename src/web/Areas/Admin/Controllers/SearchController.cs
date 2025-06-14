using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme")]
public class SearchController : Controller
{
    private readonly IAdminSearchService _searchService;

    public SearchController(IAdminSearchService searchService)
    {
        _searchService = searchService;
    }

    public async Task<IActionResult> Index(string q)
    {
        var viewModel = new AdminSearchViewModel { Query = q };

        if (!string.IsNullOrWhiteSpace(q))
        {
            viewModel.Results = await _searchService.SearchAsync(q);
        }

        return View(viewModel);
    }
}