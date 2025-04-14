// --- START OF FILE Mappers/ProjectPublicProfile.cs --- Corrected with AfterMap ---
using AutoMapper;
using domain.Entities;
using web.ViewModels.Project;

namespace web.Mappers;

public class ProjectPublicProfile : Profile
{
    public ProjectPublicProfile()
    {
        // --- Mapping for Project List Item ---
        CreateMap<Project, ProjectListItemViewModel>()
            .ForMember(dest => dest.ThumbnailOrFeaturedImageUrl, opt => opt.MapFrom(src => src.ThumbnailImage ?? src.FeaturedImage))
            .ForMember(dest => dest.PrimaryCategoryName, opt => opt.MapFrom(src =>
                src.ProjectCategories != null && src.ProjectCategories.Any()
                    ? src.ProjectCategories
                        .Where(pc => pc.Category != null && pc.Category.IsActive)
                        .OrderBy(pc => pc.Category!.OrderIndex)
                        .ThenBy(pc => pc.Category!.Name)
                        .Select(pc => pc.Category!.Name)
                        .FirstOrDefault()
                    : null
             ))
             .ForMember(dest => dest.PrimaryCategorySlug, opt => opt.MapFrom(src =>
                 src.ProjectCategories != null && src.ProjectCategories.Any()
                    ? src.ProjectCategories
                        .Where(pc => pc.Category != null && pc.Category.IsActive)
                        .OrderBy(pc => pc.Category!.OrderIndex)
                        .ThenBy(pc => pc.Category!.Name)
                        .Select(pc => pc.Category!.Slug)
                        .FirstOrDefault()
                    : null
             ));


        // --- Mapping for Project Detail ---
        CreateMap<Project, ProjectDetailViewModel>()
            // Map basic fields & simple defaults FIRST
            .ForMember(dest => dest.MetaTitle, opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.MetaTitle) ? src.MetaTitle : src.Name))
            .ForMember(dest => dest.OgTitle, opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.OgTitle) ? src.OgTitle : src.Name))
            .ForMember(dest => dest.OgDescription, opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.OgDescription) ? src.OgDescription : src.ShortDescription))
            .ForMember(dest => dest.OgImage, opt => opt.MapFrom(src => src.OgImage ?? src.FeaturedImage))
            // --- Ignore complex collections here - handle in AfterMap ---
            .ForMember(dest => dest.Categories, opt => opt.Ignore())
            .ForMember(dest => dest.Tags, opt => opt.Ignore())
            .ForMember(dest => dest.GalleryImages, opt => opt.Ignore())
            .ForMember(dest => dest.RelatedProducts, opt => opt.Ignore())
            // --- Perform complex collection mapping AFTER basic mapping ---
            .AfterMap((src, dest, context) =>
            {
                // Categories
                dest.Categories = src.ProjectCategories?
                    .Where(pc => pc.Category != null && pc.Category.IsActive)
                    .OrderBy(pc => pc.Category!.OrderIndex)
                    .ThenBy(pc => pc.Category!.Name)
                    .Select(pc => context.Mapper.Map<ProjectCategoryLinkViewModel>(pc.Category)) // Map Category to LinkViewModel
                    .ToList() ?? new List<ProjectCategoryLinkViewModel>();

                // Tags
                dest.Tags = src.ProjectTags?
                    .Where(pt => pt.Tag != null)
                    .OrderBy(pt => pt.Tag!.Name)
                    .Select(pt => context.Mapper.Map<ProjectTagLinkViewModel>(pt.Tag)) // Map Tag to LinkViewModel
                    .ToList() ?? new List<ProjectTagLinkViewModel>();

                // Gallery Images
                dest.GalleryImages = src.Images?
                    .Where(i => !string.IsNullOrEmpty(i.ImageUrl) && i.ImageUrl != src.FeaturedImage) // Filter out featured & empty URLs
                    .OrderBy(i => i.OrderIndex)
                    .Select(i => context.Mapper.Map<ProjectGalleryImageViewModel>(i)) // Map ProjectImage to Gallery VM
                    .ToList() ?? new List<ProjectGalleryImageViewModel>();

                // Related Products
                dest.RelatedProducts = src.ProjectProducts?
                    .Where(pp => pp.Product != null && pp.Product.Status == shared.Enums.PublishStatus.Published) // Filter out null/unpublished products
                    .OrderBy(pp => pp.OrderIndex)
                    .Select(pp => context.Mapper.Map<ProjectProductLinkViewModel>(pp)) // Map ProjectProduct join entity to Link VM
                    .ToList() ?? new List<ProjectProductLinkViewModel>();
            });


        // --- Mapping for related entities to link view models ---
        CreateMap<Category, ProjectCategoryLinkViewModel>();
        CreateMap<Tag, ProjectTagLinkViewModel>();
        CreateMap<ProjectImage, ProjectGalleryImageViewModel>();
        CreateMap<ProjectProduct, ProjectProductLinkViewModel>()
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
             .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Product != null ? src.Product.Slug : string.Empty))
             .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src =>
                 src.Product != null && src.Product.Images != null
                    ? src.Product.Images.Where(i => i.IsMain).Select(i => i.ImageUrl).FirstOrDefault()
                       ?? src.Product.Images.OrderBy(i => i.OrderIndex).Select(i => i.ImageUrl).FirstOrDefault()
                    : null
             ))
             .ForMember(dest => dest.Usage, opt => opt.MapFrom(src => src.Usage)); // Map Usage directly from join table
    }
}
// --- END OF FILE Mappers/ProjectPublicProfile.cs ---