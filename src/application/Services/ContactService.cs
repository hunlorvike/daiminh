using application.Interfaces;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Models;

namespace application.Services;

/// <summary>
/// Service for managing contact information.
/// </summary>
public class ContactService(ApplicationDbContext context) : IContactService
{
    /// <summary>
    /// Retrieves all contact entries.
    /// </summary>
    /// <returns>A list of contact entries.</returns>
    public async Task<List<Contact>> GetAllAsync()
    {
        try
        {
            // Retrieve all contact entries from the database.
            return await context.Contacts.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            // Log exception
            // _logger.LogError(ex, "An error occurred while retrieving all contact entries.");
            throw new Exception("Đã xảy ra lỗi khi lấy danh sách liên hệ.", ex);
        }
    }

    /// <summary>
    /// Retrieves a contact entry by its ID.
    /// </summary>
    /// <param name="id">The ID of the contact entry.</param>
    /// <returns>The contact entry, or null if not found.</returns>
    public async Task<Contact?> GetByIdAsync(int id)
    {
        try
        {
            // Retrieve a contact entry by ID from the database.
            return await context.Contacts
                .AsNoTracking()
                .FirstOrDefaultAsync(ct => ct.Id == id); // More efficient to use FirstOrDefaultAsync directly
        }
        catch (Exception ex)
        {
            // Log exception
            //_logger.LogError(ex, $"An error occurred while retrieving contact entry with ID: {id}.");
            throw new Exception($"Đã xảy ra lỗi khi lấy thông tin liên hệ có ID: {id}.", ex);
        }
    }

    /// <summary>
    /// Adds a new contact entry.
    /// </summary>
    /// <param name="model">The contact entry to add.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> AddAsync(Contact model)
    {
        try
        {
            // Add the new contact entry to the database.
            await context.Contacts.AddAsync(model);
            await context.SaveChangesAsync();

            return new SuccessResponse<Contact>(model, "Thêm liên hệ mới thành công.");
        }
        catch (Exception ex)
        {
            // Log exception
            //_logger.LogError(ex, "An error occurred while adding a new contact entry.");
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", ["Đã xảy ra lỗi khi thêm liên hệ mới. Vui lòng thử lại sau."] } });
        }
    }

    /// <summary>
    /// Updates an existing contact entry.
    /// </summary>
    /// <param name="id">The ID of the contact entry to update.</param>
    /// <param name="model">The updated contact entry data.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> UpdateAsync(int id, Contact model)
    {
        try
        {
            // Find the existing contact entry by ID.
            var existingContact = await context.Contacts.FindAsync(id); // Use FindAsync

            if (existingContact == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Liên hệ không tồn tại."] }
                });

            // Update the contact entry properties.
            existingContact.Name = model.Name; // No null check needed since Name is required
            existingContact.Email = model.Email; // No null check needed since Email is required
            existingContact.Phone = model.Phone;  // No null check, could be empty string.
            existingContact.Message = model.Message; // No null check, could be empty.
            existingContact.Status = model.Status; // No null check, enum with default value

            // Save the changes to the database.
            await context.SaveChangesAsync();

            return new SuccessResponse<Contact>(existingContact, "Cập nhật thông tin liên hệ thành công.");
        }
        catch (Exception ex)
        {
            // Log exception
            // _logger.LogError(ex, $"An error occurred while updating contact entry with ID: {id}.");
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", ["Đã xảy ra lỗi khi cập nhật thông tin liên hệ. Vui lòng thử lại sau."] } });
        }
    }

    /// <summary>
    /// Deletes a contact entry (soft delete).
    /// </summary>
    /// <param name="id">The ID of the contact entry to delete.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> DeleteAsync(int id)
    {
        try
        {
            // Find the existing contact entry by ID.
            var existingContact = await context.Contacts.FirstOrDefaultAsync(c => c.Id == id);

            if (existingContact == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Liên hệ không tồn tại."] }
                });

            // Perform a soft delete by setting the DeletedAt property.
            existingContact.DeletedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();

            return new SuccessResponse<Contact>(existingContact, "Xóa liên hệ thành công.");
        }
        catch (Exception ex)
        {
            // Log exception
            // _logger.LogError(ex, $"An error occurred while deleting contact entry with ID: {id}.");
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", ["Đã xảy ra lỗi khi xóa liên hệ. Vui lòng thử lại sau."] } });
        }
    }
}