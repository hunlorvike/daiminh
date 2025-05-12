using AutoMapper;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Client.ViewModels.Page;

namespace web.Areas.Client.Controllers;

[Area("Client")]
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

    public async Task<IActionResult> Detail(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            _logger.LogWarning("Slug của trang là null hoặc rỗng.");
            return NotFound();
        }

        var page = await _dbContext.Pages
                                .AsNoTracking()
                                .Where(p => p.Slug == slug && p.Status == PublishStatus.Published)
                                .FirstOrDefaultAsync();

        if (page == null)
        {
            _logger.LogInformation("Không tìm thấy trang với slug '{Slug}' hoặc trang chưa được xuất bản.", slug);
            return NotFound();
        }

        PageDetailViewModel viewModel = _mapper.Map<PageDetailViewModel>(page);

        return View(viewModel);
    }
}