// --- START OF FILE Controllers/ProjectController.cs --- (New File)
using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.ViewModels.Project; // Use public view model namespace
using X.PagedList.EF;

namespace web.Controllers; // Adjust namespace

public class ProjectController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ProjectController> _logger;
    // Optional: Inject a service to get SEO defaults if needed
    // private readonly ISeoService _seoService;

    public ProjectController(ApplicationDbContext context, IMapper mapper, ILogger<ProjectController> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: /du-an or /du-an/danh-muc/{categorySlug}
    [Route("du-an/{categorySlug?}")]
    public async Task<IActionResult> Index(string? categorySlug = null, int page = 1, int pageSize = 9) // Adjust pageSize
    {
        int pageNumber = page;
        Category? currentCategory = null;

        var query = _context.Projects
            .Where(p => p.PublishStatus == PublishStatus.Published) // Only published
            .Include(p => p.Images.OrderBy(i => i.OrderIndex))
            .Include(p => p.ProjectCategories).ThenInclude(pc => pc.Category)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(categorySlug))
        {
            currentCategory = await _context.Categories
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(c => c.Slug == categorySlug && c.Type == CategoryType.Project && c.IsActive);
            if (currentCategory != null)
            {
                query = query.Where(p => p.ProjectCategories.Any(pc => pc.CategoryId == currentCategory.Id));
                ViewData["Title"] = $"Dự án: {currentCategory.Name}";
                ViewData["CategoryName"] = currentCategory.Name;
                // TODO: Add Category SEO to ViewData or ViewModel if needed
            }
            else
            {
                _logger.LogWarning("Project category with slug '{CategorySlug}' not found.", categorySlug);
                // Optional: Redirect to main project index or show a not found message
                // return NotFound();
                return RedirectToAction(nameof(Index), new { categorySlug = (string?)null });
            }
        }
        else
        {
            ViewData["Title"] = "Dự án đã thực hiện";
        }

        // Sorting (e.g., featured first, then by completion date)
        query = query.OrderByDescending(p => p.IsFeatured)
                     .ThenByDescending(p => p.CompletionDate ?? p.StartDate ?? p.CreatedAt);

        var projectsPaged = await query
                                    .ProjectTo<ProjectListItemViewModel>(_mapper.ConfigurationProvider)
                                    .ToPagedListAsync(pageNumber, pageSize);

        // For Filters (Optional)
        ViewBag.Categories = await _context.Categories
                                        .Where(c => c.Type == CategoryType.Project && c.IsActive && c.ProjectCategories.Any(pc => pc.Project.PublishStatus == PublishStatus.Published)) // Only show categories with published projects
                                        .OrderBy(c => c.OrderIndex).ThenBy(c => c.Name)
                                        .Select(c => new SelectListItem { Value = c.Slug, Text = c.Name, Selected = c.Slug == categorySlug })
                                        .ToListAsync();
        ViewBag.CurrentCategorySlug = categorySlug;


        return View(projectsPaged);
    }

    // GET: /du-an/chi-tiet/{slug}
    [Route("du-an/chi-tiet/{slug}")]
    public async Task<IActionResult> Details(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return BadRequest();
        }

        var project = await _context.Projects
            .Where(p => p.Slug == slug && p.PublishStatus == PublishStatus.Published)
            // Eager load all necessary related data
            .Include(p => p.Images.OrderBy(i => i.OrderIndex))
            .Include(p => p.ProjectCategories).ThenInclude(pc => pc.Category)
            .Include(p => p.ProjectTags).ThenInclude(pt => pt.Tag)
            .Include(p => p.ProjectProducts.OrderBy(pp => pp.OrderIndex)).ThenInclude(pp => pp.Product).ThenInclude(prod => prod.Images.OrderBy(i => i.OrderIndex)) // Include products and their images
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (project == null)
        {
            _logger.LogWarning("Published project with slug '{ProjectSlug}' not found.", slug);
            return NotFound(); // Or redirect to a 404 page
        }

        // --- Increment View Count (Example - consider throttling) ---
        var trackedProject = await _context.Projects.FindAsync(project.Id);
        if (trackedProject != null)
        {
            trackedProject.ViewCount++;
            await _context.SaveChangesAsync();
        }
        // --- End View Count ---

        var viewModel = _mapper.Map<ProjectDetailViewModel>(project);

        // Set SEO data for the view's <head>
        ViewData["Title"] = viewModel.MetaTitle ?? viewModel.Name;
        ViewData["MetaDescription"] = viewModel.MetaDescription;
        ViewData["MetaKeywords"] = viewModel.MetaKeywords;
        ViewData["CanonicalUrl"] = viewModel.CanonicalUrl ?? Url.Action("Details", "Project", new { slug = viewModel.Slug }, Request.Scheme);
        // TODO: Add OG/Twitter data to ViewData or directly in _Layout using the ViewModel

        return View(viewModel);
    }
}
// --- END OF FILE Controllers/ProjectController.cs ---