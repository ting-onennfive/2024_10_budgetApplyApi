using FluentValidation;
using FluentValidation.AspNetCore;
using budgetApplyApi.Application.Configurations;

namespace budgetApplyApi.Server.Extensions
{
    internal static class MvcBuilderExtensions
    {
        internal static IMvcBuilder AddValidators(this IMvcBuilder builder)
        {
            builder.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AppConfiguration>());
            return builder;
        }
    }
}