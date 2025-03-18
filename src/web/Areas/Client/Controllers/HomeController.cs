using application.Interfaces;
using application.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Models.Setting;
using web.Areas.Client.Models.Category;
using web.Areas.Client.Models.Home;
using web.Areas.Client.Requests.Subscriber;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public partial class HomeController(
    ICategoryService categoryService,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration) : DaiminhController(mapper, serviceProvider, configuration);

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