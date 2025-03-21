using System.ComponentModel.DataAnnotations;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace web.Areas.Admin.Requests.Gallery;

/// <summary>
/// Represents a request to delete a file.
/// </summary>
public class FileDeleteRequest
{
    /// <summary>
    /// Gets or sets the ID of the file to delete.
    /// </summary>
    [Display(Name = "ID", Prompt = "ID")]
    [Required(ErrorMessage = "ID không được bỏ trống")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the file to delete.
    /// </summary>
    [Display(Name = "Tên", Prompt = "Tên")]
    [Required(ErrorMessage = "Tên không được bỏ trống")]
    public string? Name { get; set; }
}

/// <summary>
/// Validator for <see cref="FileDeleteRequest"/>.
/// </summary>
public class FileDeleteRequestValidator : AbstractValidator<FileDeleteRequest>
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileDeleteRequestValidator"/> class.
    /// </summary>
    public FileDeleteRequestValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID tệp phải là một số nguyên dương.")
            .MustAsync(BeExistingFile).WithMessage("Tệp không tồn tại hoặc đã bị xóa.");

        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Tên tệp không được vượt quá 100 ký tự.")
            .When(x => !string.IsNullOrEmpty(x.Name)); // Chỉ kiểm tra nếu Name được cung cấp
    }

    /// <summary>
    /// Checks if the file exists and is not deleted.
    /// </summary>
    private async Task<bool> BeExistingFile(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.MediaFiles
            .AnyAsync(m => m.Id == id && m.DeletedAt == null, cancellationToken);
    }
}