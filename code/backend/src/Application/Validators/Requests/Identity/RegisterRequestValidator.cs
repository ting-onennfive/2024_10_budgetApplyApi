using FluentValidation;
using Microsoft.Extensions.Localization;
using budgetApplyApi.Application.Requests.Identity;
using budgetApplyApi.Shared.Constants.Application;

namespace budgetApplyApi.Application.Validators.Requests.Identity
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator(IStringLocalizer<RegisterRequestValidator> localizer)
        {
            RuleFor(request => request.FirstName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => ResponseMessageConstants.Required("姓氏"));
            RuleFor(request => request.LastName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => ResponseMessageConstants.Required("姓名"));
            RuleFor(request => request.Email)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => ResponseMessageConstants.Required("電子郵件"))
                .EmailAddress().WithMessage(x => ResponseMessageConstants.Unvalid("電子郵件"));
            RuleFor(request => request.UserName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => ResponseMessageConstants.Required("帳號"))
                .MinimumLength(6).WithMessage("帳號至少 6 碼，且由英數字組合");
            RuleFor(request => request.Password)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => ResponseMessageConstants.Required("密碼"))
                .MinimumLength(8).WithMessage("密碼至少 8 碼，且由英數字組合")
                .Matches(@"[A-Z]").WithMessage("密碼至少有 1 個大寫字母")
                .Matches(@"[a-z]").WithMessage("密碼至少有 1 個小寫字母")
                .Matches(@"[0-9]").WithMessage("密碼至少有 1 個數字");
            RuleFor(request => request.ConfirmPassword)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => ResponseMessageConstants.Required("密碼確認"))
                .Equal(request => request.Password).WithMessage(x => ResponseMessageConstants.Unvalid("密碼不一致"));
        }
    }
}