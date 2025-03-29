// ProductController.cs
using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Admin.ViewModels.Category;
using web.Areas.Admin.ViewModels.Product;
using web.Areas.Admin.ViewModels.ProductType;
using web.Areas.Admin.ViewModels.Tag;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<ProductViewModel> _validator;
    private readonly IWebHostEnvironment _environment;

    public ProductController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<ProductViewModel> validator,
        IWebHostEnvironment environment)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
        _environment = environment;
    }

    // GET: Admin/Product
    public async Task<IActionResult> Index(
        string? searchTerm = null,
        int? productTypeId = null,
        PublishStatus? status = null,
        bool? isFeatured = null)
    {
        ViewData["PageTitle"] = "Quản lý sản phẩm";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Sản phẩm", "")
        };

        var query = _context.Set<Product>()
            .Include(p => p.ProductType)
            .Include(p => p.ProductCategories)
            .Include(p => p.ProductTags)
            .Include(p => p.Images)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p => p.Name.Contains(searchTerm) ||
                                    p.ShortDescription.Contains(searchTerm) ||
                                    p.Description.Contains(searchTerm));
        }

        if (productTypeId.HasValue && productTypeId.Value > 0)
        {
            query = query.Where(p => p.ProductTypeId == productTypeId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(p => p.Status == status.Value);
        }

        if (isFeatured.HasValue)
        {
            query = query.Where(p => p.IsFeatured == isFeatured.Value);
        }

        var products = await query
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        var viewModels = _mapper.Map<List<ProductListItemViewModel>>(products);

        // Get product types for filter dropdown
        ViewBag.ProductTypes = await _context.Set<ProductType>()
             .Where(pt => pt.IsActive)
             .OrderBy(pt => pt.Name)
             .Select(pt => new SelectListItem
             {
                 Value = pt.Id.ToString(),
                 Text = pt.Name
             })
             .ToListAsync();

        ViewBag.StatusList = Enum.GetValues(typeof(PublishStatus))
            .Cast<PublishStatus>()
            .Select(s => new SelectListItem
            {
                Value = ((int)s).ToString(),
                Text = s.ToString()
            })
            .ToList();

        ViewBag.SelectedProductTypeId = productTypeId;
        ViewBag.SelectedStatus = status;
        ViewBag.SelectedIsFeatured = isFeatured;
        ViewBag.SearchTerm = searchTerm;
        return View(viewModels);
    }

    // GET: Admin/Product/Create
    public async Task<IActionResult> Create()
    {
        ViewData["PageTitle"] = "Thêm sản phẩm mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Sản phẩm", "/Admin/Product"),
            ("Thêm mới", "")
        };

        var viewModel = new ProductViewModel
        {
            IsActive = true,
            Status = PublishStatus.Draft
        };

        await LoadRelatedData(viewModel);

        return View(viewModel);
    }

    // POST: Admin/Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductViewModel viewModel)
    {
        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            await LoadRelatedData(viewModel);
            return View(viewModel);
        }

        // Check if slug is unique
        if (await _context.Set<Product>().AnyAsync(p => p.Slug == viewModel.Slug))
        {
            ModelState.AddModelError("Slug", "Slug đã tồn tại, vui lòng chọn slug khác");
            await LoadRelatedData(viewModel);
            return View(viewModel);
        }

        var product = _mapper.Map<Product>(viewModel);

        _context.Add(product);
        await _context.SaveChangesAsync();

        // Process categories
        if (viewModel.CategoryIds != null && viewModel.CategoryIds.Any())
        {
            foreach (var categoryId in viewModel.CategoryIds)
            {
                _context.Add(new ProductCategory
                {
                    ProductId = product.Id,
                    CategoryId = categoryId
                });
            }
        }

        // Process tags
        if (viewModel.TagIds != null && viewModel.TagIds.Any())
        {
            foreach (var tagId in viewModel.TagIds)
            {
                _context.Add(new ProductTag
                {
                    ProductId = product.Id,
                    TagId = tagId
                });
            }
        }

        // Process images
        if (viewModel.ImageFiles != null && viewModel.ImageFiles.Any())
        {
            var orderIndex = 0;
            foreach (var imageFile in viewModel.ImageFiles)
            {
                var imageUrl = await SaveImage(imageFile);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    _context.Add(new ProductImage
                    {
                        ProductId = product.Id,
                        ImageUrl = imageUrl,
                        OrderIndex = orderIndex,
                        IsMain = orderIndex == 0 // First image is main
                    });
                    orderIndex++;
                }
            }
        }

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Thêm sản phẩm thành công";
        return RedirectToAction(nameof(Index));
    }

    // GET: Admin/Product/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _context.Set<Product>()
            .Include(p => p.ProductCategories)
            .Include(p => p.ProductTags)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        ViewData["PageTitle"] = "Chỉnh sửa sản phẩm";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Sản phẩm", "/Admin/Product"),
            ("Chỉnh sửa", "")
        };

        var viewModel = _mapper.Map<ProductViewModel>(product);

        await LoadRelatedData(viewModel);

        return View(viewModel);
    }

    // POST: Admin/Product/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            await LoadRelatedData(viewModel);
            return View(viewModel);
        }

        // Check if slug is unique (excluding current product)
        if (await _context.Set<Product>().AnyAsync(p => p.Slug == viewModel.Slug && p.Id != id))
        {
            ModelState.AddModelError("Slug", "Slug đã tồn tại, vui lòng chọn slug khác");
            await LoadRelatedData(viewModel);
            return View(viewModel);
        }

        try
        {
            var product = await _context.Set<Product>()
                .Include(p => p.ProductCategories)
                .Include(p => p.ProductTags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, product);

            // Update categories
            var existingCategories = product.ProductCategories?.ToList() ?? new List<ProductCategory>();
            var categoriesToAdd = viewModel.CategoryIds
                .Where(categoryId => !existingCategories.Any(pc => pc.CategoryId == categoryId))
                .Select(categoryId => new ProductCategory { ProductId = id, CategoryId = categoryId })
                .ToList();

            var categoriesToRemove = existingCategories
                .Where(pc => !viewModel.CategoryIds.Contains(pc.CategoryId))
                .ToList();

            foreach (var category in categoriesToRemove)
            {
                _context.Remove(category);
            }

            foreach (var category in categoriesToAdd)
            {
                _context.Add(category);
            }

            // Update tags
            var existingTags = product.ProductTags?.ToList() ?? new List<ProductTag>();
            var tagsToAdd = viewModel.TagIds
                .Where(tagId => !existingTags.Any(pt => pt.TagId == tagId))
                .Select(tagId => new ProductTag { ProductId = id, TagId = tagId })
                .ToList();

            var tagsToRemove = existingTags
                .Where(pt => !viewModel.TagIds.Contains(pt.TagId))
                .ToList();

            foreach (var tag in tagsToRemove)
            {
                _context.Remove(tag);
            }

            foreach (var tag in tagsToAdd)
            {
                _context.Add(tag);
            }

            // Process new images
            if (viewModel.ImageFiles != null && viewModel.ImageFiles.Any())
            {
                var maxOrderIndex = await _context.Set<ProductImage>()
                    .Where(pi => pi.ProductId == id)
                    .Select(pi => (int?)pi.OrderIndex)
                    .MaxAsync() ?? -1;

                foreach (var imageFile in viewModel.ImageFiles)
                {
                    var imageUrl = await SaveImage(imageFile);
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        maxOrderIndex++;
                        _context.Add(new ProductImage
                        {
                            ProductId = id,
                            ImageUrl = imageUrl,
                            OrderIndex = maxOrderIndex,
                            IsMain = false // New images are not main by default
                        });
                    }
                }
            }

            _context.Update(product);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ProductExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: Admin/Product/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Set<Product>()
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
        }

        // Delete product images from file system
        if (product.Images != null)
        {
            foreach (var image in product.Images)
            {
                DeleteImage(image.ImageUrl);
                if (!string.IsNullOrEmpty(image.ThumbnailUrl))
                {
                    DeleteImage(image.ThumbnailUrl);
                }
            }
        }

        _context.Set<Product>().Remove(product);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Xóa sản phẩm thành công" });
    }

    // POST: Admin/Product/ToggleActive/5
    [HttpPost]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var product = await _context.Set<Product>().FindAsync(id);

        if (product == null)
        {
            return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
        }

        product.IsActive = !product.IsActive;
        await _context.SaveChangesAsync();

        return Json(new { success = true, active = product.IsActive });
    }

    // POST: Admin/Product/ToggleFeatured/5
    [HttpPost]
    public async Task<IActionResult> ToggleFeatured(int id)
    {
        var product = await _context.Set<Product>().FindAsync(id);

        if (product == null)
        {
            return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
        }

        product.IsFeatured = !product.IsFeatured;
        await _context.SaveChangesAsync();

        return Json(new { success = true, featured = product.IsFeatured });
    }

    // POST: Admin/Product/UpdateStatus/5
    [HttpPost]
    public async Task<IActionResult> UpdateStatus(int id, PublishStatus status)
    {
        var product = await _context.Set<Product>().FindAsync(id);

        if (product == null)
        {
            return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
        }

        product.Status = status;
        await _context.SaveChangesAsync();

        return Json(new { success = true, status = status.ToString() });
    }

    // POST: Admin/Product/SetMainImage/5
    [HttpPost]
    public async Task<IActionResult> SetMainImage(int id, int productId)
    {
        var image = await _context.Set<ProductImage>().FindAsync(id);
        if (image == null || image.ProductId != productId)
        {
            return Json(new { success = false, message = "Không tìm thấy hình ảnh" });
        }

        // Reset all images for this product
        var productImages = await _context.Set<ProductImage>()
            .Where(pi => pi.ProductId == productId)
            .ToListAsync();

        foreach (var img in productImages)
        {
            img.IsMain = false;
        }

        // Set the selected image as main
        image.IsMain = true;
        await _context.SaveChangesAsync();

        return Json(new { success = true });
    }

    // POST: Admin/Product/DeleteImage/5
    [HttpPost]
    public async Task<IActionResult> DeleteImage(int id)
    {
        var image = await _context.Set<ProductImage>().FindAsync(id);
        if (image == null)
        {
            return Json(new { success = false, message = "Không tìm thấy hình ảnh" });
        }

        // Delete image file
        DeleteImage(image.ImageUrl);
        if (!string.IsNullOrEmpty(image.ThumbnailUrl))
        {
            DeleteImage(image.ThumbnailUrl);
        }

        _context.Set<ProductImage>().Remove(image);
        await _context.SaveChangesAsync();

        // If this was the main image, set another image as main
        if (image.IsMain)
        {
            var firstImage = await _context.Set<ProductImage>()
                .Where(pi => pi.ProductId == image.ProductId)
                .OrderBy(pi => pi.OrderIndex)
                .FirstOrDefaultAsync();

            if (firstImage != null)
            {
                firstImage.IsMain = true;
                await _context.SaveChangesAsync();
            }
        }

        return Json(new { success = true });
    }

    // POST: Admin/Product/ReorderImages
    [HttpPost]
    public async Task<IActionResult> ReorderImages(int productId, List<int> imageIds)
    {
        if (imageIds == null || !imageIds.Any())
        {
            return Json(new { success = false, message = "Không có hình ảnh để sắp xếp" });
        }

        var images = await _context.Set<ProductImage>()
            .Where(pi => pi.ProductId == productId && imageIds.Contains(pi.Id))
            .ToListAsync();

        for (int i = 0; i < imageIds.Count; i++)
        {
            var image = images.FirstOrDefault(img => img.Id == imageIds[i]);
            if (image != null)
            {
                image.OrderIndex = i;
            }
        }

        await _context.SaveChangesAsync();
        return Json(new { success = true });
    }

    private async Task<bool> ProductExists(int id)
    {
        return await _context.Set<Product>().AnyAsync(e => e.Id == id);
    }

    private async Task LoadRelatedData(ProductViewModel viewModel)
    {
        // Load product types
        viewModel.ProductTypes = await _context.Set<ProductType>()
            .Where(pt => pt.IsActive)
            .OrderBy(pt => pt.Name)
            .Select(pt => new ProductTypeViewModel
            {
                Id = pt.Id,
                Name = pt.Name
            })
            .ToListAsync();

        // Load categories
        viewModel.Categories = await _context.Set<Category>()
            .Where(c => c.Type == CategoryType.Product && c.IsActive)
            .OrderBy(c => c.Name)
            .Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            })
            .ToListAsync();

        // Load tags
        viewModel.Tags = await _context.Set<Tag>()
            .Where(t => t.Type == TagType.Product)
            .OrderBy(t => t.Name)
            .Select(t => new TagViewModel
            {
                Id = t.Id,
                Name = t.Name
            })
            .ToListAsync();

        // Load images if editing
        if (viewModel.Id > 0)
        {
            viewModel.Images = await _context.Set<ProductImage>()
                .Where(pi => pi.ProductId == viewModel.Id)
                .OrderBy(pi => pi.OrderIndex)
                .Select(pi => new ProductImageViewModel
                {
                    Id = pi.Id,
                    ProductId = pi.ProductId,
                    ImageUrl = pi.ImageUrl,
                    ThumbnailUrl = pi.ThumbnailUrl,
                    AltText = pi.AltText,
                    Title = pi.Title,
                    OrderIndex = pi.OrderIndex,
                    IsMain = pi.IsMain
                })
                .ToListAsync();
        }
    }

    private async Task<string> SaveImage(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            // Create uploads directory if it doesn't exist
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "products");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate unique filename
            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/uploads/products/{uniqueFileName}";
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    private void DeleteImage(string imageUrl)
    {
        try
        {
            if (string.IsNullOrEmpty(imageUrl))
                return;

            var filePath = Path.Combine(_environment.WebRootPath, imageUrl.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
        catch (Exception)
        {
            // Log error but continue
        }
    }
}