using AutoMapper;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Client.Models.Category;
using web.Areas.Client.Models.Home;
using web.Areas.Client.Requests.Subscriber;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public partial class HomeController(
    ApplicationDbContext context,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache);

public partial class HomeController
{
    public async Task<IActionResult> Index()
    {
        var categories = await context.Categories.ToListAsync();
        List<CategoryViewModel> categoryModels = _mapper.Map<List<CategoryViewModel>>(categories);
        var viewModel = new HomeViewModel
        {
            Categories = categoryModels,
            Subscriber = new SubscriberCreateRequest()
        };

        return View(viewModel);

    }
}