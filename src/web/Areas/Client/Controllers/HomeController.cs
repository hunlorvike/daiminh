using application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Client.Models.Category;
using web.Areas.Client.Models.Home;
using web.Areas.Client.Requests.Subscriber;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public partial class HomeController(
    ICategoryService categoryService,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache);

public partial class HomeController
{
    public async Task<IActionResult> Index()
    {
        var cate = await categoryService.GetAllAsync();
        List<CategoryViewModel> categoryModels = _mapper.Map<List<CategoryViewModel>>(cate);
        var viewModel = new HomeViewModel
        {
            Categories = categoryModels,
            Subscriber = new SubscriberCreateRequest()
        };

        return View(viewModel);

    }
    /*public IActionResult Index()
    {
       
        return View();

    }*/
}