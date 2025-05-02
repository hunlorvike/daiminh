using AutoMapper;
using AutoMapper.QueryableExtensions;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.Validators.Attribute;
using web.Areas.Admin.ViewModels.Attribute;
using X.PagedList.EF;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class AttributeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<AttributeController> _logger;

    public AttributeController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<AttributeController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/Attribute
    public async Task<IActionResult> Index(AttributeFilterViewModel filter, int page = 1, int pageSize = 25)
    {
        filter ??= new AttributeFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 25;

        IQueryable<domain.Entities.Attribute> query = _context.Set<domain.Entities.Attribute>()
                                                        .Include(a => a.Values) // Include to count
                                                        .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(a => a.Name.ToLower().Contains(lowerSearchTerm) ||
                                     a.Slug.ToLower().Contains(lowerSearchTerm));
        }

        query = query.OrderBy(a => a.Name);

        var attributesPaged = await query.ProjectTo<AttributeListItemViewModel>(_mapper.ConfigurationProvider)
                                         .ToPagedListAsync(pageNumber, currentPageSize);

        AttributeIndexViewModel viewModel = new()
        {
            Attributes = attributesPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/Attribute/Create
    public IActionResult Create()
    {
        AttributeViewModel viewModel = new();
        return View(viewModel);
    }

    // POST: Admin/Attribute/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AttributeViewModel viewModel)
    {
        var result = await new AttributeViewModelValidator(_context).ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return View(viewModel);
        }

        var attribute = _mapper.Map<domain.Entities.Attribute>(viewModel);
        _context.Add(attribute);

        try
        {
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã thêm thuộc tính '{attribute.Name}' thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo thuộc tính: {Name}", viewModel.Name);
            if (ex.InnerException?.Message.Contains("slug", StringComparison.OrdinalIgnoreCase) == true)
            {
                ModelState.AddModelError(nameof(viewModel.Slug), "Slug này đã tồn tại.");
            }
            else
            {
                ModelState.AddModelError("", "Lỗi cơ sở dữ liệu khi lưu thuộc tính.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi tạo thuộc tính.");
            ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn.");
        }

        return View(viewModel);
    }

    // GET: Admin/Attribute/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        domain.Entities.Attribute? attribute = await _context.Set<domain.Entities.Attribute>()
                                                     .AsNoTracking()
                                                     .FirstOrDefaultAsync(a => a.Id == id);

        if (attribute == null)
        {
            return NotFound();
        }

        AttributeViewModel viewModel = _mapper.Map<AttributeViewModel>(attribute);
        return View(viewModel);
    }

    // POST: Admin/Attribute/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AttributeViewModel viewModel)
    {
        if (id != viewModel.Id) return BadRequest();

        var result = await new AttributeViewModelValidator(_context).ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return View(viewModel);
        }

        var attribute = await _context.Set<domain.Entities.Attribute>().FirstOrDefaultAsync(a => a.Id == id);
        if (attribute == null)
        {
            TempData["ErrorMessage"] = "Không tìm thấy thuộc tính để cập nhật.";
            return RedirectToAction(nameof(Index));
        }

        _mapper.Map(viewModel, attribute);

        try
        {
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã cập nhật thuộc tính '{attribute.Name}' thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi cập nhật thuộc tính ID: {Id}", id);
            if (ex.InnerException?.Message.Contains("slug", StringComparison.OrdinalIgnoreCase) == true)
            {
                ModelState.AddModelError(nameof(viewModel.Slug), "Slug này đã được sử dụng.");
            }
            else
            {
                ModelState.AddModelError("", "Lỗi cơ sở dữ liệu khi cập nhật thuộc tính.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi cập nhật thuộc tính ID: {Id}", id);
            ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn.");
        }

        return View(viewModel);
    }

    // POST: Admin/Attribute/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var attribute = await _context.Set<domain.Entities.Attribute>()
                                      .Include(a => a.Values)
                                      .FirstOrDefaultAsync(a => a.Id == id);

        if (attribute == null)
        {
            _logger.LogWarning("Không tìm thấy thuộc tính ID {Id} để xóa.", id);
            return Json(new { success = false, message = "Không tìm thấy thuộc tính." });
        }

        try
        {
            string name = attribute.Name;
            _context.Remove(attribute);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Đã xóa thuộc tính: {Name} (ID: {Id})", name, id);
            return Json(new { success = true, message = $"Xóa thuộc tính '{name}' thành công." });
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi quan hệ khi xóa thuộc tính ID {Id}", id);
            if (ex.InnerException?.Message.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) == true)
            {
                return Json(new { success = false, message = "Không thể xóa vì thuộc tính đang được sử dụng." });
            }
            return Json(new { success = false, message = "Lỗi cơ sở dữ liệu khi xóa thuộc tính." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi xóa thuộc tính ID {Id}", id);
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa thuộc tính." });
        }
    }

}