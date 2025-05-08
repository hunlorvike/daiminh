using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Admin.Validators.FAQ;
using web.Areas.Admin.ViewModels.FAQ;
using X.PagedList;
using X.PagedList.EF;
using System.Text.Json;
using shared.Models;
using shared.Constants;

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
        var result = await new FAQViewModelValidator().ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            viewModel.Categories = await LoadCategoriesSelectListAsync(viewModel.CategoryId);
            return View(viewModel);
        }

        var faq = _mapper.Map<FAQ>(viewModel);
        _context.Add(faq);

        try
        {
            await _context.SaveChangesAsync();
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Thêm FAQ '{faq.Question}' thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo FAQ: {Question}", viewModel.Question);
            ModelState.AddModelError("", "Đã xảy ra lỗi hệ thống khi thêm FAQ.");
        }

        TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
            new ToastData("Lỗi", $"Không thể thêm FAQ '{viewModel.Question}'.", ToastType.Error)
        );
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
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var result = await new FAQViewModelValidator().ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            viewModel.Categories = await LoadCategoriesSelectListAsync(viewModel.CategoryId);
            return View(viewModel);
        }

        var faq = await _context.Set<FAQ>().FirstOrDefaultAsync(f => f.Id == id);
        if (faq == null)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy FAQ để cập nhật.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        _mapper.Map(viewModel, faq);

        try
        {
            await _context.SaveChangesAsync();
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Cập nhật FAQ '{faq.Question}' thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật FAQ: {Question}", viewModel.Question);
            ModelState.AddModelError("", "Đã xảy ra lỗi hệ thống khi cập nhật FAQ.");
        }

        TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
            new ToastData("Lỗi", $"Không thể cập nhật FAQ '{viewModel.Question}'.", ToastType.Error)
        );
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
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy FAQ.", ToastType.Error)
            );
            return Json(new { success = false, message = "Không tìm thấy FAQ." });
        }

        try
        {
            string question = faq.Question;
            _context.Remove(faq);
            await _context.SaveChangesAsync();
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Xóa FAQ '{question}' thành công.", ToastType.Success)
            );
            return Json(new { success = true, message = $"Xóa FAQ '{question}' thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa FAQ ID {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Đã xảy ra lỗi không mong muốn khi xóa FAQ.", ToastType.Error)
            );
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