using application.Interfaces;
using domain.Entities;
using infrastructure; // Namespace của ApplicationDbContext
using Microsoft.EntityFrameworkCore;
using shared.Models;

namespace application.Services;

public class SubscriberService : ISubscriberService
{
    private readonly ApplicationDbContext _context; // Inject DbContext

    public SubscriberService(ApplicationDbContext context) // Constructor
    {
        _context = context;
    }

    public async Task<List<Subscriber>> GetAllAsync()
    {
        try
        {
            // Direct query
            return await _context.Subscribers
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Subscriber?> GetByIdAsync(int id)
    {
        try
        {
            // Direct query
            return await _context.Subscribers
                .Where(ct => ct.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<BaseResponse> AddAsync(Subscriber model)
    {
        try
        {
            // Check for existing email (Important!)
            var existingSubscriber = await _context.Subscribers.FirstOrDefaultAsync(s => s.Email == model.Email);
            if (existingSubscriber != null)
            {
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.Email), ["Email đã đăng ký."] }
                });
            }

            // Direct add
            await _context.Subscribers.AddAsync(model);
            await _context.SaveChangesAsync();

            return new SuccessResponse<Subscriber>(model, "Thêm thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            });
        }
    }

    public async Task<BaseResponse> UpdateAsync(int id, Subscriber model)
    {
        try
        {
            // Find existing subscriber
            var existingSubscriber = await _context.Subscribers
                .FirstOrDefaultAsync(ct => ct.Id == id);

            if (existingSubscriber == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Subscriber không tồn tại"] }
                });

            // Check for duplicate email (excluding current record)
            var duplicateEmail = await _context.Subscribers.FirstOrDefaultAsync(s => s.Email == model.Email && s.Id != id);
            if (duplicateEmail != null)
            {
                return new ErrorResponse(new Dictionary<string, string[]>()
                {
                    {nameof(model.Email), ["Email đã được sử dụng."]}
                });
            }

            // Update email
            existingSubscriber.Email = model.Email ?? existingSubscriber.Email;

            await _context.SaveChangesAsync();

            return new SuccessResponse<Subscriber>(existingSubscriber, "Cập nhật thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            });
        }
    }

    public async Task<BaseResponse> DeleteAsync(int id)
    {
        try
        {
            // Find and soft-delete
            var subscriber = await _context.Subscribers.FirstOrDefaultAsync(ct => ct.Id == id);

            if (subscriber == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Email không tồn tại."] } }); // Corrected message

            subscriber.DeletedAt = DateTime.UtcNow; // Soft delete

            await _context.SaveChangesAsync();

            return new SuccessResponse<Subscriber>(subscriber, "Đã xóa thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            });
        }
    }
}