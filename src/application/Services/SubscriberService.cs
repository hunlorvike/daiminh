using application.Interfaces;
using domain.Entities;
using infrastructure; // Namespace của ApplicationDbContext
using Microsoft.EntityFrameworkCore;
using shared.Models;

namespace application.Services;

/// <summary>
/// Service for managing subscribers.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SubscriberService"/> class.
/// </remarks>
/// <param name="context">The application database context.</param>
public class SubscriberService(ApplicationDbContext context) : ISubscriberService
{

    /// <summary>
    /// Retrieves all subscribers.
    /// </summary>
    /// <returns>A list of subscribers.</returns>
    public async Task<List<Subscriber>> GetAllAsync()
    {
        try
        {
            // Retrieve all subscribers from the database.
            return await context.Subscribers
                .AsNoTracking()
                .Where(x => x.DeletedAt == null)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, "GetAllAsync SubscriberService");
            throw new Exception("Đã xảy ra lỗi khi lấy danh sách người đăng ký.", ex);
        }
    }

    /// <summary>
    /// Retrieves a subscriber by its ID.
    /// </summary>
    /// <param name="id">The ID of the subscriber.</param>
    /// <returns>The subscriber, or null if not found.</returns>
    public async Task<Subscriber?> GetByIdAsync(int id)
    {
        try
        {
            // Retrieve a subscriber by ID from the database.
            return await context.Subscribers
                .AsNoTracking()
                .FirstOrDefaultAsync(ct => ct.Id == id && ct.DeletedAt == null); // Use FirstOrDefaultAsync
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, $"GetByIdAsync SubscriberService with id: {id}");
            throw new Exception($"Đã xảy ra lỗi khi lấy thông tin người đăng ký có ID: {id}.", ex);
        }
    }

    /// <summary>
    /// Adds a new subscriber.
    /// </summary>
    /// <param name="model">The subscriber to add.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> AddAsync(Subscriber model)
    {
        try
        {
            // Check for existing email.
            var existingSubscriber = await context.Subscribers.FirstOrDefaultAsync(s => s.Email == model.Email && s.DeletedAt == null);
            if (existingSubscriber != null)
            {
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.Email), ["Địa chỉ email này đã được đăng ký. Vui lòng sử dụng một địa chỉ email khác."] }
                });
            }

            // Add the new subscriber to the database.
            await context.Subscribers.AddAsync(model);
            await context.SaveChangesAsync();

            return new SuccessResponse<Subscriber>(model, "Đăng ký nhận tin thành công.");
        }
        catch (Exception)
        {
            //log
            //_logger.LogError(ex, "AddAsync SubscriberService");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi khi đăng ký nhận tin. Vui lòng thử lại sau."] }
            });
        }
    }

    /// <summary>
    /// Updates an existing subscriber.
    /// </summary>
    /// <param name="id">The ID of the subscriber to update.</param>
    /// <param name="model">The updated subscriber data.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> UpdateAsync(int id, Subscriber model)
    {
        try
        {
            // Find the existing subscriber by ID.
            var existingSubscriber = await context.Subscribers
                .FirstOrDefaultAsync(ct => ct.Id == id && ct.DeletedAt == null);

            if (existingSubscriber == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Người đăng ký không tồn tại hoặc đã bị xóa."] }
                });

            // Check for duplicate email (excluding the current record).
            var duplicateEmail = await context.Subscribers.FirstOrDefaultAsync(s => s.Email == model.Email && s.Id != id && s.DeletedAt == null);
            if (duplicateEmail != null)
            {
                return new ErrorResponse(new Dictionary<string, string[]>()
                {
                    {nameof(model.Email), ["Địa chỉ email này đã được đăng ký. Vui lòng sử dụng một địa chỉ email khác."]}
                });
            }

            // Update the subscriber's email.
            existingSubscriber.Email = model.Email ?? existingSubscriber.Email;

            await context.SaveChangesAsync();

            return new SuccessResponse<Subscriber>(existingSubscriber, "Cập nhật thông tin người đăng ký thành công.");
        }
        catch (Exception)
        {
            //log
            //_logger.LogError(ex, $"UpdateAsync SubscriberService with id: {id}");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi khi cập nhật thông tin người đăng ký. Vui lòng thử lại sau."] }
            });
        }
    }

    /// <summary>
    /// Deletes a subscriber (soft delete).
    /// </summary>
    /// <param name="id">The ID of the subscriber to delete.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> DeleteAsync(int id)
    {
        try
        {
            // Find the subscriber by ID (only if not already soft-deleted).
            var subscriber = await context.Subscribers.FirstOrDefaultAsync(ct => ct.Id == id && ct.DeletedAt == null);

            if (subscriber == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Người đăng ký không tồn tại hoặc đã bị xóa."] } });

            // Perform a soft delete by setting the DeletedAt property.
            subscriber.DeletedAt = DateTime.UtcNow; // Soft delete

            await context.SaveChangesAsync();

            return new SuccessResponse<Subscriber>(subscriber, "Xóa người đăng ký thành công (đã ẩn).");
        }
        catch (Exception)
        {
            //log
            //_logger.LogError(ex, $"DeleteAsync SubscriberService with id: {id}");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi khi xóa người đăng ký. Vui lòng thử lại sau."] }
            });
        }
    }
}