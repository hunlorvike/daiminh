using AutoMapper;
using domain.Entities;
using FluentValidation;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Admin.Services;
using web.Areas.Admin.ViewModels.Media;

namespace web.Areas.Admin.Controllers.Api;

[Route("api/admin/media")]
[ApiController]
[Area("Admin")]
[Authorize]
public class MediaApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMinioStorageService _minioService;
    private readonly IValidator<MediaFolderViewModel> _folderValidator;
    private readonly IValidator<MediaFileEditViewModel> _fileEditValidator;
    private readonly ILogger<MediaApiController> _logger;

    public MediaApiController(
     ApplicationDbContext context,
     IMapper mapper,
     IMinioStorageService minioService,
     IValidator<MediaFolderViewModel> folderValidator,
     IValidator<MediaFileEditViewModel> fileEditValidator,
     ILogger<MediaApiController> logger)
    {
        _context = context;
        _mapper = mapper;
        _minioService = minioService;
        _folderValidator = folderValidator;
        _fileEditValidator = fileEditValidator;
        _logger = logger;
    }

    // GET: api/admin/media/items?folderId=1&searchTerm=...&mediaType=Image
    [HttpGet("items")]
    public async Task<ActionResult<MediaItemsResultViewModel>> GetItems(int? folderId = null, string? searchTerm = null, MediaType? mediaType = null)
    {
        _logger.LogInformation("Fetching media items for FolderId: {FolderId}, SearchTerm: {SearchTerm}, MediaType: {MediaType}", folderId, searchTerm, mediaType);

        // Base query for folders in the current parent folder
        var folderQuery = _context.Set<MediaFolder>()
                                 .Where(f => f.ParentId == folderId); // Root folders if folderId is null

        // Base query for files in the current parent folder
        var fileQuery = _context.Set<MediaFile>()
                                .Where(f => f.FolderId == folderId);

        // Apply search term
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            folderQuery = folderQuery.Where(f => f.Name.Contains(searchTerm) || (f.Description != null && f.Description.Contains(searchTerm)));
            fileQuery = fileQuery.Where(f => f.OriginalFileName.Contains(searchTerm) || (f.AltText != null && f.AltText.Contains(searchTerm)) || (f.Description != null && f.Description.Contains(searchTerm)));
        }

        // Apply media type filter (only to files)
        if (mediaType.HasValue)
        {
            fileQuery = fileQuery.Where(f => f.MediaType == mediaType.Value);
        }

        // Fetch data
        var folders = await folderQuery.OrderBy(f => f.Name).ToListAsync();
        var files = await fileQuery.OrderByDescending(f => f.CreatedAt).ToListAsync(); // Order files by creation date

        // Map to ViewModel
        var folderItems = _mapper.Map<List<MediaItemViewModel>>(folders);
        var fileItems = _mapper.Map<List<MediaItemViewModel>>(files);

        // Combine and sort (folders first, then files)
        var allItems = folderItems.Concat(fileItems).ToList();

        // --- Breadcrumbs ---
        var breadcrumbs = new List<BreadcrumbItemViewModel>();
        var currentFolder = folderId.HasValue ? await _context.Set<MediaFolder>().FindAsync(folderId.Value) : null;
        while (currentFolder != null)
        {
            breadcrumbs.Add(_mapper.Map<BreadcrumbItemViewModel>(currentFolder));
            if (currentFolder.ParentId.HasValue)
            {
                currentFolder = await _context.Set<MediaFolder>()
                                              .FirstOrDefaultAsync(f => f.Id == currentFolder.ParentId.Value);
            }
            else
            {
                currentFolder = null;
            }
        }
        breadcrumbs.Add(new BreadcrumbItemViewModel { Id = null, Name = "Media Library" }); // Root
        breadcrumbs.Reverse(); // Correct order

        var result = new MediaItemsResultViewModel
        {
            Items = allItems,
            Breadcrumbs = breadcrumbs,
            CurrentFolderId = folderId
        };

        return Ok(result);
    }

    // POST: api/admin/media/upload?folderId=1
    [HttpPost("upload")]
    [RequestSizeLimit(100_000_000)] // Set limit e.g., 100 MB, configure as needed
    [RequestFormLimits(MultipartBodyLengthLimit = 100_000_000)] // Also configure form limits
    public async Task<ActionResult<MediaItemViewModel>> UploadFile([FromForm] IFormFile file, [FromQuery] int? folderId = null)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "No file uploaded." });
        }

        _logger.LogInformation("Attempting upload for file: {FileName}, Size: {FileSize}, FolderId: {FolderId}", file.FileName, file.Length, folderId);

        // Optional: Determine subfolder path based on folderId
        string? minioSubFolder = null;
        if (folderId.HasValue)
        {
            // You might want a helper function to build the full path for MinIO
            // based on the folder hierarchy in your database.
            // For simplicity here, we'll just use the folder ID. Avoid if possible.
            minioSubFolder = $"folder_{folderId.Value}"; // Example - NEEDS BETTER LOGIC
            _logger.LogWarning("Using simplified subfolder logic: {SubFolder}. Consider implementing full path resolution.", minioSubFolder);
        }

        var uploadResult = await _minioService.UploadFileAsync(file, minioSubFolder);

        if (uploadResult == null)
        {
            _logger.LogError("MinIO upload failed for file: {FileName}", file.FileName);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to upload file to storage." });
        }

        _logger.LogInformation("MinIO upload successful. ObjectName: {ObjectName}", uploadResult.ObjectName);

        // Map MinIO result to MediaFile entity
        var mediaFile = _mapper.Map<MediaFile>(uploadResult);
        mediaFile.FolderId = folderId; // Assign the folder ID

        _context.MediaFiles.Add(mediaFile);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("MediaFile entity saved to DB with ID: {MediaFileId}", mediaFile.Id);
            // Map the newly created entity back to a ViewModel to return
            var viewModel = _mapper.Map<MediaItemViewModel>(mediaFile);
            return CreatedAtAction(nameof(GetItems), new { folderId = mediaFile.FolderId }, viewModel); // Return 201 Created
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving MediaFile entity to DB after successful MinIO upload. ObjectName: {ObjectName}", uploadResult.ObjectName);
            // Attempt to clean up the orphaned MinIO file
            await _minioService.DeleteFileAsync(uploadResult.ObjectName);
            _logger.LogWarning("Attempted to delete orphaned MinIO object: {ObjectName}", uploadResult.ObjectName);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to save file metadata to database after upload." });
        }
    }

    // POST: api/admin/media/folder
    [HttpPost("folder")]
    public async Task<ActionResult<MediaItemViewModel>> CreateFolder(MediaFolderViewModel viewModel)
    {
        var validationResult = await _folderValidator.ValidateAsync(viewModel);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary()); // Return validation errors
        }

        // Additional check: Prevent creating folder with same name in same parent
        bool nameExists = await _context.MediaFolders
            .AnyAsync(f => f.ParentId == viewModel.ParentId && f.Name == viewModel.Name);
        if (nameExists)
        {
            return BadRequest(new { Name = new[] { "Tên thư mục đã tồn tại trong thư mục cha này." } });
        }

        var folder = _mapper.Map<MediaFolder>(viewModel);
        folder.Children = new List<MediaFolder>(); // Initialize collections
        folder.Files = new List<MediaFile>();

        _context.MediaFolders.Add(folder);
        await _context.SaveChangesAsync();

        var resultViewModel = _mapper.Map<MediaItemViewModel>(folder);
        return CreatedAtAction(nameof(GetItems), new { folderId = folder.Id }, resultViewModel);
    }

    // PUT: api/admin/media/file/details/5 (Using PUT for update)
    [HttpPut("file/details/{id}")]
    public async Task<IActionResult> UpdateFileDetails(int id, MediaFileEditViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return BadRequest(new { message = "ID mismatch." });
        }

        var validationResult = await _fileEditValidator.ValidateAsync(viewModel);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }

        var mediaFile = await _context.MediaFiles.FindAsync(id);
        if (mediaFile == null)
        {
            return NotFound(new { message = "File not found." });
        }

        _mapper.Map(viewModel, mediaFile); // Update only AltText and Description

        try
        {
            await _context.SaveChangesAsync();
            return NoContent(); // Success, no content to return
        }
        catch (DbUpdateConcurrencyException)
        {
            _logger.LogWarning("Concurrency conflict updating MediaFile ID {MediaFileId}", id);
            return Conflict(new { message = "Concurrency conflict. The file may have been modified by someone else." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating MediaFile details for ID {MediaFileId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while saving changes." });
        }
    }

    // DELETE: api/admin/media/delete/file/5
    [HttpPost("delete/file/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteFile(int id)
    {
        var mediaFile = await _context.MediaFiles.FindAsync(id);
        if (mediaFile == null)
        {
            return NotFound(new { message = "File not found." });
        }

        var objectName = mediaFile.FilePath; // Get the path stored in DB

        _context.MediaFiles.Remove(mediaFile);

        try
        {
            await _context.SaveChangesAsync(); // Save DB changes first

            // If DB save is successful, delete from MinIO
            if (!string.IsNullOrEmpty(objectName))
            {
                await _minioService.DeleteFileAsync(objectName);
            }
            else
            {
                _logger.LogWarning("MediaFile ID {MediaFileId} deleted from DB but had no FilePath to delete from MinIO.", id);
            }

            return Ok(new { success = true, message = "File deleted successfully." }); // Use Ok with body for consistency
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting MediaFile ID {MediaFileId}. DB changes might have been saved, but MinIO deletion might have failed or vice-versa.", id);
            // Consider more sophisticated transaction/rollback if needed
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred during deletion." });
        }
    }

    // DELETE: api/admin/media/delete/folder/5
    [HttpPost("delete/folder/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteFolder(int id)
    {
        var folder = await _context.MediaFolders
                                 .Include(f => f.Children) // Check for subfolders
                                 .Include(f => f.Files)    // Check for files
                                 .FirstOrDefaultAsync(f => f.Id == id);

        if (folder == null)
        {
            return NotFound(new { message = "Folder not found." });
        }

        // Basic check: Don't delete if it contains items
        if (folder.Children.Any() || folder.Files.Any())
        {
            return BadRequest(new { message = "Cannot delete folder. Please ensure it is empty first." });
        }

        // TODO: More robust deletion might involve recursive deletion or
        // defining the MinIO folder path more accurately.
        // For now, we only delete the DB record if empty.
        // MinIO deletion for the "folder" prefix is complex and potentially dangerous if paths overlap.
        // It's often safer to delete files individually.

        _context.MediaFolders.Remove(folder);

        try
        {
            await _context.SaveChangesAsync();

            // Optional: Attempt to delete the corresponding "folder" prefix in MinIO if needed,
            // but requires careful path construction and understanding MinIO prefixes.
            // string minioFolderPath = $"folder_{id}/"; // VERY simplified example
            // await _minioService.DeleteFolderAsync(minioFolderPath);

            return Ok(new { success = true, message = "Folder deleted successfully." });
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Database error deleting folder ID {FolderId}. It might be referenced elsewhere (DeleteBehavior.Restrict).", id);
            return BadRequest(new { message = "Cannot delete folder. It might be referenced elsewhere or a database error occurred." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting folder ID {FolderId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred during deletion." });
        }
    }

    // GET: api/admin/media/file/details/5 (For edit modal)
    [HttpGet("file/details/{id}")]
    public async Task<ActionResult<MediaFileEditViewModel>> GetFileDetails(int id)
    {
        var mediaFile = await _context.MediaFiles.FindAsync(id);
        if (mediaFile == null)
        {
            return NotFound(new { message = "File not found." });
        }
        var viewModel = _mapper.Map<MediaFileEditViewModel>(mediaFile);
        return Ok(viewModel);
    }
}
