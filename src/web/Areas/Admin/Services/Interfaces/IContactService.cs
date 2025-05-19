using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface IContactService
{
    Task<IPagedList<ContactListItemViewModel>> GetPagedContactsAsync(ContactFilterViewModel filter, int pageNumber, int pageSize);

    Task<ContactViewModel?> GetContactByIdAsync(int id);

    Task<OperationResult> UpdateContactDetailsAsync(ContactViewModel viewModel);

    Task<OperationResult> DeleteContactAsync(int id);
    Task RefillContactViewModelFromDbAsync(ContactViewModel viewModel);
}
