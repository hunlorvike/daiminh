using web.Areas.Client.ViewModels;

namespace web.Areas.Client.Services.Interfaces;

public interface IProductClientService
{
    Task<ProductIndexViewModel> GetProductIndexViewModelAsync(ProductFilterViewModel filter, int pageNumber, int pageSize);
    Task<ProductDetailViewModel?> GetProductDetailBySlugAsync(string slug);
}