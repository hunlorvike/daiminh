using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoRegister;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Services;

[Register(ServiceLifetime.Scoped)]
public class PopupModalService : IPopupModalService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<PopupModalService> _logger;

    public PopupModalService(ApplicationDbContext context, IMapper mapper, ILogger<PopupModalService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IPagedList<PopupModalListItemViewModel>> GetPagedPopupModalsAsync(PopupModalFilterViewModel filter, int pageNumber, int pageSize)
    {
        IQueryable<PopupModal> query = _context.Set<PopupModal>().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(p => p.Title.ToLower().Contains(lowerSearchTerm) ||
                                     (p.Content != null && p.Content.ToLower().Contains(lowerSearchTerm)));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(p => p.IsActive == filter.IsActive.Value);
        }

        query = query.OrderByDescending(p => p.OrderIndex).ThenByDescending(p => p.CreatedAt);

        return await query
            .ProjectTo<PopupModalListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);
    }

    public async Task<PopupModalViewModel?> GetPopupModalByIdAsync(int id)
    {
        var popupModal = await _context.Set<PopupModal>()
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(p => p.Id == id);
        return _mapper.Map<PopupModalViewModel>(popupModal);
    }

    public async Task<OperationResult<int>> CreatePopupModalAsync(PopupModalViewModel viewModel)
    {
        var popupModal = _mapper.Map<PopupModal>(viewModel);
        _context.Add(popupModal);
        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created PopupModal: ID={Id}, Title={Title}", popupModal.Id, popupModal.Title);
            return OperationResult<int>.SuccessResult(popupModal.Id, $"Thêm Popup '{popupModal.Title}' thành công.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating PopupModal: {Title}", viewModel.Title);
            return OperationResult<int>.FailureResult("Lỗi khi tạo Popup Modal.");
        }
    }

    public async Task<OperationResult> UpdatePopupModalAsync(PopupModalViewModel viewModel)
    {
        var popupModal = await _context.Set<PopupModal>().FirstOrDefaultAsync(p => p.Id == viewModel.Id);
        if (popupModal == null)
        {
            return OperationResult.FailureResult("Không tìm thấy Popup Modal.");
        }

        _mapper.Map(viewModel, popupModal);
        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated PopupModal: ID={Id}, Title={Title}", popupModal.Id, popupModal.Title);
            return OperationResult.SuccessResult($"Cập nhật Popup '{popupModal.Title}' thành công.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating PopupModal ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult("Lỗi khi cập nhật Popup Modal.");
        }
    }

    public async Task<OperationResult> DeletePopupModalAsync(int id)
    {
        var popupModal = await _context.Set<PopupModal>().FirstOrDefaultAsync(p => p.Id == id);
        if (popupModal == null)
        {
            return OperationResult.FailureResult("Không tìm thấy Popup Modal.");
        }
        string title = popupModal.Title;
        _context.Remove(popupModal);
        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted PopupModal: ID={Id}, Title={Title}", id, title);
            return OperationResult.SuccessResult($"Xóa Popup '{title}' thành công.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting PopupModal ID: {Id}", id);
            return OperationResult.FailureResult("Lỗi khi xóa Popup Modal.");
        }
    }
}