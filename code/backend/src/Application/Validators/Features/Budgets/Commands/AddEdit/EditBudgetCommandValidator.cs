using FluentValidation;
using budgetApplyApi.Application.Features.Budgets.Commands.AddEdit;
using budgetApplyApi.Shared.Constants.Application;

namespace budgetApplyApi.Application.Validators.Features.Budgets.Commands.AddEdit
{
    public class EditBudgetCommandValidator : AbstractValidator<EditBudgetCommand>
    {
        public EditBudgetCommandValidator()
        {
            RuleFor(request => request.Id)
                .Must(x => x > 0).WithMessage(ResponseMessageConstants.FluentValidator.Unvalid)
                .OverridePropertyName("預算大項 id");
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage(ResponseMessageConstants.FluentValidator.Required)
                .Must(x => x.Length < 30).WithMessage(ResponseMessageConstants.FluentValidator.OverLength)
                .OverridePropertyName("大項名稱");
        }
    }
}