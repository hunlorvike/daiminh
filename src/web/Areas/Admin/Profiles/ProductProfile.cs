using AutoMapper;
using domain.Entities;
using web.Areas.Admin.Requests.Product;

namespace web.Areas.Admin.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        // Product -> ProductCreateRequest
        CreateMap<Product, ProductCreateRequest>()
            .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src =>
                src.ProductCategories != null ? src.ProductCategories.Select(pc => pc.CategoryId).ToList() : new List<int>()))
            .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src =>
                src.ProductTags != null ? src.ProductTags.Select(pt => pt.TagId).ToList() : new List<int>()))
            .ForMember(dest => dest.FieldValues, opt => opt.MapFrom(src =>
                src.FieldValues != null ? src.FieldValues.Select(fv => new ProductFieldValueRequest
                {
                    FieldId = fv.FieldId,
                    Value = fv.Value ?? string.Empty
                }).ToList() : new List<ProductFieldValueRequest>()))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src =>
                src.Images != null ? src.Images.Select(img => new ProductImageRequest
                {
                    ImageUrl = img.ImageUrl ?? string.Empty,
                    AltText = img.AltText ?? string.Empty,
                    IsPrimary = img.IsPrimary,
                    DisplayOrder = img.DisplayOrder
                }).ToList() : new List<ProductImageRequest>()));

        // ProductCreateRequest -> Product
        CreateMap<ProductCreateRequest, Product>()
            .ForMember(dest => dest.ProductCategories, opt => opt.Ignore())
            .ForMember(dest => dest.ProductTags, opt => opt.Ignore())
            .ForMember(dest => dest.FieldValues, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore());

        // Product -> ProductUpdateRequest
        CreateMap<Product, ProductUpdateRequest>()
            .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src =>
                src.ProductCategories != null ? src.ProductCategories.Select(pc => pc.CategoryId).ToList() : new List<int>()))
            .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src =>
                src.ProductTags != null ? src.ProductTags.Select(pt => pt.TagId).ToList() : new List<int>()))
            .ForMember(dest => dest.FieldValues, opt => opt.MapFrom(src =>
                src.FieldValues != null ? src.FieldValues.Select(fv => new ProductFieldValueRequest
                {
                    FieldId = fv.FieldId,
                    Value = fv.Value ?? string.Empty
                }).ToList() : new List<ProductFieldValueRequest>()))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src =>
                src.Images != null ? src.Images.Select(img => new ProductImageRequest
                {
                    ImageUrl = img.ImageUrl ?? string.Empty,
                    AltText = img.AltText ?? string.Empty,
                    IsPrimary = img.IsPrimary,
                    DisplayOrder = img.DisplayOrder
                }).ToList() : new List<ProductImageRequest>()));

        // ProductUpdateRequest -> Product
        CreateMap<ProductUpdateRequest, Product>()
            .ForMember(dest => dest.ProductCategories, opt => opt.Ignore())
            .ForMember(dest => dest.ProductTags, opt => opt.Ignore())
            .ForMember(dest => dest.FieldValues, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore());

        // Product -> ProductDeleteRequest
        CreateMap<Product, ProductDeleteRequest>();
    }
}
