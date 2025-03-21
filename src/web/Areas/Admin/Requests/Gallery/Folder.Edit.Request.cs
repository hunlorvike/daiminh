using System.ComponentModel.DataAnnotations;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace web.Areas.Admin.Requests.Gallery;

/// <summary>
/// Represents a request to rename a folder.
/// </summary>
public class FolderEditRequest
{
    /// <summary>
    /// Gets or sets the ID of the folder to rename.
    /// </summary>
    [Required]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the new name of the folder.
    /// </summary>
    [Display(Name = "Tên thư mục", Prompt = "Nhập tên thư mục")]
    [Required(ErrorMessage = "Tên thư mục không được bỏ trống.")]
    public string? Name { get; set; }
}

/// <summary>
/// Validator for <see cref="FolderEditRequest"/>.
/// </summary>
public class FolderEditRequestValidator : AbstractValidator<FolderEditRequest>
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="FolderEditRequestValidator"/> class.
    /// </summary>
    public FolderEditRequestValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(request => request.Id)
            .GreaterThan(0).WithMessage("ID thư mục phải là một số nguyên dương.")
            .MustAsync(BeExistingFolder).WithMessage("Thư mục không tồn tại hoặc đã bị xóa.");

        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên thư mục không được bỏ trống.")
            .MaximumLength(100).WithMessage("Tên thư mục không được vượt quá 100 ký tự.")
            .Matches("^[a-zA-Z0-9\\s\\-_]+$").WithMessage("Tên thư mục chỉ được chứa chữ cái, số, dấu gạch ngang và gạch dưới")
            .MustAsync(BeUniqueFolderNameInParent).WithMessage("Tên thư mục đã tồn tại trong thư mục cha. Vui lòng chọn tên khác.");
    }

    /// <summary>
    /// Checks if the folder exists and is not deleted.
    /// </summary>
    private async Task<bool> BeExistingFolder(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Folders
            .AnyAsync(f => f.Id == id && f.DeletedAt == null, cancellationToken);
    }

    /// <summary>
    /// Checks if the folder name is unique within the parent folder, excluding the current folder.
    /// </summary>
    private async Task<bool> BeUniqueFolderNameInParent(FolderEditRequest request, string name, CancellationToken cancellationToken)
    {
        var folder = await _dbContext.Folders
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == request.Id && f.DeletedAt == null, cancellationToken);

        if (folder == null) return true;

        return !await _dbContext.Folders
            .AnyAsync(f => f.Name == name && f.ParentId == folder.ParentId && f.Id != request.Id && f.DeletedAt == null, cancellationToken);
    }
}