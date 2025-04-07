using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Product;

namespace web.Areas.Admin.Mappers;

public class ProductImageProfile : Profile
{
    public ProductImageProfile()
    {
        // Entity -> ViewModel
        CreateMap<ProductImage, ProductImageViewModel>()
            // Optional: Use resolver for ThumbnailUrl if it's different from ImageUrl
            // .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom<ProductImageThumbnailResolver>())
            ;


        // ViewModel -> Entity
        CreateMap<ProductImageViewModel, ProductImage>()
           .ForMember(dest => dest.Id, opt => opt.Ignore()) // Let DB handle ID generation for new ones
           .ForMember(dest => dest.Product, opt => opt.Ignore())
           .ForMember(dest => dest.ProductId, opt => opt.Ignore()); // Set by Controller
    }
}

// Optional Resolver if Thumbnail is different (e.g., resized version)
// public class ProductImageThumbnailResolver : IValueResolver<ProductImage, ProductImageViewModel, string?>
// {
//     private readonly IMinioStorageService _minioService;
//     public ProductImageThumbnailResolver(IMinioStorageService minioService){_minioService = minioService;}
//     public string? Resolve(ProductImage source, ProductImageViewModel dest, string? member, ResolutionContext ctx)
//     {
//         // Use ThumbnailUrl if available, otherwise fall back to ImageUrl
//         return _minioService.GetMinioUrl(source.ThumbnailUrl ?? source.ImageUrl);
//     }
// }