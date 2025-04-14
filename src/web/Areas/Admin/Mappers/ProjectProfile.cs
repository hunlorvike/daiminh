// --- START OF FILE ProjectProfile.cs --- New File ---
using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Project;
namespace web.Areas.Admin.Mappers;
public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<Project, ProjectListItemViewModel>()
            .ForMember(dest => dest.ThumbnailImage, opt => opt.MapFrom(src => src.ThumbnailImage ?? src.FeaturedImage))
             .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt ?? src.CreatedAt));

        // Entity -> ViewModel
        CreateMap<Project, ProjectViewModel>()
            .ForMember(dest => dest.SelectedCategoryIds, opt => opt.MapFrom(src => src.ProjectCategories != null ? src.ProjectCategories.Select(pc => pc.CategoryId).ToList() : new List<int>()))
            .ForMember(dest => dest.SelectedTagIds, opt => opt.MapFrom(src => src.ProjectTags != null ? src.ProjectTags.Select(pt => pt.TagId).ToList() : new List<int>()))
            .ForMember(dest => dest.SelectedProductIds, opt => opt.MapFrom(src => src.ProjectProducts != null ? src.ProjectProducts.Select(pp => pp.ProductId).ToList() : new List<int>()))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images)) // Map images using ProjectImageProfile
                                                                                   // Ignore Dropdowns
            .ForMember(dest => dest.CategoryList, opt => opt.Ignore())
            .ForMember(dest => dest.TagList, opt => opt.Ignore())
            .ForMember(dest => dest.ProductList, opt => opt.Ignore())
            .ForMember(dest => dest.StatusList, opt => opt.Ignore())
            .ForMember(dest => dest.PublishStatusList, opt => opt.Ignore());

        // ViewModel -> Entity
        CreateMap<ProjectViewModel, Project>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src =>
                src.StartDate.HasValue ? src.StartDate.Value.ToUniversalTime() : (DateTime?)null))
            .ForMember(dest => dest.CompletionDate, opt => opt.MapFrom(src =>
                src.CompletionDate.HasValue ? src.CompletionDate.Value.ToUniversalTime() : (DateTime?)null))
            .ForMember(dest => dest.ViewCount, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore()) // Manual handling
            .ForMember(dest => dest.ProjectCategories, opt => opt.Ignore()) // Manual handling
            .ForMember(dest => dest.ProjectTags, opt => opt.Ignore()) // Manual handling
            .ForMember(dest => dest.ProjectProducts, opt => opt.Ignore()) // Manual handling
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
    }
}
// --- END OF FILE ProjectProfile.cs ---