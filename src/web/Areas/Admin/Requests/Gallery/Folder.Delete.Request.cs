using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.Gallery;

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
    public string Name { get; set; }
}

/// <summary>
/// Validator for <see cref="FolderDeleteRequest"/>.
/// </summary>
public class FolderDeleteRequestValidator : AbstractValidator<FolderDeleteRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FolderDeleteRequestValidator"/> class.
    /// </summary>
    public FolderDeleteRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}