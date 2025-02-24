using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace web.Areas.Admin.Requests.Tag
{
    public class TagRequest
    {
        // For Create and Update
        [Required]
        [Display(Name = "Tên thẻ")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Slug")]
        public string Slug { get; set; } = string.Empty;

        // Optional fields for Create/Update
        [Display(Name = "Ngày tạo")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime UpdatedAt { get; set; }

        // For Edit and Delete
        public int? Id { get; set; }

        // For Delete only
        public bool IsDeleteRequest { get; set; }

        // For Soft Deletion
        public DateTime? DeletedAt { get; set; }
    }

    // Validator for Tag Create/Update Request
    public class TagRequestValidator : AbstractValidator<TagRequest>
    {
        public TagRequestValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty().WithMessage("Tên thẻ không được để trống.")
                .MaximumLength(50).WithMessage("Tên thẻ không được vượt quá 50 ký tự."); // Adjust max length to match schema

            RuleFor(request => request.Slug)
                .NotEmpty().WithMessage("Slug không được để trống.")
                .MaximumLength(50).WithMessage("Slug không được vượt quá 50 ký tự.") // Adjust max length to match schema
                .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang.");

            // Optional: Add validation rules for CreatedAt and UpdatedAt if needed
            RuleFor(request => request.CreatedAt)
                .NotEmpty().WithMessage("Ngày tạo không được để trống.");

            RuleFor(request => request.UpdatedAt)
                .NotEmpty().WithMessage("Ngày cập nhật không được để trống.");
        }
    }
}
