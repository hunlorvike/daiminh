using FluentValidation;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Validators;

public class ClaimDefinitionViewModelValidator : AbstractValidator<ClaimDefinitionViewModel>
{
    private readonly IClaimDefinitionService _claimDefinitionService;

    public ClaimDefinitionViewModelValidator(IClaimDefinitionService claimDefinitionService)
    {
        _claimDefinitionService = claimDefinitionService;

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(50).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.Value)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(50).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
            .MustAsync(BeUniqueValue).WithMessage("Giá trị Claim đã tồn tại. Vui lòng chọn giá trị khác.");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");
    }

    private async Task<bool> BeUniqueValue(ClaimDefinitionViewModel viewModel, string value, ValidationContext<ClaimDefinitionViewModel> context, CancellationToken cancellationToken)
    {
        return await _claimDefinitionService.IsClaimDefinitionValueUniqueAsync(viewModel.Id, value);
    }
}