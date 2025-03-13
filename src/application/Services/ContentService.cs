using application.Interfaces;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Models;

namespace application.Services;

/// <summary>
/// Service for managing content.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ContentService"/> class.
/// </remarks>
/// <param name="context">The application database context.</param>
public class ContentService(ApplicationDbContext context) : IContentService
{
    /// <inheritdoc/>
    public async Task<List<Content>> GetAllAsync()
    {
        try
        {
            return await context.Contents
                .AsNoTracking()
                .Where(c => c.DeletedAt == null)
                .Include(c => c.ContentType) // Include related data as needed
                .Include(c => c.Author)      // Include Author
                .ToListAsync();
        }
        catch (Exception ex)
        {
            // Use proper logging here
            // _logger.LogError(ex, "Error getting all content");
            throw new Exception("An error occurred while retrieving content.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<Content?> GetByIdAsync(int id)
    {
        try
        {
            return await context.Contents
                .AsNoTracking()
                .Include(c => c.ContentType) // Include related data
                .Include(c => c.Author)
                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, $"Error getting content by ID: {id}");
            throw new Exception($"An error occurred while retrieving content with ID: {id}.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<BaseResponse> AddAsync(Content model)
    {
        try
        {
            // Check for duplicate slugs.
            var existingContent = await context.Contents
                .FirstOrDefaultAsync(c => c.Slug == model.Slug && c.DeletedAt == null);

            if (existingContent != null)
            {
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.Slug), ["A content item with this slug already exists."] }
                });
            }

            // Add the new content item.
            await context.Contents.AddAsync(model);
            await context.SaveChangesAsync();
            return new SuccessResponse<Content>(model, "Content added successfully.");
        }
        catch (Exception)
        {
            //_logger.LogError(ex, "Error adding content");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["An error occurred while adding the content."] }
            });
        }
    }

    /// <inheritdoc/>
    public async Task<BaseResponse> UpdateAsync(int id, Content model)
    {
        try
        {
            // Check for duplicate slugs, excluding the current record.
            var existingSlug = await context.Contents
                .FirstOrDefaultAsync(c => c.Slug == model.Slug && c.Id != id && c.DeletedAt == null);

            if (existingSlug != null)
            {
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.Slug), ["A content item with this slug already exists."] }
                });
            }

            // Find the existing content item.
            var existingContent = await context.Contents.FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

            if (existingContent == null)
            {
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Content not found or has been deleted."] }
                });
            }

            existingContent.ContentTypeId = model.ContentTypeId;
            existingContent.AuthorId = model.AuthorId;
            existingContent.Title = model.Title ?? existingContent.Title;
            existingContent.Slug = model.Slug ?? existingContent.Slug;
            existingContent.ContentBody = model.ContentBody ?? existingContent.ContentBody;
            existingContent.CoverImageUrl = model.CoverImageUrl ?? existingContent.CoverImageUrl;
            existingContent.Status = model.Status;
            existingContent.MetaTitle = model.MetaTitle ?? existingContent.MetaTitle;
            existingContent.MetaDescription = model.MetaDescription ?? existingContent.MetaDescription;
            existingContent.CanonicalUrl = model.CanonicalUrl ?? existingContent.CanonicalUrl;
            existingContent.OgTitle = model.OgTitle ?? existingContent.OgTitle;
            existingContent.OgDescription = model.OgDescription ?? existingContent.OgDescription;
            existingContent.OgImage = model.OgImage ?? existingContent.OgImage;
            existingContent.StructuredData = model.StructuredData ?? existingContent.StructuredData;

            await context.SaveChangesAsync();
            return new SuccessResponse<Content>(existingContent, "Content updated successfully.");
        }
        catch (Exception)
        {
            //_logger.LogError(ex, $"Error updating content with ID: {id}");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["An error occurred while updating the content."] }
            });
        }
    }

    /// <inheritdoc/>
    public async Task<BaseResponse> DeleteAsync(int id)
    {
        try
        {
            // Find the content item (soft delete).
            var content = await context.Contents.FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

            if (content == null)
            {
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Content not found or has been deleted."] }
                });
            }

            content.DeletedAt = DateTime.UtcNow; // Soft delete
            await context.SaveChangesAsync();
            return new SuccessResponse<Content>(content, "Content deleted successfully (soft delete).");
        }
        catch (Exception)
        {
            //_logger.LogError(ex, $"Error deleting content with ID: {id}");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["An error occurred while deleting the content."] }
            });
        }
    }
}