using AutoMapper;
using AutoMapper.QueryableExtensions;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Client.ViewModels.FAQ;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public class FAQController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<FAQController> _logger;

    public FAQController(ApplicationDbContext context, IMapper mapper, ILogger<FAQController> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var faqCategories = await _context.Categories
            .AsNoTracking()
            .Where(c => c.Type == CategoryType.FAQ && c.IsActive)
            .Include(c => c.FAQs!.Where(f => f.IsActive).OrderBy(f => f.OrderIndex))
            .OrderBy(c => c.OrderIndex)
            .ToListAsync();

        var viewModel = new FAQIndexViewModel
        {
            Categories = _mapper.Map<List<FAQCategoryViewModel>>(faqCategories)
        };

        if (!viewModel.Categories.Any())
        {
            var uncategorizedFaqs = await _context.FAQs
                .AsNoTracking()
                .Where(f => f.CategoryId == null && f.IsActive)
                .OrderBy(f => f.OrderIndex)
                .ProjectTo<FAQItemViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            if (uncategorizedFaqs.Any())
            {
                viewModel.Categories.Add(new FAQCategoryViewModel
                {
                    CategoryName = "Câu hỏi chung",
                    Faqs = uncategorizedFaqs
                });
            }
        }

        return View(viewModel);
    }
}