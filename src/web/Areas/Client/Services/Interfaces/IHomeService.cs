using web.Areas.Client.ViewModels;

namespace web.Areas.Client.Services.Interfaces;

public interface IHomeService
{
    Task<HomeViewModel> GetHomeViewModelAsync();
}