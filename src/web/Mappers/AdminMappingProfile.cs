using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Category;
using web.Areas.Admin.ViewModels.Comment;
using web.Areas.Admin.ViewModels.FAQ;
using web.Areas.Admin.ViewModels.Media;
using web.Areas.Admin.ViewModels.Product;
using web.Areas.Admin.ViewModels.ProductType;
using web.Areas.Admin.ViewModels.Redirect;
using web.Areas.Admin.ViewModels.Seo;
using web.Areas.Admin.ViewModels.Tag;
using web.Areas.Admin.ViewModels.Testimonial;
using web.Areas.Admin.ViewModels.User;

namespace web.Mappers;

public class AdminMappingProfile : Profile
{
    public AdminMappingProfile()
    {
        // FAQ mappings
        CreateMap<FAQ, FAQListItemViewModel>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));

        CreateMap<FAQ, FAQViewModel>().ReverseMap();

        // FAQ Category mappings
        CreateMap<FAQCategory, FAQCategoryListItemViewModel>()
            .ForMember(dest => dest.FAQCount, opt => opt.MapFrom(src => src.FAQs != null ? src.FAQs.Count : 0));

        CreateMap<FAQCategory, FAQCategoryViewModel>().ReverseMap();

        // Category mappings
        CreateMap<Category, CategoryListItemViewModel>()
            .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src => src.Parent != null ? src.Parent.Name : null))
            .ForMember(dest => dest.ChildrenCount, opt => opt.MapFrom(src => src.Children != null ? src.Children.Count : 0))
            .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src =>
                (src.Type == shared.Enums.CategoryType.Product ? (src.ProductCategories != null ? src.ProductCategories.Count : 0) : 0) +
                (src.Type == shared.Enums.CategoryType.Article ? (src.ArticleCategories != null ? src.ArticleCategories.Count : 0) : 0) +
                (src.Type == shared.Enums.CategoryType.Project ? (src.ProjectCategories != null ? src.ProjectCategories.Count : 0) : 0) +
                (src.Type == shared.Enums.CategoryType.Gallery ? (src.GalleryCategories != null ? src.GalleryCategories.Count : 0) : 0)
            ));

        CreateMap<Category, CategoryViewModel>()
            .ReverseMap()
            .ForMember(dest => dest.ImageUrl, opt => opt.Condition(src => src.ImageFile == null));

        CreateMap<Category, CategorySelectViewModel>()
            .ForMember(dest => dest.Level, opt => opt.Ignore());

        // Tag mappings
        CreateMap<Tag, TagListItemViewModel>()
            .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src =>
                (src.Type == shared.Enums.TagType.Product ? (src.ProductTags != null ? src.ProductTags.Count : 0) : 0) +
                (src.Type == shared.Enums.TagType.Article ? (src.ArticleTags != null ? src.ArticleTags.Count : 0) : 0) +
                (src.Type == shared.Enums.TagType.Project ? (src.ProjectTags != null ? src.ProjectTags.Count : 0) : 0) +
                (src.Type == shared.Enums.TagType.Gallery ? (src.GalleryTags != null ? src.GalleryTags.Count : 0) : 0)
            ));

        CreateMap<Tag, TagViewModel>().ReverseMap();

        // Testimonial mappings
        CreateMap<Testimonial, TestimonialListItemViewModel>();

        CreateMap<Testimonial, TestimonialViewModel>()
            .ReverseMap()
            .ForMember(dest => dest.ClientAvatar, opt => opt.Condition(src => src.AvatarFile == null));

        // User mappings
        CreateMap<User, UserListItemViewModel>();

        CreateMap<User, UserViewModel>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore());

        CreateMap<UserViewModel, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        // MediaFolder mappings
        CreateMap<MediaFolder, MediaFolderListItemViewModel>()
            .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src => src.Parent != null ? src.Parent.Name : null))
            .ForMember(dest => dest.FilesCount, opt => opt.MapFrom(src => src.Files != null ? src.Files.Count : 0))
            .ForMember(dest => dest.SubFoldersCount, opt => opt.MapFrom(src => src.Children != null ? src.Children.Count : 0));

        CreateMap<MediaFolder, MediaFolderViewModel>().ReverseMap();

        CreateMap<MediaFolder, MediaFolderSelectViewModel>()
            .ForMember(dest => dest.Level, opt => opt.Ignore());

        // MediaFile mappings
        CreateMap<MediaFile, MediaFileListItemViewModel>()
            .ForMember(dest => dest.FolderName, opt => opt.MapFrom(src => src.MediaFolder != null ? src.MediaFolder.Name : null));

        CreateMap<MediaFile, MediaFileViewModel>().ReverseMap()
            .ForMember(dest => dest.FilePath, opt => opt.Condition(src => src.FileUpload == null))
            .ForMember(dest => dest.ThumbnailPath, opt => opt.Condition(src => src.FileUpload == null))
            .ForMember(dest => dest.MediumSizePath, opt => opt.Condition(src => src.FileUpload == null))
            .ForMember(dest => dest.LargeSizePath, opt => opt.Condition(src => src.FileUpload == null));

        // Redirect mappings
        CreateMap<Redirect, RedirectListItemViewModel>();
        CreateMap<Redirect, RedirectViewModel>().ReverseMap();

        // SeoSettings mappings
        CreateMap<SeoSettings, SeoSettingsViewModel>();
        CreateMap<SeoSettingsViewModel, SeoSettings>();
        CreateMap<SeoSettings, SeoSettingsListItemViewModel>();

        // SeoAnalytics mappings
        CreateMap<SeoAnalytics, SeoAnalyticsViewModel>()
            .ForMember(dest => dest.TopKeywords, opt => opt.MapFrom(src => src.TopKeywords ?? "[]"));

        CreateMap<SeoAnalyticsViewModel, SeoAnalytics>()
            .ForMember(dest => dest.TopKeywords, opt => opt.MapFrom(src => src.TopKeywords ?? "[]"));

        CreateMap<SeoAnalytics, SeoAnalyticsListItemViewModel>();

        // Product mappings
        CreateMap<Product, ProductListItemViewModel>()
            .ForMember(dest => dest.ProductTypeName, opt => opt.MapFrom(src => src.ProductType != null ? src.ProductType.Name : string.Empty))
            .ForMember(dest => dest.CategoryCount, opt => opt.MapFrom(src => src.ProductCategories != null ? src.ProductCategories.Count : 0))
            .ForMember(dest => dest.TagCount, opt => opt.MapFrom(src => src.ProductTags != null ? src.ProductTags.Count : 0))
            .ForMember(dest => dest.ImageCount, opt => opt.MapFrom(src => src.Images != null ? src.Images.Count : 0))
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
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
            .ForMember(dest => dest.ImageFiles, opt => opt.Ignore());

        CreateMap<ProductViewModel, Product>()
            .ForMember(dest => dest.ProductCategories, opt => opt.Ignore())
            .ForMember(dest => dest.ProductTags, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore());

        CreateMap<ProductType, ProductTypeViewModel>().ReverseMap();
        CreateMap<ProductType, ProductTypeListItemViewModel>();

        CreateMap<ProductImage, ProductImageViewModel>().ReverseMap();

        // Comment mappings
        CreateMap<Comment, CommentListItemViewModel>()
            .ForMember(dest => dest.ArticleTitle, opt => opt.MapFrom(src => src.Article != null ? src.Article.Title : ""))
            .ForMember(dest => dest.ArticleSlug, opt => opt.MapFrom(src => src.Article != null ? src.Article.Slug : ""))
            .ForMember(dest => dest.ReplyCount, opt => opt.Ignore());

        CreateMap<Comment, CommentViewModel>()
            .ForMember(dest => dest.ArticleTitle, opt => opt.Ignore())
            .ForMember(dest => dest.ParentAuthorName, opt => opt.Ignore());

        CreateMap<CommentViewModel, Comment>();
    }
}
