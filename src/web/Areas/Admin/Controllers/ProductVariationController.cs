using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using FluentValidation;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.ProductVariation;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public partial class ProductVariationController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductVariationController> _logger;

    public ProductVariationController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<ProductVariationController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/ProductVariation/Index/{productId}
    // Lists variations for a specific product
    [Route("Admin/ProductVariation/Index/{productId}")]
    public async Task<IActionResult> Index(int productId, ProductVariationFilterViewModel filter, int page = 1, int pageSize = 25)
    {
        var product = await _context.Set<Product>().AsNoTracking().FirstOrDefaultAsync(p => p.Id == productId);
        if (product == null)
        {
            return NotFound("Product not found."); // Variations must belong to a product
        }

        filter ??= new ProductVariationFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 25;

        IQueryable<ProductVariation> query = _context.Set<ProductVariation>()
                                             .Where(pv => pv.ProductId == productId)
                                             .Include(pv => pv.Product) // Include parent product
                                                                        // Crucially, include the relationships needed for the AttributeCombinationResolver
                                             .Include(pv => pv.ProductVariationAttributeValues)
                                                 .ThenInclude(pvav => pvav.AttributeValue)
                                                     .ThenInclude(av => av.Attribute) // Include Attribute from AttributeValue
                                             .AsNoTracking();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            // Searching variations by text attributes is hard without complex setup.
            // We'll ignore SearchTerm for now or implement later if needed via joins/full-text search.
            // As per the ViewModel, SearchTerm isn't tied to any specific filter logic yet.
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(pv => pv.IsActive == filter.IsActive.Value);
        }

        if (filter.IsDefault.HasValue)
        {
            query = query.Where(pv => pv.IsDefault == filter.IsDefault.Value);
        }

        if (filter.InStock.HasValue)
        {
            query = filter.InStock.Value ? query.Where(pv => pv.StockQuantity > 0) : query.Where(pv => pv.StockQuantity <= 0);
        }


        query = query.OrderBy(pv => pv.IsDefault == false) // Default variations first
                     .ThenByDescending(pv => pv.CreatedAt); // Then by creation date


        var variationsPaged = await query.ProjectTo<ProductVariationListItemViewModel>(_mapper.ConfigurationProvider)
                                         .ToPagedListAsync(pageNumber, currentPageSize);

        // Populate filter options
        filter.IsActiveOptions = GetYesNoSelectList(filter.IsActive, "Tất cả");
        filter.IsDefaultOptions = GetYesNoSelectList(filter.IsDefault, "Tất cả");
        filter.InStockOptions = GetInStockSelectList(filter.InStock); // Need a helper for InStock

        // Pass product info and variations to the view
        var viewModel = new ProductVariationIndexViewModel
        {
            ProductId = productId,
            ProductName = product.Name,
            Variations = variationsPaged,
            Filter = filter
        };


        return View(viewModel);
    }

    // GET: Admin/ProductVariation/Create/{productId}
    [Route("Admin/ProductVariation/Create/{productId}")]
    public async Task<IActionResult> Create(int productId)
    {
        var product = await _context.Set<Product>()
                                    .Include(p => p.ProductAttributes) // Need attributes linked to the product
                                        .ThenInclude(pa => pa.Attribute) // Need the Attribute details
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(p => p.Id == productId);

        if (product == null)
        {
            return NotFound("Product not found.");
        }

        // Check if product has attributes. If not, variations might not be needed/possible via attribute values.
        // Or maybe a single 'default' variation is always allowed regardless of attributes?
        // For this implementation, we assume variations are based on product attributes.
        if (product.ProductAttributes == null || !product.ProductAttributes.Any())
        {
            ModelState.AddModelError("", $"Sản phẩm '{product.Name}' chưa được gán thuộc tính nào. Vui lòng gán thuộc tính cho sản phẩm trước khi tạo biến thể.");
            // Redirect back to product edit? Or display error and keep on empty create page?
            // Redirecting seems better UX if variations MUST be based on attributes.
            // return RedirectToAction("Edit", "Product", new { id = productId }); // Need ProductController Edit action
            // For now, just display the error on the variation create page.
        }


        ProductVariationViewModel viewModel = new()
        {
            ProductId = productId,
            ProductName = product.Name,
            IsActive = true, // Default to active
            StockQuantity = 0 // Default stock
            // IsDefault should be false by default unless it's the very first variation
        };

        // Populate select lists (Attribute Values based on product attributes)
        await PopulateViewModelSelectListsAsync(viewModel, product.ProductAttributes?.Select(pa => pa.AttributeId).ToList());

        // Store parent product attributes in the ViewModel for the view to render attribute selectors
        viewModel.ParentProductAttributes = product.ProductAttributes?.Select(pa => pa.Attribute).ToList();


        return View(viewModel);
    }

    // POST: Admin/ProductVariation/Create/{productId}
    [Route("Admin/ProductVariation/Create/{productId}")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int productId, ProductVariationViewModel viewModel)
    {
        if (productId != viewModel.ProductId)
        {
            return BadRequest("Product ID mismatch.");
        }

        var product = await _context.Set<Product>()
                                    .Include(p => p.ProductAttributes)
                                       .ThenInclude(pa => pa.Attribute)
                                    .FirstOrDefaultAsync(p => p.Id == productId);

        if (product == null)
        {
            return NotFound("Product not found.");
        }

        // Need to manually add ProductAttributes to the ViewModel's validation context
        // so the validator can check attribute value selections against them.
        // This is a limitation/quirk when validation depends on related data not directly in the ViewModel.
        // Alternative: Load ProductAttributes inside the validator itself (current implementation does this).

        if (ModelState.IsValid)
        {
            ProductVariation variation = _mapper.Map<ProductVariation>(viewModel);
            variation.ProductId = productId; // Ensure ProductId is correct

            // Handle IsDefault logic: If this one is set to default, un-default others
            if (variation.IsDefault)
            {
                await ClearDefaultVariationsAsync(productId);
            }
            else
            {
                // If no variations exist or none is marked default, make the first one default
                var anyExistingVariations = await _context.Set<ProductVariation>().AnyAsync(pv => pv.ProductId == productId && pv.Id != viewModel.Id);
                if (!anyExistingVariations)
                {
                    variation.IsDefault = true; // Make the very first variation default
                }
            }


            // Handle ProductVariationAttributeValues relationship
            UpdateVariationAttributeValues(variation, viewModel.SelectedAttributeValueIds);

            _context.Add(variation);

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { productId = productId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product variation for product ID {ProductId}.", productId);
                // Check for potential unique constraint violation on AttributeValue combination
                if (ex is DbUpdateException dbEx && dbEx.InnerException?.Message.Contains("idx_product_variation_unique_attribute_combination") == true) // Assuming you add this unique index in EF config
                {
                    ModelState.AddModelError("", "Một biến thể với sự kết hợp các thuộc tính này đã tồn tại.");
                }
                else
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi lưu biến thể sản phẩm.");
                }
            }
        }

        // If validation fails, re-populate select lists and product attributes for the view
        viewModel.ProductName = product.Name; // Ensure product name is retained
        await PopulateViewModelSelectListsAsync(viewModel, product.ProductAttributes?.Select(pa => pa.AttributeId).ToList());
        viewModel.ParentProductAttributes = product.ProductAttributes?.Select(pa => pa.Attribute).ToList();

        return View(viewModel);
    }


    // GET: Admin/ProductVariation/Edit/5
    [Route("Admin/ProductVariation/Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        ProductVariation? variation = await _context.Set<ProductVariation>()
                                         .Include(pv => pv.Product) // Need parent product
                                         .Include(pv => pv.ProductVariationAttributeValues) // Need linked attribute values
                                         .AsNoTracking() // Use AsNoTracking for GET
                                         .FirstOrDefaultAsync(pv => pv.Id == id);

        if (variation == null)
        {
            return NotFound();
        }
        if (variation.Product == null)
        {
            // Should not happen with Include(pv => pv.Product) but defensive check
            return NotFound("Parent product not found for this variation.");
        }

        ProductVariationViewModel viewModel = _mapper.Map<ProductVariationViewModel>(variation);

        // Load attributes for the parent product to render attribute selectors
        var productAttributes = await _context.Set<ProductAttribute>()
                                              .Where(pa => pa.ProductId == variation.ProductId)
                                              .Include(pa => pa.Attribute)
                                              .Select(pa => pa.Attribute!) // Assuming Attribute is never null if ProductAttribute exists
                                              .AsNoTracking()
                                              .ToListAsync();

        viewModel.ParentProductAttributes = productAttributes;

        // Populate select lists (Attribute Values based on product attributes)
        await PopulateViewModelSelectListsAsync(viewModel, productAttributes.Select(a => a.Id).ToList());


        return View(viewModel);
    }

    // POST: Admin/ProductVariation/Edit/5
    [Route("Admin/ProductVariation/Edit/{id}")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductVariationViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            ProductVariation? variation = await _context.Set<ProductVariation>()
                                             .Include(pv => pv.ProductVariationAttributeValues) // Include relationships to update them
                                             .FirstOrDefaultAsync(pv => pv.Id == id);

            if (variation == null)
            {
                return RedirectToAction(nameof(Index), new { productId = viewModel.ProductId }); // Redirect back to product's variation list
            }

            // Get parent product for validation context and select lists if needed on error
            var product = await _context.Set<Product>()
                                        .Include(p => p.ProductAttributes)
                                           .ThenInclude(pa => pa.Attribute)
                                        .FirstOrDefaultAsync(p => p.Id == variation.ProductId); // Use variation's ProductId

            if (product == null)
            {
                return NotFound("Parent product not found for this variation."); // Data integrity issue
            }

            // Manually add ProductAttributes to the ViewModel's validation context
            // (Or ensure validator loads them - current validator loads them)

            // Handle IsDefault logic BEFORE mapping, based on incoming viewModel value
            bool shouldBecomeDefault = viewModel.IsDefault; // Store the desired state
            if (shouldBecomeDefault && !variation.IsDefault) // If it wasn't default but should be now
            {
                await ClearDefaultVariationsAsync(variation.ProductId, id); // Clear others, excluding this one
            }
            // Note: If viewModel.IsDefault is false, we don't clear others.
            // If the *last* default is unchecked, no variation will be default until one is manually set.
            // An alternative is to auto-assign a new default if the current default is unchecked.
            // Let's keep it simple for now: only clear others if this one is explicitly set to default.


            // Map basic properties from ViewModel to Entity
            _mapper.Map(viewModel, variation);

            // If it was already default and remained default, or if it was unset, re-apply the stored state
            // This ensures we don't accidentally re-default others if it was already default.
            variation.IsDefault = shouldBecomeDefault; // Apply the desired state from ViewModel


            // Handle ProductVariationAttributeValues relationship (add/remove links)
            UpdateVariationAttributeValues(variation, viewModel.SelectedAttributeValueIds);


            try
            {
                _context.Update(variation); // Mark as updated
                await _context.SaveChangesAsync();

                // After save, check if any variation is default. If not, set the first one found as default.
                var hasDefault = await _context.Set<ProductVariation>().AnyAsync(pv => pv.ProductId == variation.ProductId && pv.IsDefault);
                if (!hasDefault)
                {
                    var firstVariation = await _context.Set<ProductVariation>()
                                                      .Where(pv => pv.ProductId == variation.ProductId)
                                                      .OrderBy(pv => pv.Id) // Or some other consistent order
                                                      .FirstOrDefaultAsync();
                    if (firstVariation != null)
                    {
                        firstVariation.IsDefault = true;
                        _context.Update(firstVariation);
                        await _context.SaveChangesAsync(); // Save again
                    }
                }

                return RedirectToAction(nameof(Index), new { productId = variation.ProductId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product variation with ID {VariationId}.", id);
                if (ex is DbUpdateException dbEx && dbEx.InnerException?.Message.Contains("idx_product_variation_unique_attribute_combination") == true) // Assuming unique index name
                {
                    ModelState.AddModelError("", "Một biến thể với sự kết hợp các thuộc tính này đã tồn tại.");
                }
                else
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật biến thể sản phẩm.");
                }
            }

            // If validation fails or save fails, re-populate data for the view
            viewModel.ProductName = product.Name; // Ensure product name is retained
            var productAttributes = product.ProductAttributes?.Select(pa => pa.Attribute!).ToList() ?? new List<domain.Entities.Attribute>();
            viewModel.ParentProductAttributes = productAttributes;
            await PopulateViewModelSelectListsAsync(viewModel, productAttributes.Select(a => a.Id).ToList());
        }

        return View(viewModel);
    }

    // POST: Admin/ProductVariation/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        ProductVariation? variation = await _context.Set<ProductVariation>()
                                            .Include(pv => pv.Product) // To get ProductId and Name for redirect/message
                                            .FirstOrDefaultAsync(pv => pv.Id == id);

        if (variation == null)
        {
            return Json(new { success = false, message = "Không tìm thấy biến thể sản phẩm." });
        }

        int productId = variation.ProductId;
        string productName = variation.Product?.Name ?? "Sản phẩm không xác định"; // Get name before deleting

        try
        {
            bool wasDefault = variation.IsDefault;
            _context.Remove(variation);
            await _context.SaveChangesAsync();

            // If the deleted variation was the default one, find a new default
            if (wasDefault)
            {
                var firstVariation = await _context.Set<ProductVariation>()
                                                   .Where(pv => pv.ProductId == productId)
                                                   .OrderBy(pv => pv.Id) // Or some other consistent order
                                                   .FirstOrDefaultAsync();
                if (firstVariation != null)
                {
                    firstVariation.IsDefault = true;
                    _context.Update(firstVariation);
                    await _context.SaveChangesAsync(); // Save the new default
                }
            }

            return Json(new { success = true, message = $"Xóa biến thể thành công.", redirectUrl = Url.Action(nameof(Index), new { productId = productId }) });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product variation with ID {VariationId}.", id);
            // Check for FK issues if variations are linked to order items, etc. (Beyond current scope)
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa biến thể sản phẩm." });
        }
    }
}

public partial class ProductVariationController
{
    // Helper to populate AttributeValue select lists for the ViewModel
    // Only loads values for attributes specified in allowedAttributeIds
    private async Task PopulateViewModelSelectListsAsync(ProductVariationViewModel viewModel, List<int>? allowedAttributeIds)
    {
        if (allowedAttributeIds == null || !allowedAttributeIds.Any())
        {
            viewModel.AttributeValueOptionsByAttribute = new Dictionary<int, List<SelectListItem>>();
            return;
        }

        // Fetch all AttributeValues for the allowed attributes
        var attributeValues = await _context.Set<AttributeValue>()
                                            .Where(av => allowedAttributeIds.Contains(av.AttributeId))
                                            .AsNoTracking()
                                            .ToListAsync();

        viewModel.AttributeValueOptionsByAttribute = attributeValues
            .GroupBy(av => av.AttributeId)
            .ToDictionary(
                group => group.Key,
                group => group.Select(av => new SelectListItem
                {
                    Value = av.Id.ToString(),
                    Text = av.Value,
                    Selected = viewModel.SelectedAttributeValueIds != null && viewModel.SelectedAttributeValueIds.Contains(av.Id)
                }).ToList()
            );
    }

    // Helper to clear the IsDefault flag for all other variations of a product
    private async Task ClearDefaultVariationsAsync(int productId, int? excludeVariationId = null)
    {
        var query = _context.Set<ProductVariation>()
                            .Where(pv => pv.ProductId == productId && pv.IsDefault);

        if (excludeVariationId.HasValue)
        {
            query = query.Where(pv => pv.Id != excludeVariationId.Value);
        }

        await query.ForEachAsync(pv => pv.IsDefault = false);
        // Note: Changes are tracked but not saved here. SaveChangesAsync is called after this.
    }


    // Helper to generate SelectListItems for InStock filter
    private List<SelectListItem> GetInStockSelectList(bool? selectedValue)
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "Tất cả", Selected = !selectedValue.HasValue },
            new SelectListItem { Value = "true", Text = "Còn hàng", Selected = selectedValue == true },
            new SelectListItem { Value = "false", Text = "Hết hàng", Selected = selectedValue == false }
        };
    }

    // Helper to generate Yes/No SelectListItems (already exists in ArticleController, put in Shared?)
    private List<SelectListItem> GetYesNoSelectList(bool? selectedValue, string allText = "Tất cả")
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = allText, Selected = !selectedValue.HasValue },
            new SelectListItem { Value = "true", Text = "Có", Selected = selectedValue == true },
            new SelectListItem { Value = "false", Text = "Không", Selected = selectedValue == false }
        };
    }


    // Helper to update ProductVariationAttributeValues collection
    private void UpdateVariationAttributeValues(ProductVariation variation, List<int>? selectedAttributeValueIds)
    {
        var existingValueIds = variation.ProductVariationAttributeValues?.Select(pvav => pvav.AttributeValueId).ToList() ?? new List<int>();
        var valueIdsToAdd = selectedAttributeValueIds?.Except(existingValueIds).ToList() ?? new List<int>();
        var valueIdsToRemove = existingValueIds.Except(selectedAttributeValueIds ?? new List<int>()).ToList();

        // Remove relationships
        if (variation.ProductVariationAttributeValues != null) // Ensure collection exists
        {
            foreach (var valueId in valueIdsToRemove)
            {
                var pvav = variation.ProductVariationAttributeValues.First(pvav => pvav.AttributeValueId == valueId);
                _context.Remove(pvav); // Mark for deletion
            }
        }


        // Add new relationships
        variation.ProductVariationAttributeValues ??= new List<ProductVariationAttributeValue>(); // Initialize if null
        foreach (var valueId in valueIdsToAdd)
        {
            // Ensure the AttributeValue exists before creating the link (validation should handle this, but defensive)
            // Check if the link already exists (shouldn't if using Except/Contains, but defensive)
            if (!_context.Set<AttributeValue>().Any(av => av.Id == valueId) || variation.ProductVariationAttributeValues.Any(pvav => pvav.AttributeValueId == valueId))
            {
                _logger.LogWarning("Attempted to add invalid or duplicate AttributeValueId {AttributeValueId} to variation {VariationId}", valueId, variation.Id);
                continue; // Skip if invalid or already exists
            }
            variation.ProductVariationAttributeValues.Add(new ProductVariationAttributeValue { ProductVariationId = variation.Id, AttributeValueId = valueId });
        }

        // Re-sync the collection reference (optional but can help with complex scenarios)
        variation.ProductVariationAttributeValues = variation.ProductVariationAttributeValues.Where(pvav => !_context.Entry(pvav).State.HasFlag(EntityState.Deleted)).ToList();
    }
}