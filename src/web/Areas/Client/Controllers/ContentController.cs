using AutoMapper;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Client.Models.Content;
using web.Areas.Client.Models.ContentType;
using web.Areas.Client.Models.Home;
using web.Areas.Client.Requests.Subscriber;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("bai-viet")]
public partial class ContentController(
    ApplicationDbContext context,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache);

public partial class ContentController : DaiminhController
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var contents = await context.Contents
            .AsNoTracking()
            .ToListAsync();
        List<ContentViewModel> contentModels = _mapper.Map<List<ContentViewModel>>(contents);

        var contentTypes = await context.ContentTypes
            .AsNoTracking()
            .ToListAsync();
        List<ContentTypeViewModel> contentTypeModels = _mapper.Map<List<ContentTypeViewModel>>(contentTypes);

        var latestContent = await context.Contents
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .FirstOrDefaultAsync();
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