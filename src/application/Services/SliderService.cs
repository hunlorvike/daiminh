using application.Interfaces;
using domain.Entities;
using infrastructure; // Namespace của ApplicationDbContext
using Microsoft.EntityFrameworkCore;
using shared.Models;

namespace application.Services;

/// <summary>
/// Service for managing slider items.
/// </summary>
public partial class SliderService : ISliderService
{
    private readonly ApplicationDbContext _context; // Inject DbContext

    /// <summary>
    /// Initializes a new instance of the <see cref="SliderService"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public SliderService(ApplicationDbContext context) // Constructor
    {
        _context = context;
    }

    /// <summary>
    /// Adds a new slider item.
    /// </summary>
    /// <param name="model">The slider item to add.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> AddAsync(Slider model)
    {
        try
        {
            // Check for duplicates on Title, ImageUrl, LinkUrl, and Order.
            var errors = new Dictionary<string, string[]>();

            var existingSlider = await _context.Sliders.FirstOrDefaultAsync(s =>
                (s.Title == model.Title && model.Title != null && model.Title != string.Empty) ||
                (s.ImageUrl == model.ImageUrl && model.ImageUrl != null && model.ImageUrl != string.Empty) ||
                (s.Order == model.Order) ||
                (s.LinkUrl == model.LinkUrl && model.LinkUrl != null && model.LinkUrl != string.Empty) && s.DeletedAt == null

            );

            if (existingSlider != null)
            {
                if (existingSlider.Title == model.Title)
                    errors.Add(nameof(model.Title), ["Tiêu đề slider đã tồn tại. Vui lòng chọn một tiêu đề khác."]);
                if (existingSlider.LinkUrl == model.LinkUrl)
                    errors.Add(nameof(model.LinkUrl), ["Đường dẫn liên kết đã tồn tại. Vui lòng chọn một đường dẫn khác."]);
                if (existingSlider.ImageUrl == model.ImageUrl)
                    errors.Add(nameof(model.ImageUrl), ["Hình ảnh đã tồn tại. Vui lòng chọn một hình ảnh khác."]);
                if (existingSlider.Order == model.Order)
                    errors.Add(nameof(model.Order), ["Thứ tự hiển thị đã tồn tại. Vui lòng chọn một thứ tự khác."]);
            }

            if (errors.Count != 0) return new ErrorResponse(errors);

            // Add the new slider item to the database.
            await _context.Sliders.AddAsync(model);
            await _context.SaveChangesAsync();

            return new SuccessResponse<Slider>(model, "Thêm slider mới thành công.");
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, "AddAsync SliderService");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi khi thêm slider mới. Vui lòng thử lại sau."] }
            });
        }
    }


    /// <summary>
    /// Deletes a slider item (soft delete).
    /// </summary>
    /// <param name="id">The ID of the slider item to delete.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> DeleteAsync(int id)
    {
        try
        {
            // Find the slider item by ID (only if not already soft-deleted).
            var slider = await _context.Sliders.FirstOrDefaultAsync(ct => ct.Id == id && ct.DeletedAt == null);

            if (slider == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Slider không tồn tại hoặc đã bị xóa."] } });
            slider.DeletedAt = DateTime.UtcNow; // Soft delete
            await _context.SaveChangesAsync();

            return new SuccessResponse<Slider>(slider, "Xóa slider thành công (đã ẩn).");
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, $"DeleteAsync SliderService with id: {id}");
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", ["Đã xảy ra lỗi khi xóa slider. Vui lòng thử lại sau."] } });
        }
    }

    /// <summary>
    /// Retrieves all slider items.
    /// </summary>
    /// <returns>A list of slider items.</returns>
    public async Task<List<Slider>> GetAllAsync()
    {
        try
        {
            // Retrieve all slider items from the database.
            return await _context.Sliders
                .AsNoTracking()
                .Where(x => x.DeletedAt == null)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, "GetAllAsync SliderService");
            throw new Exception("Đã xảy ra lỗi khi lấy danh sách slider.", ex);
        }
    }

    /// <summary>
    /// Retrieves a slider item by its ID.
    /// </summary>
    /// <param name="id">The ID of the slider item.</param>
    /// <returns>The slider item, or null if not found.</returns>
    public async Task<Slider?> GetByIdAsync(int id)
    {
        try
        {
            // Retrieve a slider item by ID from the database.
            return await _context.Sliders
                .AsNoTracking()
                .FirstOrDefaultAsync(ct => ct.Id == id && ct.DeletedAt == null); // Use FirstOrDefaultAsync
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, $"GetByIdAsync SliderService with id: {id}");
            throw new Exception($"Đã xảy ra lỗi khi lấy thông tin slider có ID: {id}.", ex);
        }
    }

    /// <summary>
    /// Updates an existing slider item.
    /// </summary>
    /// <param name="id">The ID of the slider item to update.</param>
    /// <param name="model">The updated slider item data.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> UpdateAsync(int id, Slider model)
    {
        try
        {
            // Find the existing slider item by ID.
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
               ((s.Title == model.Title && model.Title != null && model.Title != string.Empty) ||
                (s.ImageUrl == model.ImageUrl && model.ImageUrl != null && model.ImageUrl != string.Empty) ||
                (s.Order == model.Order) ||
                (s.LinkUrl == model.LinkUrl && model.LinkUrl != null && model.LinkUrl != string.Empty))
                && s.DeletedAt == null
           );

            if (duplicateSlider != null)
            {
                if (duplicateSlider.Title == model.Title)
                    errors.Add(nameof(model.Title), ["Tiêu đề slider đã tồn tại. Vui lòng chọn một tiêu đề khác."]);
                if (duplicateSlider.LinkUrl == model.LinkUrl)
                    errors.Add(nameof(model.LinkUrl), ["Đường dẫn liên kết đã tồn tại. Vui lòng chọn một đường dẫn khác."]);
                if (duplicateSlider.ImageUrl == model.ImageUrl)
                    errors.Add(nameof(model.ImageUrl), ["Hình ảnh đã tồn tại. Vui lòng chọn một hình ảnh khác."]);
                if (duplicateSlider.Order == model.Order)
                    errors.Add(nameof(model.Order), ["Thứ tự hiển thị đã tồn tại. Vui lòng chọn một thứ tự khác."]);
            }

            if (errors.Count != 0) return new ErrorResponse(errors);

            // Update the slider item properties.
            existingSlider.Title = model.Title ?? existingSlider.Title;
            existingSlider.ImageUrl = model.ImageUrl ?? existingSlider.ImageUrl;
            existingSlider.LinkUrl = model.LinkUrl ?? existingSlider.LinkUrl;
            existingSlider.Order = model.Order;
            existingSlider.OverlayHtml = model.OverlayHtml ?? existingSlider.OverlayHtml;
            existingSlider.OverlayPosition = model.OverlayPosition ?? existingSlider.OverlayPosition;

            await _context.SaveChangesAsync();

            return new SuccessResponse<Slider>(existingSlider, "Cập nhật slider thành công.");
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, $"UpdateAsync SliderService with id: {id}");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi khi cập nhật slider. Vui lòng thử lại sau."] }
            });
        }
    }
}