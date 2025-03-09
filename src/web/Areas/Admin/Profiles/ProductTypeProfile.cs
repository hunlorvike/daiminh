using AutoMapper;
using domain.Entities;
using web.Areas.Admin.Models.ProductType;
using web.Areas.Admin.Requests.ProductType;

namespace web.Areas.Admin.Profiles;

public class ProductTypeProfile : Profile
{
    public ProductTypeProfile()
    {
        CreateMap<ProductType, ProductTypeViewModel>();
        CreateMap<ProductTypeCreateRequest, ProductType>();
        CreateMap<ProductType, ProductTypeUpdateRequest>().ReverseMap();
        CreateMap<ProductType, ProductTypeDeleteRequest>().ReverseMap();
    }
}