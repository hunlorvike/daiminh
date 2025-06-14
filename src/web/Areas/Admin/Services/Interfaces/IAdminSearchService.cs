using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Services.Interfaces;

public interface IAdminSearchService
{
    Task<List<AdminSearchResultItemViewModel>> SearchAsync(string query);
}