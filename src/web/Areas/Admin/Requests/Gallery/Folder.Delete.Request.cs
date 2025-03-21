using System.ComponentModel.DataAnnotations;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace web.Areas.Admin.Requests.Gallery;

/// <summary>
/// Represents a request to delete a folder.
/// </summary>
public class FolderDeleteRequest
{
    /// <summary>
    /// Gets or sets the ID of the folder to delete.
    /// </summary>
    [Display(Name = "ID", Prompt = "ID")]
    [Required(ErrorMessage = "ID không được bỏ trống")]
    public int Id { get; set; }
    /// <summary>
    /// Gets or sets the name of the folder to delete.
    /// </summary>
    [Display(Name = "Tên", Prompt = "Tên")]
    [Required(ErrorMessage = "Tên không được bỏ trống")]
    public string? Name { get; set; }
}

/// <summary>
/// Validator for <see cref="FolderDeleteRequest"/>.
/// </summary>
public class FolderDeleteRequestValidator : AbstractValidator<FolderDeleteRequest>
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="FolderDeleteRequestValidator"/> class.
    /// </summary>
    public FolderDeleteRequestValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID thư mục phải là một số nguyên dương.")
            .MustAsync(BeExistingFolder).WithMessage("Thư mục không tồn tại hoặc đã bị xóa.")
            .MustAsync(HaveNoChildrenOrFiles).WithMessage("Không thể xóa thư mục vì vẫn còn tệp hoặc thư mục con bên trong.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên thư mục không được bỏ trống.")
            .MaximumLength(100).WithMessage("Tên thư mục không được vượt quá 100 ký tự.");
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
    /// Checks if the folder has no child folders or files.
    /// </summary>
    private async Task<bool> HaveNoChildrenOrFiles(int id, CancellationToken cancellationToken)
    {
        var hasChildren = await _dbContext.Folders
            .AnyAsync(f => f.ParentId == id && f.DeletedAt == null, cancellationToken);
        var hasFiles = await _dbContext.MediaFiles
            .AnyAsync(m => m.FolderId == id && m.DeletedAt == null, cancellationToken);
        return !hasChildren && !hasFiles;
    }
}