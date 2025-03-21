using System.ComponentModel.DataAnnotations;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace web.Areas.Admin.Requests.Gallery;

/// <summary>
/// Represents a request to create a new folder.
/// </summary>
public class FolderCreateRequest
{
    /// <summary>
    /// Gets or sets the name of the folder.
    /// </summary>
    [Display(Name = "Tên thư mục", Prompt = "Nhập tên thư mục")]
    [Required(ErrorMessage = "Tên thư mục không được bỏ trống.")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the ID of the parent folder.               
    /// </summary>
    [Display(Name = "Thư mục cha", Prompt = "Chọn thư mục cha")]
    public int? ParentId { get; set; }
}


/// <summary>
/// Validator for <see cref="FolderCreateRequest"/>.
/// </summary>
public class FolderCreateRequestValidator : AbstractValidator<FolderCreateRequest>
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="FolderCreateRequestValidator"/> class.
    /// </summary>
    public FolderCreateRequestValidator(ApplicationDbContext context)
    {
        _dbContext = context;

        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên thư mục không được bỏ trống.")
            .MaximumLength(50).WithMessage("Tên thư mục không được vượt quá 50 ký tự.")
            .Matches("^[a-zA-Z0-9\\s\\-_]+$").WithMessage("Tên thư mục chỉ được chứa chữ cái, số, dấu gạch ngang và gạch dưới")
            .MustAsync(BeUniqueFolderNameInParent).WithMessage("Tên thư mục đã tồn tại trong thư mục cha. Vui lòng chọn tên khác.");

        RuleFor(request => request.ParentId)
            .MustAsync(BeExistingParentFolder).When(request => request.ParentId.HasValue)
            .WithMessage("Thư mục cha không tồn tại hoặc đã bị xóa.");
    }

    /// <summary>
    /// Checks if the folder name is unique within the parent folder.
    /// </summary>
    private async Task<bool> BeUniqueFolderNameInParent(FolderCreateRequest request, string name, CancellationToken cancellationToken)
    {
        return !await _dbContext.Folders
            .AnyAsync(f => f.Name == name && f.ParentId == request.ParentId && f.DeletedAt == null, cancellationToken);
    }

    /// <summary>
    /// Checks if the parent folder exists and is not deleted.
    /// </summary>
    private async Task<bool> BeExistingParentFolder(int? parentId, CancellationToken cancellationToken)
    {
        return await _dbContext.Folders
            .AnyAsync(f => f.Id == parentId && f.DeletedAt == null, cancellationToken);
    }
}