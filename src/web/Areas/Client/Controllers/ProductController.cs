using AutoMapper;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using shared.Enums;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Client.Models.Product;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("san-pham")]
public partial class ProductController(
    ApplicationDbContext context,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache);

public partial class ProductController
{
    [HttpGet]
    public async Task<IActionResult> Index(
          string? search = null,
          int[]? categoryIds = null,
          int[]? tagIds = null,
          int[]? productTypeIds = null,
          decimal? minPrice = null,
          decimal? maxPrice = null,
          string sortBy = "newest",
          string? status = null,
          string[]? fieldIds = null,
          string[]? fieldValues = null,
          int page = 1,
          int pageSize = 12)
    {
        // Base query for products
        var query = context.Products
            .Include(p => p.ProductType)
            .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
            .Include(p => p.ProductTags)
                .ThenInclude(pt => pt.Tag)
            .Include(p => p.FieldValues)
                .ThenInclude(fv => fv.Field)
            .Include(p => p.Images)
            .AsQueryable();

        // Apply status filter (default to Published for public view)
        PublishStatus? statusEnum = null;
        if (!string.IsNullOrEmpty(status) && Enum.TryParse<PublishStatus>(status, true, out var parsedStatus))
        {
            statusEnum = parsedStatus;
            query = query.Where(p => p.Status == parsedStatus);
        }
        else
        {
            // Default to published for public view
            query = query.Where(p => p.Status == PublishStatus.Published);
        }

        // Apply search filter
        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(search) ||
                                    p.Description.ToLower().Contains(search) ||
                                    p.Sku.ToLower().Contains(search));
        }

        // Apply category filter
        if (categoryIds != null && categoryIds.Length > 0)
        {
            query = query.Where(p => p.ProductCategories != null &&
                                    p.ProductCategories.Any(pc => categoryIds.Contains(pc.CategoryId)));
        }

        // Apply tag filter
        if (tagIds != null && tagIds.Length > 0)
        {
            query = query.Where(p => p.ProductTags != null &&
                                    p.ProductTags.Any(pt => tagIds.Contains(pt.TagId)));
        }

        // Apply product type filter
        if (productTypeIds != null && productTypeIds.Length > 0)
        {
            query = query.Where(p => productTypeIds.Contains(p.ProductTypeId));
        }

        // Apply price range filter
        if (minPrice.HasValue)
        {
            query = query.Where(p => p.BasePrice >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.BasePrice <= maxPrice.Value);
        }

        if (fieldIds != null && fieldIds.Length > 0 &&
            fieldValues != null && fieldValues.Length > 0 &&
            fieldIds.Length == fieldValues.Length)
        {
            for (int i = 0; i < fieldIds.Length; i++)
            {
                if (int.TryParse(fieldIds[i], out int fieldId) && !string.IsNullOrEmpty(fieldValues[i]))
                {
                    string fieldValue = fieldValues[i];

                    query = query.Where(p => p.FieldValues != null &&
                                          p.FieldValues.Any(fv =>
                                              fv.FieldId == fieldId &&
                                              fv.Value == fieldValue));
                }
            }
        }

        // Apply sorting
        query = sortBy switch
        {
            "name_asc" => query.OrderBy(p => p.Name),
            "name_desc" => query.OrderByDescending(p => p.Name),
            "price_asc" => query.OrderBy(p => p.BasePrice),
            "price_desc" => query.OrderByDescending(p => p.BasePrice),
            "oldest" => query.OrderBy(p => p.CreatedAt),
            "newest" => query.OrderByDescending(p => p.CreatedAt),
            _ => query.OrderByDescending(p => p.CreatedAt)
        };

        // Count total products for pagination
        var totalProducts = await query.CountAsync();

        // Calculate total pages
        var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

        // Ensure page is within valid range
        page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

        // Get paginated products
        var products = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Get all categories for products
        var allCategories = await context.Categories
            .Where(cat => cat.EntityType == EntityType.Product)
            .OrderBy(cat => cat.Name)
            .ToListAsync();

        // Separate parent and child categories
        var parentCategories = allCategories
            .Where(c => c.ParentCategoryId == null)
            .ToList();

        var childCategories = allCategories
            .Where(c => c.ParentCategoryId != null)
            .ToList();

        // Get all tags for products
        var tags = await context.Tags
            .Where(t => t.EntityType == EntityType.Product)
            .OrderBy(t => t.Name)
            .ToListAsync();

        // Get all product types
        var productTypes = await context.ProductTypes
            .OrderBy(pt => pt.Name)
            .ToListAsync();

        // Get all field definitions
        var fieldDefinitions = await context.ProductFieldDefinitions
            .OrderBy(fd => fd.ProductTypeId)
            .ThenBy(fd => fd.FieldName)
            .ToListAsync();

        // Get available field values
        var availableFieldValues = new Dictionary<int, List<string>>();
        foreach (var field in fieldDefinitions)
        {
            var values = await context.ProductFieldValues
                .Where(fv => fv.FieldId == field.Id)
                .Select(fv => fv.Value)
                .Distinct()
                .OrderBy(v => v)
                .ToListAsync();

            availableFieldValues[field.Id] = values;
        }

        // Get price range
        var minAvailablePrice = await context.Products
            .Where(p => p.Status == PublishStatus.Published)
            .Select(p => (decimal?)p.BasePrice)
            .MinAsync() ?? 0;

        var maxAvailablePrice = await context.Products
            .Where(p => p.Status == PublishStatus.Published)
            .Select(p => (decimal?)p.BasePrice)
            .MaxAsync() ?? 0;

        // Parse field values from request
        var fieldValuesDictionary = new Dictionary<int, List<string>>();
        if (fieldIds != null && fieldValues != null &&
            fieldIds.Length == fieldValues.Length)
        {
            for (int i = 0; i < fieldIds.Length; i++)
            {
                if (int.TryParse(fieldIds[i], out int fieldId) && !string.IsNullOrEmpty(fieldValues[i]))
                {
                    if (!fieldValuesDictionary.ContainsKey(fieldId))
                    {
                        fieldValuesDictionary[fieldId] = new List<string>();
                    }

                    fieldValuesDictionary[fieldId].Add(fieldValues[i]);
                }
            }
        }

        // Create view model
        var model = new ProductViewModel
        {
            Products = products,
            Categories = allCategories,
            ParentCategories = parentCategories,
            ChildCategories = childCategories,
            Tags = tags,
            ProductTypes = productTypes,
            FieldDefinitions = fieldDefinitions,
            AvailableFieldValues = availableFieldValues,
            SelectedCategoryIds = categoryIds?.ToList() ?? new List<int>(),
            SelectedTagIds = tagIds?.ToList() ?? new List<int>(),
            SelectedProductTypeIds = productTypeIds?.ToList() ?? new List<int>(),
            FieldValues = fieldValuesDictionary,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            MinAvailablePrice = minAvailablePrice,
            MaxAvailablePrice = maxAvailablePrice,
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = totalPages,
            TotalProducts = totalProducts,
            Search = search ?? "",
            SortBy = sortBy,
            Status = statusEnum,
        };

        return View(model);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Detail(int id)
    {
        var product = await context.Products
            .Include(p => p.ProductType)
            .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
            .Include(p => p.ProductTags)
                .ThenInclude(pt => pt.Tag)
            .Include(p => p.FieldValues)
                .ThenInclude(fv => fv.Field)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id && p.Status == PublishStatus.Published);

        if (product == null)
        {
            return NotFound();
        }

        var relatedProducts = new List<Product>();

        if (product.ProductCategories != null && product.ProductCategories.Any())
        {
            var categoryIds = product.ProductCategories.Select(pc => pc.CategoryId).ToList();

            relatedProducts = await context.Products
                .Where(p => p.Id != product.Id &&
                           p.Status == PublishStatus.Published &&
                           p.ProductCategories != null &&
                           p.ProductCategories.Any(pc => categoryIds.Contains(pc.CategoryId)))
                .Include(p => p.ProductType)
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .OrderByDescending(p => p.CreatedAt)
                .Take(4)
                .ToListAsync();
        }

        ViewBag.RelatedProducts = relatedProducts;

        return View(product);
    }
}