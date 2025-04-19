using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Brand;
using web.Areas.Admin.ViewModels.Shared; // For SeoViewModel

namespace web.Areas.Admin.Mappers;

public class BrandProfile : Profile
{
    public BrandProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<Brand, BrandListItemViewModel>()
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products != null ? src.Products.Count : 0));

        // --- Mapping for SEO part ---
        // SeoEntity -> SeoViewModel (and reverse)
        // We map specifically from Brand (which is a SeoEntity<int>) to SeoViewModel for the GET case
        CreateMap<Brand, SeoViewModel>();
        // And map from SeoViewModel back to Brand for the POST/PUT case
        CreateMap<SeoViewModel, Brand>()
             // Ignore properties specific to Brand base or other unrelated ones
             .ForMember(dest => dest.Id, opt => opt.Ignore())
             .ForMember(dest => dest.Name, opt => opt.Ignore())
             .ForMember(dest => dest.Slug, opt => opt.Ignore())
             .ForMember(dest => dest.Description, opt => opt.Ignore())
             .ForMember(dest => dest.LogoUrl, opt => opt.Ignore())
             .ForMember(dest => dest.Website, opt => opt.Ignore())
             .ForMember(dest => dest.IsActive, opt => opt.Ignore())
             .ForMember(dest => dest.Products, opt => opt.Ignore())
             .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
             .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());


        // --- Main Mappings for Brand ---

        // Entity -> ViewModel (GET Edit)
        // Map Brand -> BrandViewModel, and its SEO part -> BrandViewModel.Seo
        CreateMap<Brand, BrandViewModel>()
             .ForMember(dest => dest.Seo, opt => opt.MapFrom(src => src)); // AutoMapper uses the Brand -> SeoViewModel map defined above

        // ViewModel -> Entity (POST Create / PUT Edit)
        // Map BrandViewModel -> Brand, including mapping BrandViewModel.Seo -> Brand (SEO properties)
        CreateMap<BrandViewModel, Brand>()
            .ForMember(dest => dest.Products, opt => opt.Ignore()) // Ignore navigation property
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Let base handling/DB handle this
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()) // Let base handling/DB handle this
                                                                    // Use AfterMap to explicitly map the nested SeoViewModel onto the Brand entity
            .AfterMap((src, dest, context) =>
            {
                // Use the existing SeoViewModel -> Brand mapping to update SEO fields
                context.Mapper.Map(src.Seo, dest);
            });
    }
}