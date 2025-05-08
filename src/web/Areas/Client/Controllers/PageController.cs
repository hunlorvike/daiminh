using AutoMapper;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Client.ViewModels.Page;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("trang")]
public class PageController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<PageController> _logger;

    public PageController(ApplicationDbContext dbContext, IMapper mapper, ILogger<PageController> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: /{pageSlug}
    [HttpGet("{pageSlug}")]
    public async Task<IActionResult> Detail(string pageSlug)
    {
        if (string.IsNullOrEmpty(pageSlug))
        {
            _logger.LogWarning("Page slug is null or empty.");
            return NotFound();
        }
        var page = await _dbContext.Pages
                                .AsNoTracking()
                                .Where(p => p.Slug == pageSlug && p.Status == PublishStatus.Published)
                                .FirstOrDefaultAsync();
        if (page == null)
        {
            _logger.LogInformation("Page with slug '{Slug}' not found or not published.", pageSlug);
            return NotFound();
        }

        PageDetailViewModel viewModel = _mapper.Map<PageDetailViewModel>(page);

        return View(viewModel);
    }
}