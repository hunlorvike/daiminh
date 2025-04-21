using FluentValidation;
using infrastructure;
using web.Areas.Admin.ViewModels.Media;

namespace web.Areas.Admin.Validators.Media;

public class MediaFolderViewModelValidator : AbstractValidator<MediaFolderViewModel>
{
    private readonly ApplicationDbContext _context;

    public MediaFolderViewModelValidator(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên thư mục không được để trống.")
            .MaximumLength(100).WithMessage("Tên thư mục không được vượt quá {MaxLength} ký tự.")
            .Must((viewModel, name) =>
            {
                var exists = _context.MediaFolders
                                       .Any(f => f.ParentId == viewModel.ParentId && f.Name == name && f.Id != viewModel.Id);
                return !exists;
            }).WithMessage("Tên thư mục đã tồn tại trong thư mục hiện tại.");

        RuleFor(x => x.ParentId)
            .Must((parentId) =>
            {
                if (!parentId.HasValue) return true;
                return _context.MediaFolders.Any(f => f.Id == parentId.Value);
            }).WithMessage("Thư mục cha không tồn tại.");

        RuleFor(x => x.ParentId)
           .NotEqual(x => x.Id).When(x => x.ParentId.HasValue && x.Id > 0).WithMessage("Thư mục cha không thể là chính nó.");
    }
}