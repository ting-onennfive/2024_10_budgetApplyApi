using budgetApplyApi.Application.Features.Budgets.Commands.AddEdit;
using budgetApplyApi.Shared.Constants.Application;
using FluentValidation;

namespace budgetApplyApi.Application.Validators.Features.BudgetDetails.Commands.AddEdit
{
    public class EditBudgetDetailCommandValidator : AbstractValidator<EditBudgetDetailCommand>
    {
        public EditBudgetDetailCommandValidator()
        {
            RuleFor(request => request.Id)
                .Must(x => x > 0).WithMessage(ResponseMessageConstants.FluentValidator.Unvalid)
                .OverridePropertyName("預算細項 id");
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage(ResponseMessageConstants.FluentValidator.Required)
                .Must(x => x.Length < 30).WithMessage(ResponseMessageConstants.FluentValidator.OverLength)
                .OverridePropertyName("細項名稱");
        }
    }
}
