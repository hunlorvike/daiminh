using AutoMapper;
using domain.Entities;
using web.Areas.Client.ViewModels;

namespace web.Areas.Client.Mappers;

public class BrandClientProfile : Profile
{
    public BrandClientProfile()
    {
        // Ánh xạ cho trang chi tiết thương hiệu
        CreateMap<Brand, BrandDetailViewModel>()
            .ForMember(dest => dest.Products, opt => opt.Ignore()); // Bỏ qua Products vì ta sẽ query riêng có phân trang

        // Ánh xạ cho thẻ sản phẩm (tái sử dụng từ trang chủ)
        CreateMap<Product, ProductCardViewModel>()
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : "Chưa xác định"))
            .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src =>
                src.Images!.OrderByDescending(i => i.IsMain).ThenBy(i => i.OrderIndex).FirstOrDefault()!.ImageUrl ?? "/img/placeholder.svg"));
    }
}