using System.ComponentModel.DataAnnotations;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace web.Areas.Admin.Requests.Slider;

/// <summary>
/// Represents a request to delete a slider item.
/// </summary>
public class SliderDeleteRequest
{
    /// <summary>
    /// Gets or sets the ID of the slider item to delete.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Id không được bỏ trống")]
    public int Id { get; set; }
}

/// <summary>
/// Validator for <see cref="SliderDeleteRequest"/>
/// </summary>
public class SliderDeleteRequestValidator : AbstractValidator<SliderDeleteRequest>
{
    private readonly ApplicationDbContext _dbContext;
    /// <summary>
    /// Initializer a new instance of the <see cref="SliderDeleteRequestValidator"/> class.
    /// </summary>
    public SliderDeleteRequestValidator(ApplicationDbContext context)
    {
        _dbContext = context;

        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID slider phải là một số nguyên dương.")
            .MustAsync(BeExistingSlider).WithMessage("Slider không tồn tại hoặc đã bị xoá");
    }


    private async Task<bool> BeExistingSlider(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Sliders
            .AnyAsync(s => s.Id == id && s.DeletedAt == null, cancellationToken);
    }
}