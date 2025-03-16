using FluentValidation;
using System.ComponentModel.DataAnnotations;

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
    public string Name { get; set; }
}

/// <summary>
/// Validator for <see cref="FileDeleteRequest"/>.
/// </summary>
public class FileDeleteRequestValidator : AbstractValidator<FileDeleteRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileDeleteRequestValidator"/> class.
    /// </summary>
    public FileDeleteRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}