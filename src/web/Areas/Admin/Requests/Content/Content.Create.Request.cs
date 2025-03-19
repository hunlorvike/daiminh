using System.ComponentModel.DataAnnotations;
using FluentValidation;
using shared.Enums;

namespace web.Areas.Admin.Requests.Content;

/// <summary>
/// Represents a request to create a new content item.
/// </summary>
public class ContentCreateRequest
{
    /// <summary>
    /// Gets or sets the ID of the content type.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Loại nội dung là bắt buộc.")]
    [Display(Name = "Loại nội dung", Prompt = "Chọn loại nội dung")]
    public int ContentTypeId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the author (optional).
    /// </summary>
    /// <example>2</example>
    [Display(Name = "Tác giả", Prompt = "Chọn tác giả")]
    public int? AuthorId { get; set; }

    /// <summary>
    /// Gets or set the CategoryIds 
    /// </summary>
    [Display(Name = "Danh mục", Prompt = "Chọn danh mục")]
    public List<int>? CategoryIds { get; set; } = [];

    /// <summary>
    /// Gets or set the TagIds 
    /// </summary>
    [Display(Name = "Thẻ", Prompt = "Chọn thẻ")]
    public List<int>? TagIds { get; set; } = [];

    public Dictionary<int, string> FieldValues { get; set; } = new();

    /// <summary>
    /// Gets or sets the title of the content item.
    /// </summary>
    /// <example>My Awesome Blog Post</example>
    [Required(ErrorMessage = "Tiêu đề là bắt buộc.")]
    [Display(Name = "Tiêu đề", Prompt = "Nhập tiêu đề")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the slug (URL-friendly name) of the content item.
    /// </summary>
    /// <example>my-awesome-blog-post</example>
    [Required(ErrorMessage = "Đường dẫn là bắt buộc.")]
    [Display(Name = "Đường dẫn", Prompt = "Nhập đường dẫn")]
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the body content of the content item.
    /// </summary>
    [Required(ErrorMessage = "Nội dung bài viết là bắt buộc.")]
    [Display(Name = "Nội dung", Prompt = "Nhập nội dung")]
    public string ContentBody { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL of the cover image for the content item.
    /// </summary>
    [Display(Name = "Ảnh bìa", Prompt = "Nhập URL ảnh bìa")]
    public string? CoverImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the publishing status of the content item.
    /// </summary>
    /// <example>Draft</example>
    [Display(Name = "Trạng thái", Prompt = "Chọn trạng thái")]
    public PublishStatus Status { get; set; } = PublishStatus.Draft;

    /// <summary>
    /// Gets or sets the meta title for SEO.
    /// </summary>
    /// <example>My Awesome Blog Post - Best Blog Ever</example>
    [Display(Name = "Tiêu đề SEO", Prompt = "Nhập tiêu đề SEO")]
    public string MetaTitle { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the meta description for SEO.
    /// </summary>
    /// <example>This is the best blog post ever written about...</example>
    [Display(Name = "Mô tả SEO", Prompt = "Nhập mô tả SEO")]
    public string MetaDescription { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the canonical URL for SEO.
    /// </summary>
    /// <example>https://example.com/my-awesome-blog-post</example>
    [Display(Name = "URL chuẩn", Prompt = "Nhập URL chuẩn")]
    public string CanonicalUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Open Graph title.
    /// </summary>
    /// <example>My Awesome Blog Post</example>
    [Display(Name = "Tiêu đề OG", Prompt = "Nhập tiêu đề OG")]
    public string OgTitle { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Open Graph description.
    /// </summary>
    /// <example>This is the best blog post ever written about...</example>
    [Display(Name = "Mô tả OG", Prompt = "Nhập mô tả OG")]
    public string OgDescription { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Open Graph image URL.
    /// </summary>
    /// <example>https://example.com/images/blog-post-image.jpg</example>
    [Display(Name = "Ảnh OG", Prompt = "Nhập URL ảnh OG")]
    public string OgImage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the structured data (e.g., JSON-LD) for SEO.
    /// </summary>
    /// <example>{ "@context": "https://schema.org", "@type": "BlogPosting", ... }</example>
    [Display(Name = "Dữ liệu cấu trúc", Prompt = "Nhập dữ liệu cấu trúc")]
    public string? StructuredData { get; set; }
}

/// <summary>
/// Validator for <see cref="ContentCreateRequest"/>.
/// </summary>
public class ContentCreateRequestValidator : AbstractValidator<ContentCreateRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContentCreateRequestValidator"/> class.
    /// </summary>
    public ContentCreateRequestValidator()
    {
        RuleFor(request => request.ContentTypeId)
            .GreaterThan(0).WithMessage("ID loại nội dung phải là một số nguyên dương.");

        RuleFor(request => request.AuthorId)
           .GreaterThan(0).When(x => x.AuthorId.HasValue).WithMessage("ID tác giả phải là một số nguyên dương.");

        RuleForEach(x => x.CategoryIds)
            .GreaterThan(0).WithMessage("ID danh mục phải là một số nguyên dương.");

        RuleForEach(x => x.TagIds)
            .GreaterThan(0).WithMessage("ID thẻ phải là một số nguyên dương.");

        RuleFor(request => request.Title)
            .NotEmpty().WithMessage("Tiêu đề không được bỏ trống.")
            .MaximumLength(255).WithMessage("Tiêu đề không được vượt quá 255 ký tự.");

        RuleFor(request => request.Slug)
            .NotEmpty().WithMessage("Đường dẫn (slug) không được bỏ trống.")
            .MaximumLength(255).WithMessage("Đường dẫn (slug) không được vượt quá 255 ký tự.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Đường dẫn (slug) chỉ được chứa chữ cái thường, số và dấu gạch ngang (-), và không được bắt đầu hoặc kết thúc bằng dấu gạch ngang.");

        RuleFor(request => request.ContentBody)
            .NotEmpty().WithMessage("Nội dung bài viết không được để trống.");

        RuleFor(request => request.CoverImageUrl)
           .MaximumLength(500).WithMessage("Đường dẫn ảnh bìa không được vượt quá 500 ký tự.")
           .Must(BeAValidUrl).When(x => !string.IsNullOrEmpty(x.CoverImageUrl)).WithMessage("Đường dẫn ảnh bìa không hợp lệ.");

        RuleFor(x => x.Status)
           .IsInEnum().WithMessage("Trạng thái xuất bản không hợp lệ.");

        // SEO Field Validations
        RuleFor(request => request.MetaTitle)
            .MaximumLength(255).WithMessage("Meta Title không được vượt quá 255 ký tự.");

        RuleFor(request => request.MetaDescription)
            .MaximumLength(500).WithMessage("Meta Description không được vượt quá 500 ký tự.");

        RuleFor(request => request.CanonicalUrl)
            .MaximumLength(500).WithMessage("Canonical URL không được vượt quá 500 ký tự.")
            .Must(BeAValidUrl).When(x => !string.IsNullOrEmpty(x.CanonicalUrl)).WithMessage("Canonical URL không hợp lệ.");

        RuleFor(request => request.OgTitle)
            .MaximumLength(255).WithMessage("Open Graph Title không được vượt quá 255 ký tự.");

        RuleFor(request => request.OgDescription)
            .MaximumLength(500).WithMessage("Open Graph Description không được vượt quá 500 ký tự.");

        RuleFor(request => request.OgImage)
            .MaximumLength(500).WithMessage("Open Graph Image URL không được vượt quá 500 ký tự.")
             .Must(BeAValidUrl).When(x => !string.IsNullOrEmpty(x.OgImage)).WithMessage("Open Graph Image URL không hợp lệ.");
    }

    private bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrEmpty(url)) return true;
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}