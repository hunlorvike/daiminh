// GalleryController.cs
using System.Text.RegularExpressions;
using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Admin.ViewModels.Article;
using web.Areas.Admin.ViewModels.Gallery;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
public class GalleryController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<GalleryViewModel> _validator;
    private readonly IValidator<GalleryImageViewModel> _imageValidator;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public GalleryController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<GalleryViewModel> validator,
        IValidator<GalleryImageViewModel> imageValidator,
        IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
        _imageValidator = imageValidator;
        _webHostEnvironment = webHostEnvironment;
    }

    // GET: Admin/Gallery
    public async Task<IActionResult> Index(GalleryFilterViewModel filter)
    {
        ViewData["PageTitle"] = "Quản lý thư viện ảnh";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Thư viện ảnh", "")
        };

        var query = _context.Set<Gallery>()
            .Include(g => g.Images)
            .Include(g => g.GalleryCategories)
                .ThenInclude(gc => gc.Category)
            .Include(g => g.GalleryTags)
                .ThenInclude(gt => gt.Tag)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            query = query.Where(g => g.Name.Contains(filter.SearchTerm) ||
                                    g.Description.Contains(filter.SearchTerm));
        }

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(g => g.GalleryCategories.Any(gc => gc.CategoryId == filter.CategoryId.Value));
        }

        if (filter.TagId.HasValue)
        {
            query = query.Where(g => g.GalleryTags.Any(gt => gt.TagId == filter.TagId.Value));
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(g => g.Status == filter.Status.Value);
        }

        if (filter.IsFeatured.HasValue)
        {
            query = query.Where(g => g.IsFeatured == filter.IsFeatured.Value);
        }

        if (filter.FromDate.HasValue)
        {
            query = query.Where(g => g.CreatedAt >= filter.FromDate.Value);
        }

        if (filter.ToDate.HasValue)
        {
            var toDate = filter.ToDate.Value.AddDays(1).AddSeconds(-1); // End of the day
            query = query.Where(g => g.CreatedAt <= toDate);
        }

        var galleries = await query
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync();

        var viewModels = _mapper.Map<List<GalleryListItemViewModel>>(galleries);

        // Populate categories, tags, and image count for each gallery
        foreach (var viewModel in viewModels)
        {
            var gallery = galleries.First(g => g.Id == viewModel.Id);

            viewModel.Categories = gallery.GalleryCategories?
                .Select(gc => gc.Category?.Name ?? "")
                .Where(name => !string.IsNullOrEmpty(name))
                .ToList() ?? new List<string>();

            viewModel.Tags = gallery.GalleryTags?
                .Select(gt => gt.Tag?.Name ?? "")
                .Where(name => !string.IsNullOrEmpty(name))
                .ToList() ?? new List<string>();

            viewModel.ImageCount = gallery.Images?.Count ?? 0;
        }

        // Get categories for dropdown
        ViewBag.Categories = await _context.Set<Category>()
            .OrderBy(c => c.Name)
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = filter.CategoryId.HasValue && c.Id == filter.CategoryId.Value
            })
            .ToListAsync();

        // Get tags for dropdown
        ViewBag.Tags = await _context.Set<Tag>()
            .Where(t => t.Type == TagType.Gallery)
            .OrderBy(t => t.Name)
            .Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name,
                Selected = filter.TagId.HasValue && t.Id == filter.TagId.Value
            })
            .ToListAsync();

        ViewBag.Filter = filter;

        return View(viewModels);
    }

    // GET: Admin/Gallery/Create
    public async Task<IActionResult> Create()
    {
        ViewData["PageTitle"] = "Thêm thư viện ảnh mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Thư viện ảnh", "/Admin/Gallery"),
            ("Thêm mới", "")
        };

        var viewModel = new GalleryViewModel
        {
            Status = PublishStatus.Draft
        };

        await PopulateRelatedData(viewModel);

        return View(viewModel);
    }

    // POST: Admin/Gallery/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GalleryViewModel viewModel)
    {
        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            await PopulateRelatedData(viewModel);
            return View(viewModel);
        }

        // Handle cover image upload
        if (viewModel.CoverImageFile != null)
        {
            viewModel.CoverImage = await UploadImage(viewModel.CoverImageFile, "galleries");
        }

        // Create gallery
        var gallery = _mapper.Map<Gallery>(viewModel);

        _context.Add(gallery);
        await _context.SaveChangesAsync();

        // Save categories
        if (viewModel.CategoryIds.Any())
        {
            var galleryCategories = viewModel.CategoryIds.Select(categoryId => new GalleryCategory
            {
                GalleryId = gallery.Id,
                CategoryId = categoryId
            });

            _context.AddRange(galleryCategories);
        }

        // Save tags
        if (viewModel.TagIds.Any())
        {
            var galleryTags = viewModel.TagIds.Select(tagId => new GalleryTag
            {
                GalleryId = gallery.Id,
                TagId = tagId
            });

            _context.AddRange(galleryTags);
        }

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Thêm thư viện ảnh thành công";
        return RedirectToAction(nameof(Edit), new { id = gallery.Id });
    }

    // GET: Admin/Gallery/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var gallery = await _context.Set<Gallery>()
            .Include(g => g.GalleryCategories)
            .Include(g => g.GalleryTags)
            .Include(g => g.Images)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (gallery == null)
        {
            return NotFound();
        }

        ViewData["PageTitle"] = "Chỉnh sửa thư viện ảnh";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Thư viện ảnh", "/Admin/Gallery"),
            ("Chỉnh sửa", "")
        };

        var viewModel = _mapper.Map<GalleryViewModel>(gallery);

        // Set related data
        viewModel.CategoryIds = gallery.GalleryCategories?.Select(gc => gc.CategoryId).ToList() ?? new List<int>();
        viewModel.TagIds = gallery.GalleryTags?.Select(gt => gt.TagId).ToList() ?? new List<int>();

        // Map gallery images
        if (gallery.Images != null)
        {
            viewModel.Images = _mapper.Map<List<GalleryImageViewModel>>(gallery.Images.OrderBy(i => i.OrderIndex));
        }

        await PopulateRelatedData(viewModel);

        return View(viewModel);
    }

    // POST: Admin/Gallery/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, GalleryViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            await PopulateRelatedData(viewModel);
            return View(viewModel);
        }

        try
        {
            var gallery = await _context.Set<Gallery>().FindAsync(id);

            if (gallery == null)
            {
                return NotFound();
            }

            // Handle cover image upload
            if (viewModel.CoverImageFile != null)
            {
                viewModel.CoverImage = await UploadImage(viewModel.CoverImageFile, "galleries");
            }

            // Update gallery
            _mapper.Map(viewModel, gallery);
            _context.Update(gallery);

            // Update categories
            var existingCategories = await _context.Set<GalleryCategory>()
                .Where(gc => gc.GalleryId == id)
                .ToListAsync();

            _context.RemoveRange(existingCategories);

            if (viewModel.CategoryIds.Any())
            {
                var galleryCategories = viewModel.CategoryIds.Select(categoryId => new GalleryCategory
                {
                    GalleryId = gallery.Id,
                    CategoryId = categoryId
                });

                _context.AddRange(galleryCategories);
            }

            // Update tags
            var existingTags = await _context.Set<GalleryTag>()
                .Where(gt => gt.GalleryId == id)
                .ToListAsync();

            _context.RemoveRange(existingTags);

            if (viewModel.TagIds.Any())
            {
                var galleryTags = viewModel.TagIds.Select(tagId => new GalleryTag
                {
                    GalleryId = gallery.Id,
                    TagId = tagId
                });

                _context.AddRange(galleryTags);
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật thư viện ảnh thành công";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await GalleryExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToAction(nameof(Edit), new { id });
    }

    // POST: Admin/Gallery/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var gallery = await _context.Set<Gallery>()
            .Include(g => g.Images)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (gallery == null)
        {
            return Json(new { success = false, message = "Không tìm thấy thư viện ảnh" });
        }

        try
        {
            // Delete related entities
            var galleryCategories = await _context.Set<GalleryCategory>()
                .Where(gc => gc.GalleryId == id)
                .ToListAsync();
            _context.RemoveRange(galleryCategories);

            var galleryTags = await _context.Set<GalleryTag>()
                .Where(gt => gt.GalleryId == id)
                .ToListAsync();
            _context.RemoveRange(galleryTags);

            // Delete images
            if (gallery.Images != null)
            {
                _context.RemoveRange(gallery.Images);
            }

            // Delete the gallery
            _context.Set<Gallery>().Remove(gallery);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Xóa thư viện ảnh thành công" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"Lỗi khi xóa thư viện ảnh: {ex.Message}" });
        }
    }

    // POST: Admin/Gallery/ToggleFeatured/5
    [HttpPost]
    public async Task<IActionResult> ToggleFeatured(int id)
    {
        var gallery = await _context.Set<Gallery>().FindAsync(id);

        if (gallery == null)
        {
            return Json(new { success = false, message = "Không tìm thấy thư viện ảnh" });
        }

        gallery.IsFeatured = !gallery.IsFeatured;
        await _context.SaveChangesAsync();

        return Json(new { success = true, featured = gallery.IsFeatured });
    }

    // POST: Admin/Gallery/UpdateStatus/5
    [HttpPost]
    public async Task<IActionResult> UpdateStatus(int id, PublishStatus status)
    {
        var gallery = await _context.Set<Gallery>().FindAsync(id);

        if (gallery == null)
        {
            return Json(new { success = false, message = "Không tìm thấy thư viện ảnh" });
        }

        gallery.Status = status;
        await _context.SaveChangesAsync();

        return Json(new { success = true, status = status.ToString() });
    }

    // POST: Admin/Gallery/GenerateSlug
    [HttpPost]
    public IActionResult GenerateSlug(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Json(new { slug = "" });
        }

        var slug = GenerateSlugFromName(name);
        return Json(new { slug });
    }

    // POST: Admin/Gallery/AddImage/5
    [HttpPost]
    public async Task<IActionResult> AddImage(int id, GalleryImageViewModel imageViewModel)
    {
        if (id != imageViewModel.GalleryId)
        {
            return Json(new { success = false, message = "ID thư viện không khớp" });
        }

        var validationResult = await _imageValidator.ValidateAsync(imageViewModel);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, message = string.Join(", ", errors) });
        }

        try
        {
            // Handle image upload
            if (imageViewModel.ImageFile != null)
            {
                imageViewModel.ImageUrl = await UploadImage(imageViewModel.ImageFile, "gallery-images");

                // Generate thumbnail if not provided
                if (string.IsNullOrEmpty(imageViewModel.ThumbnailUrl))
                {
                    // In a real app, you would generate a thumbnail here
                    // For now, we'll just use the same image
                    imageViewModel.ThumbnailUrl = imageViewModel.ImageUrl;
                }
            }

            // Get the max order index
            var maxOrderIndex = await _context.Set<GalleryImage>()
                .Where(gi => gi.GalleryId == id)
                .Select(gi => (int?)gi.OrderIndex)
                .MaxAsync() ?? -1;

            // Create gallery image
            var galleryImage = _mapper.Map<GalleryImage>(imageViewModel);
            galleryImage.OrderIndex = maxOrderIndex + 1;

            _context.Add(galleryImage);
            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = "Thêm ảnh thành công",
                image = new
                {
                    id = galleryImage.Id,
                    imageUrl = galleryImage.ImageUrl,
                    thumbnailUrl = galleryImage.ThumbnailUrl,
                    title = galleryImage.Title,
                    altText = galleryImage.AltText,
                    orderIndex = galleryImage.OrderIndex
                }
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"Lỗi khi thêm ảnh: {ex.Message}" });
        }
    }

    // POST: Admin/Gallery/UpdateImage/5
    [HttpPost]
    public async Task<IActionResult> UpdateImage(int id, GalleryImageViewModel imageViewModel)
    {
        if (id != imageViewModel.Id)
        {
            return Json(new { success = false, message = "ID ảnh không khớp" });
        }

        var validationResult = await _imageValidator.ValidateAsync(imageViewModel);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, message = string.Join(", ", errors) });
        }

        try
        {
            var galleryImage = await _context.Set<GalleryImage>().FindAsync(id);

            if (galleryImage == null)
            {
                return Json(new { success = false, message = "Không tìm thấy ảnh" });
            }

            // Handle image upload
            if (imageViewModel.ImageFile != null)
            {
                imageViewModel.ImageUrl = await UploadImage(imageViewModel.ImageFile, "gallery-images");

                // Generate thumbnail if not provided
                if (string.IsNullOrEmpty(imageViewModel.ThumbnailUrl))
                {
                    // In a real app, you would generate a thumbnail here
                    // For now, we'll just use the same image
                    imageViewModel.ThumbnailUrl = imageViewModel.ImageUrl;
                }
            }

            // Update gallery image
            _mapper.Map(imageViewModel, galleryImage);
            _context.Update(galleryImage);
            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = "Cập nhật ảnh thành công",
                image = new
                {
                    id = galleryImage.Id,
                    imageUrl = galleryImage.ImageUrl,
                    thumbnailUrl = galleryImage.ThumbnailUrl,
                    title = galleryImage.Title,
                    altText = galleryImage.AltText,
                    orderIndex = galleryImage.OrderIndex
                }
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"Lỗi khi cập nhật ảnh: {ex.Message}" });
        }
    }

    // POST: Admin/Gallery/DeleteImage/5
    [HttpPost]
    public async Task<IActionResult> DeleteImage(int id)
    {
        var galleryImage = await _context.Set<GalleryImage>().FindAsync(id);

        if (galleryImage == null)
        {
            return Json(new { success = false, message = "Không tìm thấy ảnh" });
        }

        try
        {
            _context.Set<GalleryImage>().Remove(galleryImage);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Xóa ảnh thành công" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"Lỗi khi xóa ảnh: {ex.Message}" });
        }
    }

    // POST: Admin/Gallery/ReorderImages
    [HttpPost]
    public async Task<IActionResult> ReorderImages([FromBody] List<GalleryImageOrderViewModel> images)
    {
        if (images == null || !images.Any())
        {
            return Json(new { success = false, message = "Không có ảnh để sắp xếp" });
        }

        try
        {
            foreach (var image in images)
            {
                var galleryImage = await _context.Set<GalleryImage>().FindAsync(image.Id);
                if (galleryImage != null)
                {
                    galleryImage.OrderIndex = image.OrderIndex;
                    _context.Update(galleryImage);
                }
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Sắp xếp ảnh thành công" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"Lỗi khi sắp xếp ảnh: {ex.Message}" });
        }
    }

    // Helper methods
    private async Task<bool> GalleryExists(int id)
    {
        return await _context.Set<Gallery>().AnyAsync(e => e.Id == id);
    }

    private async Task PopulateRelatedData(GalleryViewModel viewModel)
    {
        // Get categories
        var categories = await _context.Set<Category>()
            .OrderBy(c => c.Name)
            .ToListAsync();

        viewModel.AvailableCategories = categories.Select(c => new SelectItemViewModel
        {
            Id = c.Id,
            Name = c.Name,
            Selected = viewModel.CategoryIds.Contains(c.Id)
        }).ToList();

        // Get tags
        var tags = await _context.Set<Tag>()
            .Where(t => t.Type == TagType.Gallery)
            .OrderBy(t => t.Name)
            .ToListAsync();

        viewModel.AvailableTags = tags.Select(t => new SelectItemViewModel
        {
            Id = t.Id,
            Name = t.Name,
            Selected = viewModel.TagIds.Contains(t.Id)
        }).ToList();
    }

    private async Task<string?> UploadImage(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
        {
            return null;
        }

        // Create directory if it doesn't exist
        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", folder);
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

        return $"/uploads/{folder}/{uniqueFileName}";
    }

    private string GenerateSlugFromName(string name)
    {
        // Convert to lowercase
        var slug = name.ToLowerInvariant();

        // Remove diacritics (accents)
        slug = RemoveDiacritics(slug);

        // Replace spaces with hyphens
        slug = Regex.Replace(slug, @"\s+", "-");

        // Remove invalid characters
        slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");

        // Remove multiple hyphens
        slug = Regex.Replace(slug, @"-+", "-");

        // Trim hyphens from start and end
        slug = slug.Trim('-');

        return slug;
    }

    private string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(System.Text.NormalizationForm.FormD);
        var stringBuilder = new System.Text.StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString();
    }
}

public class GalleryImageOrderViewModel
{
    public int Id { get; set; }
    public int OrderIndex { get; set; }
}