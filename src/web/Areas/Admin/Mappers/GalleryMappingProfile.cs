using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Gallery;

namespace web.Areas.Admin.Mappers;

public class GalleryMappingProfile : Profile
{
    public GalleryMappingProfile()
    {
        // Gallery Mappings
        CreateMap<Gallery, GalleryListItemViewModel>()
             .ForMember(dest => dest.Categories, opt => opt.MapFrom(src =>
                src.GalleryCategories != null ?
                src.GalleryCategories.Select(gc => gc.Category.Name).ToList() :
                new List<string>()))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src =>
                src.GalleryTags != null ?
                src.GalleryTags.Select(gt => gt.Tag.Name).ToList() :
                new List<string>()))
            .ForMember(dest => dest.ImageCount, opt => opt.MapFrom(src =>
                src.Images != null ? src.Images.Count : 0));

        CreateMap<Gallery, GalleryViewModel>()
            .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src =>
                src.GalleryCategories != null ?
                src.GalleryCategories.Select(gc => gc.CategoryId).ToList() :
                new List<int>()))
            .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src =>
                src.GalleryTags != null ?
                src.GalleryTags.Select(gt => gt.TagId).ToList() :
                new List<int>()))
            .ForMember(dest => dest.AvailableCategories, opt => opt.Ignore()) // Populated separately
            .ForMember(dest => dest.AvailableTags, opt => opt.Ignore()) // Populated separately
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images)) // Map existing images
            .ForMember(dest => dest.CoverImageFile, opt => opt.Ignore()); // Handled by controller

        CreateMap<GalleryViewModel, Gallery>()
            .ForMember(dest => dest.GalleryCategories, opt => opt.Ignore()) // Handled separately
            .ForMember(dest => dest.GalleryTags, opt => opt.Ignore()) // Handled separately
            .ForMember(dest => dest.Images, opt => opt.Ignore()) // Handled separately
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<GalleryImage, GalleryImageViewModel>(); // Simple map for display/edit details
        CreateMap<GalleryImageViewModel, GalleryImage>()
            // Ignore navigation properties and audit fields when mapping from VM to Entity
            .ForMember(dest => dest.Gallery, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}