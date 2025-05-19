using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface INewsletterService
{
    Task<IPagedList<NewsletterListItemViewModel>> GetPagedNewslettersAsync(NewsletterFilterViewModel filter, int pageNumber, int pageSize);

    Task<NewsletterViewModel?> GetNewsletterByIdAsync(int id);

    Task<OperationResult<int>> CreateNewsletterAsync(NewsletterViewModel viewModel, string? ipAddress, string? userAgent);

    Task<OperationResult> UpdateNewsletterAsync(NewsletterViewModel viewModel);

    Task<OperationResult> DeleteNewsletterAsync(int id);

    Task<bool> IsEmailUniqueAsync(string email, int? ignoreId = null);
}
