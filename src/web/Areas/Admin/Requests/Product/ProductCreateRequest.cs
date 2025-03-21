using System.ComponentModel.DataAnnotations;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Enums;

namespace web.Areas.Admin.Requests.Product;

/// <summary>
/// Represents a request to create a new product.
/// </summary>
public class ProductCreateRequest
{
    /// <summary>
    /// Gets or sets the ID of the product type.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Loại sản phẩm là bắt buộc.")]
    [Display(Name = "Loại sản phẩm", Prompt = "Chọn loại sản phẩm")]
    public int ProductTypeId { get; set; }

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

    /// <summary>
    /// Gets or sets the field values for the product.
    /// </summary>
    public List<ProductFieldValueRequest> FieldValues { get; set; } = [];

    /// <summary>
    /// Gets or sets the images for the product.
    /// </summary>
    public List<ProductImageRequest> Images { get; set; } = [];

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    /// <example>Sơn Dulux Nội Thất</example>
    [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
    [Display(Name = "Tên sản phẩm", Prompt = "Nhập tên sản phẩm")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the slug (URL-friendly name) of the product.
    /// </summary>
    /// <example>son-dulux-noi-that</example>
    [Required(ErrorMessage = "Đường dẫn là bắt buộc.")]
    [Display(Name = "Đường dẫn", Prompt = "Nhập đường dẫn")]
    public string? Slug { get; set; }

    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    [Display(Name = "Mô tả", Prompt = "Nhập mô tả sản phẩm")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the base price of the product.
    /// </summary>
    /// <example>250000</example>
    [Required(ErrorMessage = "Giá cơ bản là bắt buộc.")]
    [Display(Name = "Giá cơ bản", Prompt = "Nhập giá cơ bản")]
    public decimal BasePrice { get; set; }

    /// <summary>
    /// Gets or sets the SKU (Stock Keeping Unit) of the product.
    /// </summary>
    /// <example>DLX-NT-001</example>
    [Required(ErrorMessage = "Mã SKU là bắt buộc.")]
    [Display(Name = "Mã SKU", Prompt = "Nhập mã SKU")]
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the publishing status of the product.
    /// </summary>
    /// <example>Draft</example>
    [Display(Name = "Trạng thái", Prompt = "Chọn trạng thái")]
    public PublishStatus Status { get; set; } = PublishStatus.Draft;

    /// <summary>
    /// Gets or sets the meta title for SEO.
    /// </summary>
    /// <example>Sơn Dulux Nội Thất - Chất Lượng Cao</example>
    [Display(Name = "Tiêu đề SEO", Prompt = "Nhập tiêu đề SEO")]
    public string MetaTitle { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the meta description for SEO.
    /// </summary>
    /// <example>Sơn Dulux Nội Thất với chất lượng cao, độ bền màu tuyệt vời...</example>
    [Display(Name = "Mô tả SEO", Prompt = "Nhập mô tả SEO")]
    public string MetaDescription { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the canonical URL for SEO.
    /// </summary>
    /// <example>https://example.com/san-pham/son-dulux-noi-that</example>
    [Display(Name = "URL chuẩn", Prompt = "Nhập URL chuẩn")]
    public string CanonicalUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Open Graph title.
    /// </summary>
    /// <example>Sơn Dulux Nội Thất</example>
    [Display(Name = "Tiêu đề OG", Prompt = "Nhập tiêu đề OG")]
    public string OgTitle { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Open Graph description.
    /// </summary>
    /// <example>Sơn Dulux Nội Thất với chất lượng cao, độ bền màu tuyệt vời...</example>
    [Display(Name = "Mô tả OG", Prompt = "Nhập mô tả OG")]
    public string OgDescription { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Open Graph image URL.
    /// </summary>
    /// <example>https://example.com/images/son-dulux-noi-that.jpg</example>
    [Display(Name = "Ảnh OG", Prompt = "Nhập URL ảnh OG")]
    public string OgImage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the structured data (e.g., JSON-LD) for SEO.
    /// </summary>
    /// <example>{ "@context": "https://schema.org", "@type": "Product", ... }</example>
    [Display(Name = "Dữ liệu cấu trúc", Prompt = "Nhập dữ liệu cấu trúc")]
    public string? StructuredData { get; set; }
}

/// <summary>
/// Represents a product field value in a request.
/// </summary>
public class ProductFieldValueRequest
{
    /// <summary>
    /// Gets or sets the ID of the field definition.
    /// </summary>
    public int FieldId { get; set; }

    /// <summary>
    /// Gets or sets the value of the field.
    /// </summary>
    public string Value { get; set; } = string.Empty;
}

/// <summary>
/// Represents a product image in a request.
/// </summary>
public class ProductImageRequest
{
    /// <summary>
    /// Gets or sets the URL of the image.
    /// </summary>
    [Required(ErrorMessage = "URL hình ảnh là bắt buộc.")]
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the alternative text for the image.
    /// </summary>
    public string AltText { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether this is the primary image.
    /// </summary>
    public bool IsPrimary { get; set; }

    /// <summary>
    /// Gets or sets the display order of the image.
    /// </summary>
    public short DisplayOrder { get; set; }
}

/// <summary>
/// Validator for <see cref="ProductCreateRequest"/>.
/// </summary>
public class ProductCreateRequestValidator : AbstractValidator<ProductCreateRequest>
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductCreateRequestValidator"/> class.
    /// </summary>
    public ProductCreateRequestValidator(ApplicationDbContext context)
    {
        _dbContext = context;

        RuleFor(request => request.ProductTypeId)
            .GreaterThan(0).WithMessage("ID loại sản phẩm phải là một số nguyên dương.")
            .MustAsync(BeExistingProductType).WithMessage("Loại sản phẩm không tồn tại.");

        RuleForEach(x => x.CategoryIds)
            .GreaterThan(0).WithMessage("ID danh mục phải là một số nguyên dương.");

        RuleForEach(x => x.TagIds)
            .GreaterThan(0).WithMessage("ID thẻ phải là một số nguyên dương.");

        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên sản phẩm không được bỏ trống.")
            .MaximumLength(255).WithMessage("Tên sản phẩm không được vượt quá 255 ký tự.");

        RuleFor(request => request.Slug)
            .NotEmpty().WithMessage("Đường dẫn (slug) không được bỏ trống.")
            .MaximumLength(255).WithMessage("Đường dẫn (slug) không được vượt quá 255 ký tự.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Đường dẫn (slug) chỉ được chứa chữ cái thường, số và dấu gạch ngang (-), và không được bắt đầu hoặc kết thúc bằng dấu gạch ngang.")
            .MustAsync(BeUniqueSlug).WithMessage("Đường dẫn (slug) đã tồn tại. Vui lòng chọn một đường dẫn khác.");

        RuleFor(request => request.Description)
            .MaximumLength(10000).WithMessage("Mô tả không được vượt quá 10000 ký tự.");

        RuleFor(request => request.BasePrice)
            .GreaterThanOrEqualTo(0).WithMessage("Giá cơ bản không được âm.");

        RuleFor(request => request.Sku)
            .NotEmpty().WithMessage("Mã SKU không được bỏ trống.")
            .MaximumLength(50).WithMessage("Mã SKU không được vượt quá 50 ký tự.")
            .MustAsync(BeUniqueSku).WithMessage("Mã SKU đã tồn tại. Vui lòng chọn một mã SKU khác.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Trạng thái xuất bản không hợp lệ.");

        // Validate at least one image is provided
        RuleFor(x => x.Images)
            .NotEmpty().WithMessage("Sản phẩm phải có ít nhất một hình ảnh.");

        // Validate that there is exactly one primary image if images are provided
        RuleFor(x => x.Images)
            .Must(images => images.Count(i => i.IsPrimary) == 1)
            .When(x => x.Images.Any())
            .WithMessage("Phải có đúng một hình ảnh chính.");

        // Validate each image
        RuleForEach(x => x.Images)
            .SetValidator(new ProductImageValidator());

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

    private async Task<bool> BeUniqueSlug(string slug, CancellationToken cancellationToken)
    {
        return !await _dbContext.Products
            .AnyAsync(p => p.Slug == slug && p.DeletedAt == null, cancellationToken);
    }

    private async Task<bool> BeUniqueSku(string sku, CancellationToken cancellationToken)
    {
        return !await _dbContext.Products
            .AnyAsync(p => p.Sku == sku && p.DeletedAt == null, cancellationToken);
    }

    private async Task<bool> BeExistingProductType(int productTypeId, CancellationToken cancellationToken)
    {
        return await _dbContext.ProductTypes
            .AnyAsync(pt => pt.Id == productTypeId && pt.DeletedAt == null, cancellationToken);
    }
}

/// <summary>
/// Validator for <see cref="ProductImageRequest"/>.
/// </summary>
public class ProductImageValidator : AbstractValidator<ProductImageRequest>
{
    public ProductImageValidator()
    {
        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("URL hình ảnh không được bỏ trống.")
            .MaximumLength(255).WithMessage("URL hình ảnh không được vượt quá 255 ký tự.")
            .Must(BeAValidUrl).WithMessage("URL hình ảnh không hợp lệ.");

        RuleFor(x => x.AltText)
            .NotEmpty().WithMessage("Mô tả hình ảnh không được bỏ trống.")
            .MaximumLength(255).WithMessage("Mô tả hình ảnh không được vượt quá 255 ký tự.");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}