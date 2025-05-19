using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Mappers;

public class ProductReviewProfile : Profile
{
    public ProductReviewProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<ProductReview, ProductReviewListItemViewModel>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : "N/A"))
            .ForMember(dest => dest.ContentSummary, opt => opt.MapFrom(src => src.Content.Length > 100 ? src.Content.Substring(0, 100) + "..." : src.Content));

        // Entity -> ViewModel (GET Edit)
        CreateMap<ProductReview, ProductReviewViewModel>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : "N/A"))
            .ForMember(dest => dest.StatusOptions, opt => opt.Ignore());

        // ViewModel -> Entity (POST Edit)
        CreateMap<ProductReviewViewModel, ProductReview>()
            .ForMember(dest => dest.ProductId, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.UserName, opt => opt.Ignore())
            .ForMember(dest => dest.UserEmail, opt => opt.Ignore())
            .ForMember(dest => dest.Rating, opt => opt.Ignore())
            .ForMember(dest => dest.Content, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
    }
}