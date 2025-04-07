using domain.Entities;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Media;

namespace web.Areas.Admin.Validators.Media;

public class MediaFolderViewModelValidator : AbstractValidator<MediaFolderViewModel>
{
    private readonly ApplicationDbContext _context;
    public MediaFolderViewModelValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập tên thư mục.")
            .MaximumLength(100).WithMessage("Tên thư mục không được vượt quá 100 ký tự.")
            .Matches(@"^[a-zA-Z0-9_~\-.\s]+$").WithMessage("Tên thư mục chứa ký tự không hợp lệ.");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Mô tả không được vượt quá 255 ký tự.");

        RuleFor(x => x)
            .Must(folder => !FolderNameExistsSync(folder))
            .WithMessage("Tên thư mục đã tồn tại trong thư mục cha này.")
            .WithName(nameof(MediaFolderViewModel.Name));
    }

    private bool FolderNameExistsSync(MediaFolderViewModel folder)
    {
        return _context.Set<MediaFolder>()
             .Any(f => f.ParentId == folder.ParentId && f.Name == folder.Name && f.Id != folder.Id);
    }
}