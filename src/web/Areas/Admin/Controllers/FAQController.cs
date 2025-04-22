using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Admin.ViewModels.FAQ;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public partial class FAQController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<FAQController> _logger;

    public FAQController(
       ApplicationDbContext context,
       IMapper mapper,
       ILogger<FAQController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/FAQ
    public async Task<IActionResult> Index(FAQFilterViewModel filter, int page = 1, int pageSize = 15)
    {

        filter ??= new FAQFilterViewModel();

        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IQueryable<FAQ> query = _context.Set<FAQ>()
                            .Include(fc => fc.Category)
                            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(f => f.Question.ToLower().Contains(lowerSearchTerm)
                                  || f.Answer.ToLower().Contains(lowerSearchTerm));
        }

        if (filter.CategoryId.HasValue && filter.CategoryId > 0)
        {
            query = query.Where(f => f.CategoryId == filter.CategoryId.Value);
        }

        IPagedList<FAQListItemViewModel> faqsPaged = await query
            .OrderBy(f => f.OrderIndex)
            .ThenBy(f => f.Question)
            .ProjectTo<FAQListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, currentPageSize);

        filter.Categories = await LoadCategoriesSelectListAsync(filter.CategoryId);

        FAQIndexViewModel viewModel = new()
        {
            FAQs = faqsPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/FAQ/Create
    public async Task<IActionResult> Create()
    {
        FAQViewModel viewModel = new()
        {
            IsActive = true,
            OrderIndex = 0,
            Categories = await LoadCategoriesSelectListAsync()
        };

        return View(viewModel);
    }

    // POST: Admin/FAQ/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FAQViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            FAQ faq = _mapper.Map<FAQ>(viewModel);
            _context.Add(faq);

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi thêm FAQ.");
            }
        }

        viewModel.Categories = await LoadCategoriesSelectListAsync(viewModel.CategoryId);

        return View(viewModel);
    }

    // GET: Admin/FAQ/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        FAQ? faq = await _context.Set<FAQ>()
                              .AsNoTracking()
                              .FirstOrDefaultAsync(f => f.Id == id);

        if (faq == null)
        {
            return NotFound();
        }

        FAQViewModel viewModel = _mapper.Map<FAQViewModel>(faq);
        viewModel.Categories = await LoadCategoriesSelectListAsync(viewModel.CategoryId);

        return View(viewModel);
    }

    // POST: Admin/FAQ/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, FAQViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            FAQ? faq = await _context.Set<FAQ>().FirstOrDefaultAsync(f => f.Id == id);

            if (faq == null)
            {
                return RedirectToAction(nameof(Index));
            }

            _mapper.Map(viewModel, faq);
            _context.Entry(faq).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật FAQ.");
            }
        }

        viewModel.Categories = await LoadCategoriesSelectListAsync(viewModel.CategoryId);

        return View(viewModel);
    }


    // POST: Admin/FAQ/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        FAQ? faq = await _context.Set<FAQ>().FindAsync(id);
        if (faq == null)
        {
            return Json(new { success = false, message = "Không tìm thấy FAQ." });
        }

        try
        {
            string question = faq.Question;
            _context.Remove(faq);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = $"Xóa FAQ thành công." });
        }
        catch (Exception)
        {
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa FAQ." });
        }
    }
}


public partial class FAQController
{
    private async Task<List<SelectListItem>> LoadCategoriesSelectListAsync(int? selectedValue = null)
    {
        var categories = await _context.Set<Category>()
                                     .Where(c => c.Type == CategoryType.FAQ && c.IsActive)
                                     .OrderBy(c => c.Name)
                                     .Select(c => new { c.Id, c.Name })
                                     .AsNoTracking()
                                     .ToListAsync();

        var items = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "Tất cả danh mục", Selected = !selectedValue.HasValue }
        };

        items.AddRange(categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name,
            Selected = selectedValue.HasValue && c.Id == selectedValue.Value
        }));

        return items;
    }

}