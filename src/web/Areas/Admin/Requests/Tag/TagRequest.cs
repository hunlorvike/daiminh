using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace web.Areas.Admin.Requests.Tag;

public class TagCreateRequest
{
    [Display(Name = "Tên thẻ", Prompt = "Nhập tên của thẻ")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Đường dẫn danh mục", Prompt = "Nhập đường dẫn của danh mục")]
    public string Slug { get; set; } = string.Empty;
}

public class TagUpdateRequest
{
    public int Id { get; set; }

    [Display(Name = "Tên thẻ", Prompt = "Nhập tên của thẻ")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Đường dẫn danh mục", Prompt = "Nhập đường dẫn của danh mục")]
    public string Slug { get; set; } = string.Empty;
}

public class TagDeleteRequest
{
    public int Id { get; set; }
}

public class TagCreateRequestValidator : AbstractValidator<TagCreateRequest>
{
    public TagCreateRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên thẻ không được để trống.")
            .MaximumLength(50)
            .WithMessage("Tên thẻ không được vượt quá 50 ký tự.");

        RuleFor(request => request.Slug)
            .NotEmpty().WithMessage("Slug không được để trống.")
            .MaximumLength(50)
            .WithMessage("Slug không được vượt quá 50 ký tự.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang.");
    }
}

public class TagUpdateRequestValidator : AbstractValidator<TagUpdateRequest>
{
    public TagUpdateRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên không được để trống.")
            .MaximumLength(50).WithMessage("Tên không được vượt quá 50 ký tự.");

        RuleFor(request => request.Slug)
            .NotEmpty().WithMessage("Slug không được để trống.")
            .MaximumLength(50).WithMessage("Slug không được vượt quá 50 ký tự.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang.");
    }
}