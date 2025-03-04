using core.Attributes;
using Core.Common.Models;
using core.Entities;
using core.Interfaces.Infrastructure;
using core.Interfaces.Service;
using Microsoft.EntityFrameworkCore;

namespace core.Services;

public class SubscriberService(IUnitOfWork unitOfWork) : ScopedService, ISubscriberService
{
    public async Task<List<Subscriber>> GetAllAsync()
    {
        try
        {
            var subscriberRepository = unitOfWork.GetRepository<Subscriber, int>();

            return await subscriberRepository
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
            var subscriberRepository = unitOfWork.GetRepository<Subscriber, int>();

            return await subscriberRepository
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
            var subscriberRepository = unitOfWork.GetRepository<Subscriber, int>();

            var errors = new Dictionary<string, string[]>();

            if (errors.Count != 0) return new ErrorResponse(errors);

            await subscriberRepository.AddAsync(model);
            await unitOfWork.SaveChangesAsync();

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
            var subscriberRepository = unitOfWork.GetRepository<Subscriber, int>();
            var existingSubscriber = await subscriberRepository
                .FirstOrDefaultAsync(ct => ct.Id == id);

            if (existingSubscriber == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Subscriber không tồn tại"] }
                });

            existingSubscriber.Email = model.Email ?? existingSubscriber.Email;


            await unitOfWork.SaveChangesAsync();

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
            var subscriberRepository = unitOfWork.GetRepository<Subscriber, int>();
            var subscriber = await subscriberRepository.FirstOrDefaultAsync(ct => ct.Id == id);

            if (subscriber == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Email không tồn tại."] } });

            subscriber.DeletedAt = DateTime.UtcNow;

            await unitOfWork.SaveChangesAsync();

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