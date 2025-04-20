using AutoMapper;
using domain.Entities;
using shared.Models;
using web.Areas.Admin.ViewModels.Media;

namespace web.Areas.Admin.Mappers;

public class MediaProfile : Profile
{
    public MediaProfile()
    {
        // Media Mappings
        CreateMap<MediaFolder, MediaItemViewModel>()
          .ForMember(dest => dest.IsFolder, opt => opt.MapFrom(src => true))
          .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
          .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
          .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentId))
          .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
          // File specific properties are ignored (will be null)
          .ForMember(dest => dest.ThumbnailUrl, opt => opt.Ignore())
          .ForMember(dest => dest.FilePath, opt => opt.Ignore())
          .ForMember(dest => dest.MimeType, opt => opt.Ignore())
          .ForMember(dest => dest.FileSize, opt => opt.Ignore())
          .ForMember(dest => dest.AltText, opt => opt.Ignore())
          .ForMember(dest => dest.MediaType, opt => opt.Ignore());

        CreateMap<MediaFolderViewModel, MediaFolder>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Don't map ID on create/update from VM
            .ForMember(dest => dest.Parent, opt => opt.Ignore())
            .ForMember(dest => dest.Children, opt => opt.Ignore())
            .ForMember(dest => dest.Files, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<MediaFolder, BreadcrumbItemViewModel>();

        CreateMap<MediaFile, MediaItemViewModel>()
            .ForMember(dest => dest.IsFolder, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.OriginalFileName)) // Show original name in UI
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ParentId, opt => opt.Ignore()) // Files don't have parent folder ID in this specific VM item
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.FilePath)) // Store the MinIO ObjectName/path
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.MimeType))
            .ForMember(dest => dest.FileSize, opt => opt.MapFrom(src => src.FileSize))
            .ForMember(dest => dest.AltText, opt => opt.MapFrom(src => src.AltText))
            .ForMember(dest => dest.MediaType, opt => opt.MapFrom(src => src.MediaType));

        // Map upload result + folder ID -> MediaFile entity
        CreateMap<MinioUploadResult, MediaFile>()
           .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.GeneratedFileName)) // Store unique generated name
           .ForMember(dest => dest.OriginalFileName, opt => opt.MapFrom(src => src.OriginalFileName))
           .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.ContentType))
           .ForMember(dest => dest.FileExtension, opt => opt.MapFrom(src => src.FileExtension))
           .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.ObjectName)) // Store full path in bucket
           .ForMember(dest => dest.FileSize, opt => opt.MapFrom(src => src.FileSize))
           .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(dest => dest.Description, opt => opt.Ignore())
           .ForMember(dest => dest.AltText, opt => opt.MapFrom(src => Path.GetFileNameWithoutExtension(src.OriginalFileName))) // Default AltText
           .ForMember(dest => dest.Width, opt => opt.Ignore()) // Ignore initially
           .ForMember(dest => dest.Height, opt => opt.Ignore()) // Ignore initially
           .ForMember(dest => dest.Duration, opt => opt.Ignore()) // Ignore initially
           .ForMember(dest => dest.FolderId, opt => opt.Ignore()) // Set manually in controller
           .ForMember(dest => dest.MediaFolder, opt => opt.Ignore())
           .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
           .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // Map for editing file details
        CreateMap<MediaFile, MediaFileEditViewModel>();

        CreateMap<MediaFileEditViewModel, MediaFile>()
            .ForMember(dest => dest.AltText, opt => opt.MapFrom(src => src.AltText))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
             // Ignore all other fields - only update AltText and Description
             .ForAllMembers(opts =>
             {
                 if (opts.DestinationMember.Name != nameof(MediaFile.AltText) &&
                     opts.DestinationMember.Name != nameof(MediaFile.Description) &&
                     opts.DestinationMember.Name != nameof(MediaFile.UpdatedAt)) // Allow UpdatedAt to be set by SaveChanges logic
                 {
                     opts.Ignore();
                 }
             });
    }
}