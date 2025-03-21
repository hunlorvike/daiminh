using AutoMapper;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using shared.Attributes;
using shared.Constants;
using shared.Extensions;
using shared.Models;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Requests.Product;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}", AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public class ProductController : DaiminhController
{
    private readonly ApplicationDbContext _dbContext;

    public ProductController(
        ApplicationDbContext dbContext,
        IMapper mapper,
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        IDistributedCache cache)
        : base(mapper, serviceProvider, configuration, cache)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _dbContext.Products
            .AsNoTracking()
            .Where(p => p.DeletedAt == null)
            .Include(p => p.ProductType)
            .Include(p => p.Images.Where(i => i.IsPrimary))
            .ToListAsync();

        return View(products);
    }

    [AjaxOnly]
    public async Task<IActionResult> Create()
    {
        await PopulateDropdowns();
        return PartialView("_Create.Modal", new ProductCreateRequest());
    }

    [AjaxOnly]
    public async Task<IActionResult> GetFieldDefinitions(int productTypeId)
    {
        var fields = await _dbContext.ProductFieldDefinitions
            .AsNoTracking()
            .Where(f => f.ProductTypeId == productTypeId && f.DeletedAt == null)
            .ToListAsync();

        return Json(fields);
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _dbContext.Products
            .AsNoTracking()
            .Include(p => p.ProductType)
            .Include(p => p.ProductCategories)
            .Include(p => p.ProductTags)
            .Include(p => p.FieldValues)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == null) ?? throw new NotFoundException("Product not found.");

        await PopulateDropdowns(product);
        var request = _mapper.Map<ProductUpdateRequest>(product);
        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == null) ?? throw new NotFoundException("Product not found.");

        var request = _mapper.Map<ProductDeleteRequest>(product);
        return PartialView("_Delete.Modal", request);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCreateRequest model)
    {
        var validator = GetValidator<ProductCreateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null)
        {
            await PopulateDropdowns();
            return result;
        }

        try
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var newProduct = _mapper.Map<Product>(model);

            // Add categories
            if (model.CategoryIds != null && model.CategoryIds.Any())
            {
                newProduct.ProductCategories = model.CategoryIds.Select(categoryId => new ProductCategory
                {
                    CategoryId = categoryId
                }).ToList();
            }

            // Add tags
            if (model.TagIds != null && model.TagIds.Any())
            {
                newProduct.ProductTags = model.TagIds.Select(tagId => new ProductTag
                {
                    TagId = tagId
                }).ToList();
            }

            // Add field values
            if (model.FieldValues != null && model.FieldValues.Any())
            {
                newProduct.FieldValues = model.FieldValues.Select(fv => new ProductFieldValue
                {
                    FieldId = fv.FieldId,
                    Value = fv.Value
                }).ToList();
            }

            // Add images
            if (model.Images != null && model.Images.Any())
            {
                newProduct.Images = model.Images.Select((img, index) => new ProductImage
                {
                    ImageUrl = img.ImageUrl,
                    AltText = img.AltText,
                    IsPrimary = img.IsPrimary,
                    DisplayOrder = (short)(img.DisplayOrder > 0 ? img.DisplayOrder : index)
                }).ToList();
            }

            await _dbContext.Products.AddAsync(newProduct);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            var successResponse = new SuccessResponse<Product>(newProduct, "Thêm sản phẩm mới thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "Product", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "Product", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            throw new SystemException2("Error creating product.", ex);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductUpdateRequest model)
    {
        var validator = GetValidator<ProductUpdateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null)
        {
            var product = await _dbContext.Products
                .AsNoTracking()
                .Include(p => p.ProductType)
                .Include(p => p.ProductCategories)
                .Include(p => p.ProductTags)
                .Include(p => p.FieldValues)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == model.Id && p.DeletedAt == null);

            await PopulateDropdowns(product);
            return result;
        }

        try
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var existingProduct = await _dbContext.Products
                .Include(p => p.ProductCategories)
                .Include(p => p.ProductTags)
                .Include(p => p.FieldValues)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == model.Id && p.DeletedAt == null);

            if (existingProduct == null) throw new NotFoundException("Product not found.");

            // Update product
            _mapper.Map(model, existingProduct);

            // Update categories
            _dbContext.ProductCategories.RemoveRange(existingProduct.ProductCategories ?? Enumerable.Empty<ProductCategory>());
            if (model.CategoryIds != null && model.CategoryIds.Any())
            {
                existingProduct.ProductCategories = model.CategoryIds.Select(categoryId => new ProductCategory
                {
                    ProductId = model.Id,
                    CategoryId = categoryId
                }).ToList();
            }

            // Update tags
            _dbContext.ProductTags.RemoveRange(existingProduct.ProductTags ?? Enumerable.Empty<ProductTag>());
            if (model.TagIds != null && model.TagIds.Any())
            {
                existingProduct.ProductTags = model.TagIds.Select(tagId => new ProductTag
                {
                    ProductId = model.Id,
                    TagId = tagId
                }).ToList();
            }

            // Update field values
            _dbContext.ProductFieldValues.RemoveRange(existingProduct.FieldValues ?? Enumerable.Empty<ProductFieldValue>());
            if (model.FieldValues != null && model.FieldValues.Any())
            {
                existingProduct.FieldValues = model.FieldValues.Select(fv => new ProductFieldValue
                {
                    ProductId = model.Id,
                    FieldId = fv.FieldId,
                    Value = fv.Value
                }).ToList();
            }

            // Update images
            _dbContext.ProductImages.RemoveRange(existingProduct.Images ?? Enumerable.Empty<ProductImage>());
            if (model.Images != null && model.Images.Any())
            {
                existingProduct.Images = model.Images.Select((img, index) => new ProductImage
                {
                    ProductId = model.Id,
                    ImageUrl = img.ImageUrl,
                    AltText = img.AltText,
                    IsPrimary = img.IsPrimary,
                    DisplayOrder = (short)(img.DisplayOrder > 0 ? img.DisplayOrder : index)
                }).ToList();
            }

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            var successResponse = new SuccessResponse<Product>(existingProduct, "Cập nhật sản phẩm thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "Product", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "Product", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            throw new SystemException2("Error updating product.", ex);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(ProductDeleteRequest model)
    {
        try
        {
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == model.Id && p.DeletedAt == null);

            _dbContext.Products.Remove(product!);
            await _dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<Product>(product!, "Xóa sản phẩm thành công (đã ẩn).");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "Product", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "Product", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            throw new SystemException2("Error deleting product.", ex);
        }
    }

    private async Task PopulateDropdowns(Product? product = null)
    {
        var productTypes = await _dbContext.ProductTypes
            .AsNoTracking()
            .Where(pt => pt.DeletedAt == null)
            .ToListAsync();
        ViewBag.ProductTypes = new SelectList(productTypes, "Id", "Name", product?.ProductTypeId);

        var tags = await _dbContext.Tags
            .AsNoTracking()
            .Where(t => t.DeletedAt == null)
            .ToListAsync();
        ViewBag.Tags = new MultiSelectList(tags, "Id", "Name", product?.ProductTags?.Select(pt => pt.TagId).ToList());

        var categories = await _dbContext.Categories
            .AsNoTracking()
            .Where(c => c.DeletedAt == null && c.EntityType == shared.Enums.EntityType.Product)
            .ToListAsync();
        ViewBag.Categories = new MultiSelectList(categories, "Id", "Name", product?.ProductCategories?.Select(pc => pc.CategoryId).ToList());
    }
}