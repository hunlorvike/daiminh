using AutoMapper;
using domain.Entities;
using shared.Models;
using web.Areas.Admin.ViewModels.Article;
using web.Areas.Admin.ViewModels.Brand;
using web.Areas.Admin.ViewModels.Category;
using web.Areas.Admin.ViewModels.Comment;
using web.Areas.Admin.ViewModels.Contact;
using web.Areas.Admin.ViewModels.FAQ;
using web.Areas.Admin.ViewModels.Gallery;
using web.Areas.Admin.ViewModels.Media;
using web.Areas.Admin.ViewModels.Newsletter;
using web.Areas.Admin.ViewModels.Product;
using web.Areas.Admin.ViewModels.ProductType;
using web.Areas.Admin.ViewModels.Setting;
using web.Areas.Admin.ViewModels.Tag;
using web.Areas.Admin.ViewModels.Testimonial;
using web.Areas.Admin.ViewModels.User;
using web.Mappers.Resolvers;

namespace web.Mappers;

public class AdminMappingProfile : Profile
{
    public AdminMappingProfile()
    {
        // =========================================
        // User Mappings
        // =========================================
        CreateMap<User, UserListItemViewModel>();

        CreateMap<User, UserViewModel>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore());

        CreateMap<UserViewModel, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Assume PasswordHash is set separately

        // =========================================
        // Category Mappings (Generic)
        // =========================================
        CreateMap<Category, CategoryListItemViewModel>()
            .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src => src.Parent != null ? src.Parent.Name : null))
            .ForMember(dest => dest.ChildrenCount, opt => opt.MapFrom(src => src.Children != null ? src.Children.Count : 0))
            // Calculate ItemCount based on all possible related collections
            .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src =>
                (src.ProductCategories != null ? src.ProductCategories.Count : 0) +
                (src.ArticleCategories != null ? src.ArticleCategories.Count : 0) +
                (src.ProjectCategories != null ? src.ProjectCategories.Count : 0) + // Assuming ProjectCategories exists
                (src.GalleryCategories != null ? src.GalleryCategories.Count : 0)
            ));

        CreateMap<Category, CategoryViewModel>() // Added Parent Name mapping
             .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.Parent != null ? src.Parent.Name : null))
             .ReverseMap(); // Keep ReverseMap if needed for saving

        CreateMap<Category, CategorySelectViewModel>() // Renamed from CategoryParentViewModel for clarity
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
             .ForMember(dest => dest.Level, opt => opt.Ignore()); // Level usually set during recursive fetch

        // =========================================
        // Tag Mappings (Generic)
        // =========================================
        CreateMap<Tag, TagListItemViewModel>()
            // Calculate ItemCount based on all possible related collections
            .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src =>
                (src.ProductTags != null ? src.ProductTags.Count : 0) +
                (src.ArticleTags != null ? src.ArticleTags.Count : 0) +
                (src.ProjectTags != null ? src.ProjectTags.Count : 0) + // Assuming ProjectTags exists
                (src.GalleryTags != null ? src.GalleryTags.Count : 0)
            ));

        CreateMap<Tag, TagViewModel>().ReverseMap();

        // =========================================
        // Setting Mappings (Generic)
        // =========================================
        CreateMap<Setting, SettingViewModel>();

        // Map from ViewModel back to Entity (ONLY updateable fields)
        CreateMap<SettingViewModel, Setting>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            // Ignore all other properties during the update mapping
            .ForMember(dest => dest.Key, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Type, opt => opt.Ignore())
            .ForMember(dest => dest.Description, opt => opt.Ignore())
            .ForMember(dest => dest.DefaultValue, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // =========================================
        // Article Mappings
        // =========================================
        CreateMap<Article, ArticleListItemViewModel>()
            // Map Categories and Tags to lists of names for display
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src =>
                src.ArticleCategories != null ?
                src.ArticleCategories.Select(ac => ac.Category.Name).ToList() :
                new List<string>()))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src =>
                src.ArticleTags != null ?
                src.ArticleTags.Select(at => at.Tag.Name).ToList() :
                new List<string>()))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src =>
                 src.Comments != null ? src.Comments.Count(c => c.IsApproved) : 0)); // Count approved comments

        CreateMap<Article, ArticleViewModel>()
            .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src =>
                src.ArticleCategories != null ?
                src.ArticleCategories.Select(ac => ac.CategoryId).ToList() :
                new List<int>()))
            .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src =>
                src.ArticleTags != null ?
                src.ArticleTags.Select(at => at.TagId).ToList() :
                new List<int>()))
            .ForMember(dest => dest.ProductIds, opt => opt.MapFrom(src =>
                src.ArticleProducts != null ?
                src.ArticleProducts.Select(ap => ap.ProductId).ToList() :
                new List<int>()))
             .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src =>
                 src.Comments != null ? src.Comments.Count : 0)) // Total comments for info
            .ForMember(dest => dest.AvailableCategories, opt => opt.Ignore()) // Populated separately
            .ForMember(dest => dest.AvailableTags, opt => opt.Ignore()) // Populated separately
            .ForMember(dest => dest.AvailableProducts, opt => opt.Ignore()) // Populated separately
            .ForMember(dest => dest.FeaturedImageFile, opt => opt.Ignore()) // Handled by controller
            .ForMember(dest => dest.ThumbnailImageFile, opt => opt.Ignore()); // Handled by controller

        CreateMap<ArticleViewModel, Article>()
            .ForMember(dest => dest.ArticleCategories, opt => opt.Ignore()) // Handled separately based on CategoryIds
            .ForMember(dest => dest.ArticleTags, opt => opt.Ignore()) // Handled separately based on TagIds
            .ForMember(dest => dest.ArticleProducts, opt => opt.Ignore()) // Handled separately based on ProductIds
            .ForMember(dest => dest.Comments, opt => opt.Ignore()) // Comments are not managed via ArticleViewModel
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Managed by DB/BaseEntity
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()); // Managed by DB/BaseEntity

        // =========================================
        // Comment Mappings
        // =========================================
        CreateMap<Comment, CommentListItemViewModel>()
            .ForMember(dest => dest.ArticleTitle, opt => opt.MapFrom(src => src.Article != null ? src.Article.Title : ""))
            .ForMember(dest => dest.ArticleSlug, opt => opt.MapFrom(src => src.Article != null ? src.Article.Slug : ""))
            // Calculate ReplyCount in the controller/service where needed using a query
            .ForMember(dest => dest.ReplyCount, opt => opt.Ignore());

        CreateMap<Comment, CommentViewModel>()
            .ForMember(dest => dest.ArticleTitle, opt => opt.MapFrom(src => src.Article != null ? src.Article.Title : null)) // Include ArticleTitle
            .ForMember(dest => dest.ParentAuthorName, opt => opt.MapFrom(src => src.Parent != null ? src.Parent.AuthorName : null)); // Map parent author name

        CreateMap<CommentViewModel, Comment>(); // Simple reverse map

        // =========================================
        // Product Mappings
        // =========================================
        CreateMap<ProductType, ProductTypeViewModel>();
        CreateMap<ProductType, ProductTypeListItemViewModel>()
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src =>
                src.Products != null ? src.Products.Count : 0));
        CreateMap<ProductTypeViewModel, ProductType>()
            // Map only the fields that should be updated from the ViewModel
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => src.Icon))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            // Ignore fields that should not be updated from this ViewModel
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());


        CreateMap<Product, ProductListItemViewModel>()
            .ForMember(dest => dest.ProductTypeName, opt => opt.MapFrom(src => src.ProductType != null ? src.ProductType.Name : string.Empty))
            .ForMember(dest => dest.CategoryCount, opt => opt.MapFrom(src => src.ProductCategories != null ? src.ProductCategories.Count : 0))
            .ForMember(dest => dest.TagCount, opt => opt.MapFrom(src => src.ProductTags != null ? src.ProductTags.Count : 0))
            .ForMember(dest => dest.ImageCount, opt => opt.MapFrom(src => src.Images != null ? src.Images.Count : 0))
            // Get main image URL or first image URL
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src =>
                src.Images != null && src.Images.Any(i => i.IsMain) ?
                src.Images.First(i => i.IsMain).ImageUrl :
                (src.Images != null && src.Images.Any() ? src.Images.First().ImageUrl : null)));

        CreateMap<Product, ProductViewModel>()
            .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src =>
                src.ProductCategories != null ?
                src.ProductCategories.Select(pc => pc.CategoryId).ToList() :
                new List<int>()))
            .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src =>
                src.ProductTags != null ?
                src.ProductTags.Select(pt => pt.TagId).ToList() :
                new List<int>()))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images)) // Map existing images
            .ForMember(dest => dest.ImageFiles, opt => opt.Ignore()); // Handled by controller

        CreateMap<ProductViewModel, Product>()
            .ForMember(dest => dest.ProductCategories, opt => opt.Ignore()) // Handled separately
            .ForMember(dest => dest.ProductTags, opt => opt.Ignore()) // Handled separately
            .ForMember(dest => dest.Images, opt => opt.Ignore()); // Handled separately

        CreateMap<ProductImage, ProductImageViewModel>().ReverseMap();

        // =========================================
        // Gallery Mappings
        // =========================================
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

        // =========================================
        // FAQ Mappings
        // =========================================
        CreateMap<FAQCategory, FAQCategoryListItemViewModel>()
            .ForMember(dest => dest.FAQCount, opt => opt.MapFrom(src => src.FAQs != null ? src.FAQs.Count : 0));

        CreateMap<FAQCategory, FAQCategoryViewModel>().ReverseMap();

        CreateMap<FAQ, FAQListItemViewModel>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));

        CreateMap<FAQ, FAQViewModel>().ReverseMap();

        // =========================================
        // Testimonial Mappings
        // =========================================
        CreateMap<Testimonial, TestimonialListItemViewModel>();

        // Entity -> ViewModel (For Edit GET)
        CreateMap<Testimonial, TestimonialViewModel>();

        // ViewModel -> Entity (For Create/Edit POST)
        CreateMap<TestimonialViewModel, Testimonial>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore Id and Nav properties
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // =========================================
        // Contact Mappings
        // =========================================
        CreateMap<Contact, ContactListItemViewModel>();
        CreateMap<Contact, ContactViewModel>().ReverseMap();

        // =========================================
        // Newsletter Mappings
        // =========================================
        CreateMap<Newsletter, NewsletterListItemViewModel>();
        CreateMap<Newsletter, NewsletterViewModel>().ReverseMap();

        // =========================================
        // Media Mappings
        // =========================================
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
            .ForMember(dest => dest.MediaType, opt => opt.MapFrom(src => src.MediaType))
            // Map ThumbnailUrl using the injected Minio service
            .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom<MediaItemThumbnailResolver>()); // Use custom resolver

        // Map upload result + folder ID -> MediaFile entity
        CreateMap<MinioUploadResult, MediaFile>()
           .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.GeneratedFileName)) // Store unique generated name
           .ForMember(dest => dest.OriginalFileName, opt => opt.MapFrom(src => src.OriginalFileName))
           .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.ContentType))
           .ForMember(dest => dest.FileExtension, opt => opt.MapFrom(src => src.FileExtension))
           .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.ObjectName)) // Store full path in bucket
           .ForMember(dest => dest.FileSize, opt => opt.MapFrom(src => src.FileSize))
           .ForMember(dest => dest.MediaType, opt => opt.MapFrom<MediaTypeResolver>()) // Resolve MediaType
                                                                                       // Fields not in MinioUploadResult need setting manually or ignoring
           .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(dest => dest.ThumbnailPath, opt => opt.MapFrom(src => src.ObjectName)) // Initially, use main path for thumb
           .ForMember(dest => dest.MediumSizePath, opt => opt.Ignore()) // Ignore initially
           .ForMember(dest => dest.LargeSizePath, opt => opt.Ignore()) // Ignore initially
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
        CreateMap<MediaFile, MediaFileEditViewModel>()
             .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom<MediaEditThumbnailResolver>());// Use custom resolver

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

        // Brand Mappings
        CreateMap<Brand, BrandListItemViewModel>()
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products != null ? src.Products.Count : 0));

        CreateMap<Brand, BrandViewModel>().ReverseMap();

    }
}