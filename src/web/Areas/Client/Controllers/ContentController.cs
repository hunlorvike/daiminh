using application.Interfaces;
using application.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Client.Models.Content;
using web.Areas.Client.Models.ContentType;
using web.Areas.Client.Models.Home;
using web.Areas.Client.Requests.Subscriber;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("bai-viet")]
public partial class ContentController(
    IContentService contentService,
    IContentTypeService contentTypeService,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class ContentController : DaiminhController
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var contents = await contentService.GetAllAsync();
        List<ContentViewModel> contentModels = _mapper.Map<List<ContentViewModel>>(contents);

        var contentTypes = await contentTypeService.GetAllAsync();
        List<ContentTypeViewModel> contentTypeModels = _mapper.Map<List<ContentTypeViewModel>>(contentTypes);

        var latestContent = await contentService.GetLatestContentAsync();
        var latestContentModel = _mapper.Map<ContentViewModel>(latestContent);
         var viewModel = new HomeViewModel
        {
            Contents = contentModels,
            ContentTypes = contentTypeModels,
            LatestContent = latestContentModel,
            Subscriber = new SubscriberCreateRequest()
        };
        return View(viewModel);

    }
    /* [HttpGet]
     public IActionResult Index()
     {

         return View();

     }*/

    [HttpGet("ve-chung-toi")]
    public IActionResult AboutUs()
    {
        return View();
    }

    [HttpGet("{id}")]
    public IActionResult Detail(string id)
    {
        return View();
    }
}