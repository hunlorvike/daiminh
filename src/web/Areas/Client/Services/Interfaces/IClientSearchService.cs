using web.Areas.Client.ViewModels;

namespace web.Areas.Client.Services.Interfaces;

public interface IClientSearchService
{
    Task<ClientSearchViewModel> SearchAsync(string query);
}