using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Project;

namespace web.Areas.Admin.Mappers;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        // --- List Item ---
        CreateMap<Project, ProjectListItemViewModel>()
             .ForMember(dest => dest.ThumbnailImage, opt => opt.MapFrom(src => src.ThumbnailImage ?? src.FeaturedImage));

        // --- Main View Model ---
        CreateMap<Project, ProjectViewModel>()
            .ForMember(dest => dest.SelectedCategoryIds, opt => opt.MapFrom(src => src.ProjectCategories != null ? src.ProjectCategories.Select(pc => pc.CategoryId).ToList() : new List<int>()))
            .ForMember(dest => dest.SelectedTagIds, opt => opt.MapFrom(src => src.ProjectTags != null ? src.ProjectTags.Select(pt => pt.TagId).ToList() : new List<int>()))
            // Map ProjectProducts Entity to ViewModel
            .ForMember(dest => dest.ProjectProducts, opt => opt.MapFrom<ProjectProductsResolver>())
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images != null ? src.Images.OrderBy(i => i.OrderIndex).ToList() : new List<ProjectImage>()))            // Ignore dropdowns
            .ForMember(dest => dest.CategoryList, opt => opt.Ignore())
            .ForMember(dest => dest.TagList, opt => opt.Ignore())
            .ForMember(dest => dest.ProductList, opt => opt.Ignore())
            .ForMember(dest => dest.StatusList, opt => opt.Ignore())
            .ForMember(dest => dest.PublishStatusList, opt => opt.Ignore());


        // ViewModel -> Entity (Create/Edit)
        CreateMap<ProjectViewModel, Project>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ViewCount, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore()) // Manual
            .ForMember(dest => dest.ProjectCategories, opt => opt.Ignore()) // Manual
            .ForMember(dest => dest.ProjectTags, opt => opt.Ignore()) // Manual
            .ForMember(dest => dest.ProjectProducts, opt => opt.Ignore()) // Manual
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());


        // --- Sub-ViewModels ---
        CreateMap<ProjectImage, ProjectImageViewModel>();
        CreateMap<ProjectImageViewModel, ProjectImage>()
           .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(dest => dest.Project, opt => opt.Ignore())
           .ForMember(dest => dest.ProjectId, opt => opt.Ignore()); // Set by Controller

        // Mapping from ProjectProductViewModel back to ProjectProduct Entity
        // We only need ProductId and Usage from the VM for saving
        CreateMap<ProjectProductViewModel, ProjectProduct>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore ID
            .ForMember(dest => dest.Project, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.ProjectId, opt => opt.Ignore()) // Set manually
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Usage, opt => opt.MapFrom(src => src.Usage))
            .ForMember(dest => dest.OrderIndex, opt => opt.MapFrom(src => src.OrderIndex))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Ignore base fields
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());

    }
}

public class ProjectProductsResolver : IValueResolver<Project, ProjectViewModel, List<ProjectProductViewModel>>
{
    public List<ProjectProductViewModel> Resolve(Project source, ProjectViewModel destination, List<ProjectProductViewModel> destMember, ResolutionContext context)
    {
        if (source.ProjectProducts == null)
            return new List<ProjectProductViewModel>();

        return source.ProjectProducts.Select(pp => new ProjectProductViewModel
        {
            ProductId = pp.ProductId,
            ProductName = pp.Product != null ? pp.Product.Name : "N/A",
            ProductImageUrl = GetProductImageUrl(pp.Product),
            Usage = pp.Usage,
            OrderIndex = pp.OrderIndex,
            IsDeleted = false
        }).OrderBy(pvm => pvm.OrderIndex).ToList();
    }

    private string? GetProductImageUrl(Product product)
    {
        if (product == null || product.Images == null)
            return null;

        var mainImage = product.Images.FirstOrDefault(i => i.IsMain);
        if (mainImage != null)
            return mainImage.ThumbnailUrl;

        var firstImage = product.Images.FirstOrDefault();
        return firstImage?.ThumbnailUrl;
    }
}