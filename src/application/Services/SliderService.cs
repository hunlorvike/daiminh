using application.Interfaces;
using domain.Entities;
using Microsoft.EntityFrameworkCore;
using shared.Interfaces;
using shared.Models;


namespace application.Services;

public partial class SliderService(IUnitOfWork unitOfWork) : ISliderService
{
    public async Task<BaseResponse> AddAsync(Slider model)
    {
        try
        {
            var sliderRepository = unitOfWork.GetRepository<Slider, int>();
            var errors = new Dictionary<string, string[]>();

            var existingSlider = await sliderRepository.FirstOrDefaultAsync(s =>
                s.Title == model.Title ||
                s.ImageUrl == model.ImageUrl ||
                s.Order == model.Order ||
                s.LinkUrl == model.LinkUrl
            );

            if (existingSlider != null)
            {
                if (existingSlider.Title == model.Title)
                    errors.Add(nameof(model.Title), ["Tiêu đề đã tồn tại."]);
                if (existingSlider.LinkUrl == model.LinkUrl)
                    errors.Add(nameof(model.LinkUrl), ["Đường dẫn liên kết đã tồn tại."]);
                if (existingSlider.ImageUrl == model.ImageUrl)
                    errors.Add(nameof(model.ImageUrl), ["Hình ảnh đã tồn tại."]);
                if (existingSlider.Order == model.Order)
                    errors.Add(nameof(model.Order), ["Thứ tự hiển thị đã tồn tại."]);
            }

            if (errors.Count != 0) return new ErrorResponse(errors);

            await sliderRepository.AddAsync(model);
            await unitOfWork.SaveChangesAsync();

            return new SuccessResponse<Slider>(model, "Thêm thành công.");
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
            var sliderRepository = unitOfWork.GetRepository<Slider, int>();
            var slider = await sliderRepository.FirstOrDefaultAsync(ct => ct.Id == id);

            if (slider == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Loại slider không tồn tại."] } });
            slider.DeletedAt = DateTime.UtcNow;
            await unitOfWork.SaveChangesAsync();

            return new SuccessResponse<Slider>(slider, "Đã xóa thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", [ex.Message] } });
        }
    }

    public async Task<List<Slider>> GetAllAsync()
    {
        try
        {
            var sliderRepository = unitOfWork.GetRepository<Slider, int>();

            return await sliderRepository
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Slider?> GetByIdAsync(int id)
    {
        try
        {
            var sliderRepository = unitOfWork.GetRepository<Slider, int>();

            return await sliderRepository
                .Where(ct => ct.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<BaseResponse> UpdateAsync(int id, Slider model)
    {
        try
        {
            var sliderRepository = unitOfWork.GetRepository<Slider, int>();
            var errors = new Dictionary<string, string[]>();

            var existingSlider = await sliderRepository.FindByIdAsync(id);
            if (existingSlider == null)
            {
                return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Slider không tồn tại."] }
            });
            }

            var duplicateSlider = await sliderRepository.FirstOrDefaultAsync(s =>
                s.Id != id &&
                (s.Title == model.Title || s.ImageUrl == model.ImageUrl || s.Order == model.Order || s.LinkUrl == model.LinkUrl)
            );

            if (duplicateSlider != null)
            {
                if (duplicateSlider.Title == model.Title)
                    errors.Add(nameof(model.Title), ["Tiêu đề đã tồn tại."]);
                if (duplicateSlider.LinkUrl == model.LinkUrl)
                    errors.Add(nameof(model.LinkUrl), ["Đường dẫn liên kết đã tồn tại."]);
                if (duplicateSlider.ImageUrl == model.ImageUrl)
                    errors.Add(nameof(model.ImageUrl), ["Hình ảnh đã tồn tại."]);
                if (duplicateSlider.Order == model.Order)
                    errors.Add(nameof(model.Order), ["Thứ tự hiển thị đã tồn tại."]);
            }

            if (errors.Count != 0) return new ErrorResponse(errors);

            existingSlider.Title = model.Title ?? existingSlider.Title;
            existingSlider.ImageUrl = model.ImageUrl ?? existingSlider.ImageUrl;
            existingSlider.LinkUrl = model.LinkUrl ?? existingSlider.LinkUrl;
            existingSlider.Order = model.Order != 0 ? model.Order : existingSlider.Order;
            existingSlider.OverlayHtml = model.OverlayHtml ?? existingSlider.OverlayHtml;
            existingSlider.OverlayPosition = model.OverlayPosition ?? existingSlider.OverlayPosition;

            await unitOfWork.SaveChangesAsync();

            return new SuccessResponse<Slider>(existingSlider, "Cập nhật thành công.");
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
