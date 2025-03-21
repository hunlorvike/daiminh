using System.ComponentModel.DataAnnotations;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace web.Areas.Admin.Requests.Gallery;

/// <summary>
/// Represents a request to edit a file.
/// </summary>
public class FileEditRequest
{
    /// <summary>
    /// Gets or sets the ID of the file to edit.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the file.  
    /// </summary>
    [Display(Name = "Tên tệp", Prompt = "Nhập tên tệp")]
    [Required(ErrorMessage = "Tên tệp không được bỏ trống.")]
    public string? Name { get; set; }
}

/// <summary>
/// Validator for <see cref="FileEditRequest"/>.
/// </summary>
public class FileEditRequestValidator : AbstractValidator<FileEditRequest>
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileEditRequestValidator"/> class.
    /// </summary>
    public FileEditRequestValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(request => request.Id)
            .GreaterThan(0).WithMessage("ID tệp phải là một số nguyên dương.")
            .MustAsync(BeExistingFile).WithMessage("Tệp không tồn tại hoặc đã bị xóa.");

        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên tệp không được bỏ trống.")
            .MaximumLength(100).WithMessage("Tên tệp không được vượt quá 100 ký tự.")
            .Must(BeValidFileName).WithMessage("Tên tệp không được chứa các ký tự đặc biệt không hợp lệ (/ \\ : * ? \" < > |)")
            .MustAsync(BeUniqueFileNameInFolder).WithMessage("Tên tệp đã tồn tại trong cùng thư mục. Vui lòng chọn tên khác.");
    }

    /// <summary>
    /// Checks if the file exists and is not deleted.
    /// </summary>
    private async Task<bool> BeExistingFile(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.MediaFiles
            .AnyAsync(f => f.Id == id && f.DeletedAt == null, cancellationToken);
    }

    /// <summary>
    /// Checks if the file name is unique within the same folder, excluding the current file.
    /// </summary>
    private async Task<bool> BeUniqueFileNameInFolder(FileEditRequest request, string name, CancellationToken cancellationToken)
    {
        var file = await _dbContext.MediaFiles
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == request.Id && f.DeletedAt == null, cancellationToken);

        if (file == null) return true; // Nếu tệp không tồn tại, để logic khác xử lý

        return !await _dbContext.MediaFiles
            .AnyAsync(f => f.Name == name && f.FolderId == file.FolderId && f.Id != request.Id && f.DeletedAt == null, cancellationToken);
    }

    /// <summary>
    /// Checks if the file name contains valid characters.
    /// </summary>
    private bool BeValidFileName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        char[] invalidChars = { '/', '\\', ':', '*', '?', '"', '<', '>', '|' };
        return !invalidChars.Any(c => name.Contains(c));
    }
}