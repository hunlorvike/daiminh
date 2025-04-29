using AutoMapper;
using AutoMapper.QueryableExtensions;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Attribute;
using X.PagedList.EF;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize] // Add authorization if needed
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
        if (ModelState.IsValid)
        {
            domain.Entities.Attribute attribute = _mapper.Map<domain.Entities.Attribute>(viewModel);

            _context.Add(attribute);

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating attribute.");
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi lưu thuộc tính.");
            }
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
        if (ModelState.IsValid)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            domain.Entities.Attribute? attribute = await _context.Set<domain.Entities.Attribute>().FirstOrDefaultAsync(a => a.Id == id);

            if (attribute == null)
            {
                // Should not happen if loaded in GET, but good practice
                return RedirectToAction(nameof(Index));
            }

            _mapper.Map(viewModel, attribute);

            try
            {
                _context.Update(attribute);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating attribute with ID {AttributeId}.", id);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật thuộc tính.");
            }
        }

        return View(viewModel);
    }

    // POST: Admin/Attribute/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        domain.Entities.Attribute? attribute = await _context.Set<domain.Entities.Attribute>().FirstOrDefaultAsync(a => a.Id == id);

        if (attribute == null)
        {
            return Json(new { success = false, message = "Không tìm thấy thuộc tính." });
        }

        try
        {
            string attributeName = attribute.Name;
            _context.Remove(attribute);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = $"Xóa thuộc tính '{attributeName}' thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting attribute with ID {AttributeId}.", id);
            // Check for FK issues if possible (e.g., if attributes are linked to products/values)
            if (ex is DbUpdateException dbEx && dbEx.InnerException?.Message.Contains("FOREIGN KEY constraint") == true)
            {
                return Json(new { success = false, message = "Không thể xóa thuộc tính này vì nó đang được sử dụng bởi các giá trị thuộc tính hoặc sản phẩm." });
            }
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa thuộc tính." });
        }
    }
}