using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoRegister;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Services;

[Register(ServiceLifetime.Scoped)]
public class ContactService : IContactService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ContactService> _logger;

    public ContactService(ApplicationDbContext context, IMapper mapper, ILogger<ContactService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IPagedList<ContactListItemViewModel>> GetPagedContactsAsync(ContactFilterViewModel filter, int pageNumber, int pageSize)
    {
        IQueryable<domain.Entities.Contact> query = _context.Set<domain.Entities.Contact>().AsNoTracking();


        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(c => c.FullName.ToLower().Contains(lowerSearchTerm)
                              || c.Email.ToLower().Contains(lowerSearchTerm)
                              || c.Subject.ToLower().Contains(lowerSearchTerm)
                              || c.Message.ToLower().Contains(lowerSearchTerm));
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(c => c.Status == filter.Status.Value);
        }

        query = query.OrderByDescending(c => c.CreatedAt);

        IPagedList<ContactListItemViewModel> contactsPaged = await query
            .ProjectTo<ContactListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        return contactsPaged;
    }

    public async Task<ContactViewModel?> GetContactByIdAsync(int id)
    {
        domain.Entities.Contact? contact = await _context.Set<domain.Entities.Contact>()
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(c => c.Id == id);

        return _mapper.Map<ContactViewModel>(contact);
    }

    public async Task<OperationResult> UpdateContactDetailsAsync(ContactViewModel viewModel)
    {
        var contact = await _context.Set<domain.Entities.Contact>().FirstOrDefaultAsync(c => c.Id == viewModel.Id);
        if (contact == null)
        {
            _logger.LogWarning("Contact not found for update. ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult("Không tìm thấy liên hệ để cập nhật.");
        }

        bool changed = false;
        if (contact.Status != viewModel.Status)
        {
            contact.Status = viewModel.Status;
            changed = true;
        }

        if (contact.AdminNotes != viewModel.AdminNotes)
        {
            contact.AdminNotes = viewModel.AdminNotes;
            changed = true;
        }

        if (!changed)
        {
            _logger.LogInformation("No changes detected for Contact ID: {Id}", viewModel.Id);
            return OperationResult.SuccessResult("Không có thay đổi nào được lưu.");
        }


        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated Contact details: ID={Id}, NewStatus={NewStatus}, NotesUpdated={NotesUpdated}", contact.Id, contact.Status, contact.AdminNotes != null);
            return OperationResult.SuccessResult("Cập nhật trạng thái và ghi chú thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi cập nhật liên hệ ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi cơ sở dữ liệu khi cập nhật liên hệ.", errors: new List<string> { "Đã xảy ra lỗi cơ sở dữ liệu khi cập nhật liên hệ." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi cập nhật liên hệ ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi hệ thống khi cập nhật liên hệ.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi cập nhật liên hệ." });
        }
    }

    public async Task<OperationResult> DeleteContactAsync(int id)
    {
        var contact = await _context.Set<domain.Entities.Contact>().FindAsync(id);

        if (contact == null)
        {
            _logger.LogWarning("Contact not found for delete. ID: {Id}", id);
            return OperationResult.FailureResult("Không tìm thấy liên hệ.");
        }

        string subject = contact.Subject;

        _context.Remove(contact);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted Contact: ID={Id}, Subject={Subject}", id, subject);
            return OperationResult.SuccessResult($"Xóa liên hệ '{subject}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi xóa liên hệ ID {Id}", id);
            if (ex.InnerException?.Message.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult.FailureResult("Không thể xóa liên hệ vì đang được sử dụng.", errors: new List<string> { "Không thể xóa liên hệ vì đang được sử dụng." });
            }
            return OperationResult.FailureResult("Lỗi cơ sở dữ liệu khi xóa liên hệ.", errors: new List<string> { "Lỗi cơ sở dữ liệu khi xóa liên hệ." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi xóa liên hệ ID {Id}", id);
            return OperationResult.FailureResult("Đã xảy ra lỗi không mong muốn khi xóa liên hệ.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi xóa liên hệ." });
        }
    }

    public async Task RefillContactViewModelFromDbAsync(ContactViewModel viewModel)
    {
        var contact = await _context.Set<domain.Entities.Contact>().AsNoTracking().FirstOrDefaultAsync(c => c.Id == viewModel.Id);
        if (contact != null)
        {
            viewModel.FullName = contact.FullName;
            viewModel.Email = contact.Email;
            viewModel.Phone = contact.Phone;
            viewModel.Subject = contact.Subject;
            viewModel.Message = contact.Message;
            viewModel.CreatedAt = contact.CreatedAt;
            viewModel.IpAddress = contact.IpAddress;
            viewModel.UserAgent = contact.UserAgent;
        }
    }
}
