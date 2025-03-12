using application.Interfaces;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Models;

namespace application.Services;

public class ContactService(ApplicationDbContext context) : IContactService
{
    public async Task<List<Contact>> GetAllAsync()
    {
        try
        {
            // Truy vấn trực tiếp
            return await context.Contacts.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Contact?> GetByIdAsync(int id)
    {
        try
        {
            // Truy vấn trực tiếp
            return await context.Contacts
                .Where(ct => ct.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<BaseResponse> AddAsync(Contact model)
    {
        try
        {
            // Thêm trực tiếp
            //var errors = new Dictionary<string, string>(); //Không có validation đặc biệt.

            await context.Contacts.AddAsync(model);
            await context.SaveChangesAsync();

            return new SuccessResponse<Contact>(model, "Thêm thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", [ex.Message] } });
        }
    }

    public async Task<BaseResponse> UpdateAsync(int id, Contact model)
    {
        try
        {
            // Tìm và cập nhật trực tiếp
            var existingContact = await context.Contacts.FindAsync(id);

            if (existingContact == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Liên hệ không tồn tại"] }
                });

            existingContact.Name = model.Name;
            existingContact.Email = model.Email;
            existingContact.Phone = model.Phone;
            existingContact.Message = model.Message;
            existingContact.Status = model.Status;

            // Không cần gọi UpdateAsync, EF Core tự tracking.
            await context.SaveChangesAsync();

            return new SuccessResponse<Contact>(existingContact, "Cập nhật liên hệ thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", [ex.Message] } });
        }
    }

    public async Task<BaseResponse> DeleteAsync(int id)
    {
        try
        {
            var existingContact = await context.Contacts.FirstOrDefaultAsync(c => c.Id == id);

            if (existingContact == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Liên hệ không tồn tại"] }
                });

            existingContact.DeletedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();

            return new SuccessResponse<Contact>(existingContact, "Đã xóa thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", [ex.Message] } });
        }
    }
}