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
public class ClaimDefinitionService : IClaimDefinitionService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ClaimDefinitionService> _logger;

    public ClaimDefinitionService(ApplicationDbContext context, IMapper mapper, ILogger<ClaimDefinitionService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IPagedList<ClaimDefinitionListItemViewModel>> GetPagedClaimDefinitionsAsync(
        ClaimDefinitionFilterViewModel filter,
        int pageNumber,
        int pageSize)
    {
        IQueryable<ClaimDefinition> query = _context.Set<ClaimDefinition>()
                                                .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(c => c.Type.ToLower().Contains(lowerSearchTerm)
                                  || c.Value.ToLower().Contains(lowerSearchTerm)
                                  || (c.Description != null && c.Description.ToLower().Contains(lowerSearchTerm)));
        }

        query = query.OrderBy(c => c.Type).ThenBy(c => c.Value);

        IPagedList<ClaimDefinitionListItemViewModel> pagedList = await query
            .ProjectTo<ClaimDefinitionListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        return pagedList;
    }

    public async Task<ClaimDefinitionViewModel?> GetClaimDefinitionByIdAsync(int id)
    {
        ClaimDefinition? claimDefinition = await _context.Set<ClaimDefinition>()
                                                     .AsNoTracking()
                                                     .FirstOrDefaultAsync(c => c.Id == id);
        if (claimDefinition == null) return null;

        return _mapper.Map<ClaimDefinitionViewModel>(claimDefinition);
    }

    public async Task<OperationResult<int>> CreateClaimDefinitionAsync(ClaimDefinitionViewModel viewModel)
    {
        if (await _context.Set<ClaimDefinition>().AnyAsync(c => c.Value.ToLower() == viewModel.Value.ToLower()))
        {
            return OperationResult<int>.FailureResult(message: "Giá trị Claim đã tồn tại.", errors: new List<string> { "Giá trị Claim đã tồn tại. Vui lòng chọn giá trị khác." });
        }

        ClaimDefinition claimDefinition = _mapper.Map<ClaimDefinition>(viewModel);
        _context.Add(claimDefinition);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created ClaimDefinition: ID={Id}, Type={Type}, Value={Value}", claimDefinition.Id, claimDefinition.Type, claimDefinition.Value);
            return OperationResult<int>.SuccessResult(claimDefinition.Id, $"Thêm định nghĩa quyền hạn '{claimDefinition.Value}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error when creating ClaimDefinition: Value={Value}", viewModel.Value);
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi cơ sở dữ liệu khi thêm định nghĩa quyền hạn.", errors: new List<string> { "Đã xảy ra lỗi cơ sở dữ liệu khi thêm định nghĩa quyền hạn." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unknown error when creating ClaimDefinition.");
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi hệ thống khi thêm định nghĩa quyền hạn.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi thêm định nghĩa quyền hạn." });
        }
    }

    public async Task<OperationResult> UpdateClaimDefinitionAsync(ClaimDefinitionViewModel viewModel)
    {
        if (await _context.Set<ClaimDefinition>().AnyAsync(c => c.Value.ToLower() == viewModel.Value.ToLower() && c.Id != viewModel.Id))
        {
            return OperationResult.FailureResult(message: "Giá trị Claim đã tồn tại.", errors: new List<string> { "Giá trị Claim đã tồn tại. Vui lòng chọn giá trị khác." });
        }

        ClaimDefinition? claimDefinition = await _context.Set<ClaimDefinition>().FirstOrDefaultAsync(c => c.Id == viewModel.Id);
        if (claimDefinition == null)
        {
            _logger.LogWarning("ClaimDefinition not found for update. ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult("Không tìm thấy định nghĩa quyền hạn để cập nhật.");
        }

        _mapper.Map(viewModel, claimDefinition);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated ClaimDefinition: ID={Id}, Type={Type}, Value={Value}", claimDefinition.Id, claimDefinition.Type, claimDefinition.Value);
            return OperationResult.SuccessResult($"Cập nhật định nghĩa quyền hạn '{claimDefinition.Value}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error when updating ClaimDefinition ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi cơ sở dữ liệu khi cập nhật định nghĩa quyền hạn.", errors: new List<string> { "Đã xảy ra lỗi cơ sở dữ liệu khi cập nhật định nghĩa quyền hạn." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unknown error when updating ClaimDefinition ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi hệ thống khi cập nhật định nghĩa quyền hạn.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi cập nhật định nghĩa quyền hạn." });
        }
    }

    public async Task<OperationResult> DeleteClaimDefinitionAsync(int id)
    {
        ClaimDefinition? claimDefinition = await _context.Set<ClaimDefinition>().FindAsync(id);
        if (claimDefinition == null)
        {
            _logger.LogWarning("ClaimDefinition not found for delete. ID: {Id}", id);
            return OperationResult.FailureResult("Không tìm thấy định nghĩa quyền hạn.");
        }

        string claimValue = claimDefinition.Value;
        string claimType = claimDefinition.Type;

        bool isInUseByRoles = await _context.RoleClaims
                                            .AnyAsync(rc => rc.ClaimType == claimType && rc.ClaimValue == claimValue);

        bool isInUseByUsers = await _context.UserClaims
                                            .AnyAsync(uc => uc.ClaimType == claimType && uc.ClaimValue == claimValue);

        if (isInUseByRoles || isInUseByUsers)
        {
            return OperationResult.FailureResult("Không thể xóa định nghĩa quyền hạn vì đang được sử dụng bởi Vai trò hoặc Người dùng.",
                errors: new List<string> { $"Định nghĩa quyền hạn '{claimValue}' đang được gán cho một hoặc nhiều vai trò/người dùng. Vui lòng gỡ bỏ các gán này trước khi xóa." });
        }

        _context.Remove(claimDefinition);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted ClaimDefinition: ID={Id}, Value={Value}", id, claimValue);
            return OperationResult.SuccessResult($"Xóa định nghĩa quyền hạn '{claimValue}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error when deleting ClaimDefinition ID {Id}", id);
            return OperationResult.FailureResult("Lỗi cơ sở dữ liệu khi xóa định nghĩa quyền hạn.", errors: new List<string> { "Lỗi cơ sở dữ liệu khi xóa định nghĩa quyền hạn." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unknown error when deleting ClaimDefinition ID {Id}", id);
            return OperationResult.FailureResult("Đã xảy ra lỗi không mong muốn khi xóa định nghĩa quyền hạn.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi xóa định nghĩa quyền hạn." });
        }
    }

    public async Task<List<ClaimDefinitionViewModel>> GetAllClaimDefinitionsAsync()
    {
        return await _context.Set<ClaimDefinition>()
                             .AsNoTracking()
                             .OrderBy(c => c.Type)
                             .ThenBy(c => c.Value)
                             .ProjectTo<ClaimDefinitionViewModel>(_mapper.ConfigurationProvider)
                             .ToListAsync();
    }

    public async Task<bool> IsClaimDefinitionValueUniqueAsync(int? id, string value)
    {
        if (id.HasValue && id.Value > 0)
        {
            return !await _context.Set<ClaimDefinition>()
                                   .AnyAsync(c => c.Value.ToLower() == value.ToLower() && c.Id != id.Value);
        }
        else
        {
            return !await _context.Set<ClaimDefinition>()
                                   .AnyAsync(c => c.Value.ToLower() == value.ToLower());
        }
    }
}