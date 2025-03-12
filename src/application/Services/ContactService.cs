using application.Interfaces;
using domain.Entities;
using Microsoft.EntityFrameworkCore;
using shared.Interfaces;
using shared.Models;

namespace application.Services;

public class ContactService(IUnitOfWork unitOfWork) : IContactService
{
    public async Task<List<Contact>> GetAllAsync()
    {
        try
        {
            var repository = unitOfWork.GetRepository<Contact, int>();
            return await repository.AsNoTracking().ToListAsync();
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
            var repository = unitOfWork.GetRepository<Contact, int>();
            return await repository.Where(ct => ct.Id == id)
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
            var repository = unitOfWork.GetRepository<Contact, int>();

            var errors = new Dictionary<string, string>();

            await repository.AddAsync(model);
            await unitOfWork.SaveChangesAsync();

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
            var repository = unitOfWork.GetRepository<Contact, int>();
            var existingContact = await repository.FindByIdAsync(id);

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

            await repository.UpdateAsync(existingContact);
            await unitOfWork.SaveChangesAsync();

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
            var repository = unitOfWork.GetRepository<Contact, int>();
            var existingContact = await repository.FirstOrDefaultAsync(c => c.Id == id);

            if (existingContact == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Liên hệ không tồn tại"] }
                });

            existingContact.DeletedAt = DateTime.UtcNow;

            await repository.DeleteAsync(existingContact);
            await unitOfWork.SaveChangesAsync();

            return new SuccessResponse<Contact>(existingContact, "Đã xóa thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", [ex.Message] } });
        }
    }
}