using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.ProductType;

public class ProductTypeCreateRequest
{
    [Display(Name = "Tên loại sản phẩm", Prompt = "Nhập tên của loại sản phẩm")]
    public string? Name { get; set; }

    [Display(Name = "Đường dẫn loại sản phẩm", Prompt = "Nhập đường dẫn của loại sản phẩm")]
    public string? Slug { get; set; }
}

public class ProductTypeUpdateRequest
{
    public int Id { get; set; }

    [Display(Name = "Tên loại sản phẩm", Prompt = "Nhập tên của loại sản phẩm")]
    public string? Name { get; set; }

    [Display(Name = "Đường dẫn loại sản phẩm", Prompt = "Nhập đường dẫn của loại sản phẩm")]
    public string? Slug { get; set; }
}

public class ProductTypeDeleteRequest
{
    public int Id { get; set; }
}

public class ProductTypeCreateRequestValidator : AbstractValidator<ProductTypeCreateRequest>
{
    public ProductTypeCreateRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên không được để trống.")
            .MaximumLength(255).WithMessage("Tên không được vượt quá 255 ký tự.");

        RuleFor(request => request.Slug)
            .NotEmpty().WithMessage("Slug không được để trống.")
            .MaximumLength(255).WithMessage("Slug không được vượt quá 255 ký tự.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang.");
    }
}

public class ProductTypeUpdateRequestValidator : AbstractValidator<ProductTypeUpdateRequest>
{
    public ProductTypeUpdateRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên không được để trống.")
            .MaximumLength(255).WithMessage("Tên không được vượt quá 255 ký tự.");

        RuleFor(request => request.Slug)
            .NotEmpty().WithMessage("Slug không được để trống.")
            .MaximumLength(255).WithMessage("Slug không được vượt quá 255 ký tự.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang.");
    }
}