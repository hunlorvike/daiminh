using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.AttributeValue;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize] // Add authorization if needed
public partial class AttributeValueController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<AttributeValueController> _logger;

    public AttributeValueController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<AttributeValueController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/AttributeValue
    public async Task<IActionResult> Index(AttributeValueFilterViewModel filter, int page = 1, int pageSize = 25)
    {
        filter ??= new AttributeValueFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 25;

        IQueryable<AttributeValue> query = _context.Set<AttributeValue>()
                                                .Include(av => av.Attribute) // Include parent attribute
                                                .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(av => av.Value.ToLower().Contains(lowerSearchTerm) ||
                                      (av.Slug != null && av.Slug.ToLower().Contains(lowerSearchTerm)));
        }

        if (filter.AttributeId.HasValue)
        {
            query = query.Where(av => av.AttributeId == filter.AttributeId.Value);
        }

        query = query.OrderBy(av => av.Attribute!.Name) // Order by parent attribute name
                     .ThenBy(av => av.Value);

        var valuesPaged = await query.ProjectTo<AttributeValueListItemViewModel>(_mapper.ConfigurationProvider)
                                     .ToPagedListAsync(pageNumber, currentPageSize);

        filter.AttributeOptions = await LoadAttributeSelectListAsync(filter.AttributeId);

        AttributeValueIndexViewModel viewModel = new()
        {
            AttributeValues = valuesPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/AttributeValue/Create
    public async Task<IActionResult> Create()
    {
        AttributeValueViewModel viewModel = new();
        await PopulateViewModelSelectListsAsync(viewModel);
        return View(viewModel);
    }

    // POST: Admin/AttributeValue/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AttributeValueViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            AttributeValue attributeValue = _mapper.Map<AttributeValue>(viewModel);

            _context.Add(attributeValue);

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { AttributeId = attributeValue.AttributeId }); // Redirect back to list filtered by parent
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating attribute value.");
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi lưu giá trị thuộc tính.");
            }
        }

        await PopulateViewModelSelectListsAsync(viewModel);
        return View(viewModel);
    }

    // GET: Admin/AttributeValue/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        AttributeValue? attributeValue = await _context.Set<AttributeValue>()
                                                     .AsNoTracking()
                                                     .FirstOrDefaultAsync(av => av.Id == id);

        if (attributeValue == null)
        {
            return NotFound();
        }

        AttributeValueViewModel viewModel = _mapper.Map<AttributeValueViewModel>(attributeValue);
        await PopulateViewModelSelectListsAsync(viewModel);
        return View(viewModel);
    }

    // POST: Admin/AttributeValue/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AttributeValueViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            AttributeValue? attributeValue = await _context.Set<AttributeValue>().FirstOrDefaultAsync(av => av.Id == id);

            if (attributeValue == null)
            {
                // Should not happen if loaded in GET, but good practice
                return RedirectToAction(nameof(Index));
            }

            _mapper.Map(viewModel, attributeValue);

            try
            {
                _context.Update(attributeValue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { AttributeId = attributeValue.AttributeId }); // Redirect back to list filtered by parent
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating attribute value with ID {AttributeValueId}.", id);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật giá trị thuộc tính.");
            }
        }

        await PopulateViewModelSelectListsAsync(viewModel);
        return View(viewModel);
    }

    // POST: Admin/AttributeValue/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        AttributeValue? attributeValue = await _context.Set<AttributeValue>().FirstOrDefaultAsync(av => av.Id == id);

        if (attributeValue == null)
        {
            return Json(new { success = false, message = "Không tìm thấy giá trị thuộc tính." });
        }

        try
        {
            int parentAttributeId = attributeValue.AttributeId;
            string attributeValueName = attributeValue.Value;
            _context.Remove(attributeValue);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = $"Xóa giá trị thuộc tính '{attributeValueName}' thành công.", redirectUrl = Url.Action(nameof(Index), new { AttributeId = parentAttributeId }) }); // Redirect after delete
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting attribute value with ID {AttributeValueId}.", id);
            // Check for FK issues
            if (ex is DbUpdateException dbEx && dbEx.InnerException?.Message.Contains("FOREIGN KEY constraint") == true)
            {
                return Json(new { success = false, message = "Không thể xóa giá trị thuộc tính này vì nó đang được sử dụng bởi các biến thể sản phẩm." });
            }
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa giá trị thuộc tính." });
        }
    }
}

public partial class AttributeValueController
{
    private async Task PopulateViewModelSelectListsAsync(AttributeValueViewModel viewModel)
    {
        viewModel.AttributeOptions = await LoadAttributeSelectListAsync(viewModel.AttributeId);
    }

    private async Task<List<SelectListItem>> LoadAttributeSelectListAsync(int? selectedValue = null)
    {
        var attributes = await _context.Set<domain.Entities.Attribute>()
                         .OrderBy(a => a.Name)
                         .AsNoTracking()
                         .Select(a => new { a.Id, a.Name })
                         .ToListAsync();

        var items = new List<SelectListItem>
        {
             new SelectListItem { Value = "", Text = "-- Chọn thuộc tính cha --", Selected = !selectedValue.HasValue }
        };

        items.AddRange(attributes.Select(a => new SelectListItem
        {
            Value = a.Id.ToString(),
            Text = a.Name,
            Selected = selectedValue.HasValue && a.Id == selectedValue.Value
        }));

        return items;
    }
}