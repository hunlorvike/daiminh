using application.Interfaces;
using domain.Entities;
using infrastructure; // Namespace của ApplicationDbContext
using Microsoft.EntityFrameworkCore;
using shared.Models;

namespace application.Services;

public partial class SliderService : ISliderService
{
    private readonly ApplicationDbContext _context; // Inject DbContext

    public SliderService(ApplicationDbContext context) // Constructor
    {
        _context = context;
    }

    public async Task<BaseResponse> AddAsync(Slider model)
    {
        try
        {
            // Validation and direct add
            var errors = new Dictionary<string, string[]>();

            var existingSlider = await _context.Sliders.FirstOrDefaultAsync(s =>
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

            await _context.Sliders.AddAsync(model);
            await _context.SaveChangesAsync();

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
            // Find and soft-delete
            var slider = await _context.Sliders.FirstOrDefaultAsync(ct => ct.Id == id);

            if (slider == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Slider không tồn tại."] } }); // Corrected message
            slider.DeletedAt = DateTime.UtcNow; // Soft delete
            await _context.SaveChangesAsync();

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
            // Direct query
            return await _context.Sliders
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
            // Direct query
            return await _context.Sliders
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
            // Find existing slider
            var existingSlider = await _context.Sliders.FindAsync(id); // Use FindAsync for efficiency
            if (existingSlider == null)
            {
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Slider không tồn tại."] }
                });
            }
            //Check duplicate
            var errors = new Dictionary<string, string[]>();

            var duplicateSlider = await _context.Sliders.FirstOrDefaultAsync(s =>
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

            // Update fields
            existingSlider.Title = model.Title ?? existingSlider.Title;
            existingSlider.ImageUrl = model.ImageUrl ?? existingSlider.ImageUrl;
            existingSlider.LinkUrl = model.LinkUrl ?? existingSlider.LinkUrl;
            existingSlider.Order = model.Order != 0 ? model.Order : existingSlider.Order;
            existingSlider.OverlayHtml = model.OverlayHtml ?? existingSlider.OverlayHtml;
            existingSlider.OverlayPosition = model.OverlayPosition ?? existingSlider.OverlayPosition;

            await _context.SaveChangesAsync();

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