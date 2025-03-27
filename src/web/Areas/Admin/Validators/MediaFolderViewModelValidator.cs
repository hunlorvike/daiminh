using FluentValidation;
using web.Areas.Admin.ViewModels.Media;

namespace web.Areas.Admin.Validators;
public class MediaFolderViewModelValidator : AbstractValidator<MediaFolderViewModel>
{
    public MediaFolderViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập tên thư mục")
            .MaximumLength(100).WithMessage("Tên thư mục không được vượt quá 100 ký tự");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Mô tả không được vượt quá 255 ký tự");

        RuleFor(x => x.ParentId)
            .Must((model, parentId) => parentId == null || parentId != model.Id)
            .WithMessage("Thư mục không thể là thư mục cha của chính nó");
    }
}
