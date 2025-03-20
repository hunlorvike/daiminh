using domain.Entities;
using shared.Models;

namespace application.Interfaces;

/// <summary>
/// Interface for the Content service, defining the contract for content management operations.
/// </summary>
public interface IContentService
{
    /// <summary>
    /// Retrieves all content entries.
    /// </summary>
    /// <returns>A list of Content entities.</returns>
    Task<List<Content>> GetAllAsync();

    /// <summary>
    /// Retrieves a content entry by its ID.
    /// </summary>
    /// <param name="id">The ID of the content entry.</param>
    /// <returns>The Content entity, or null if not found.</returns>
    Task<Content?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new content entry.
    /// </summary>
    /// <param name="model">The Content entity to add.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    Task<BaseResponse> AddAsync(Content model);

    /// <summary>
    /// Updates an existing content entry.
    /// </summary>
    /// <param name="id">The ID of the content entry to update.</param>
    /// <param name="model">The updated Content entity data.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    Task<BaseResponse> UpdateAsync(int id, Content model);

    /// <summary>
    /// Deletes a content entry (soft delete).
    /// </summary>
    /// <param name="id">The ID of the content entry to delete.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    Task<BaseResponse> DeleteAsync(int id);

    Task<Content?> GetLatestContentAsync();
}