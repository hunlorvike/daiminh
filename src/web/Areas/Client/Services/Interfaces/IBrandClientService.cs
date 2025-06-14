using web.Areas.Client.ViewModels;

namespace web.Areas.Client.Services.Interfaces;

public interface IBrandClientService
{
    Task<BrandDetailViewModel?> GetBrandDetailBySlugAsync(string slug, int pageNumber, int pageSize);
}