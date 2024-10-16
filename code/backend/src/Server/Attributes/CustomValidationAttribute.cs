using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using budgetApplyApi.Shared.Wrapper;

namespace budgetApplyApi.Server.Attributes
{
    public class CustomValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                        .SelectMany(v => v.Errors)
                        .Select(v => v.ErrorMessage)
                        .ToList();

                var response = Result<string>.Fail(messages: errors);
                context.Result = new JsonResult(response)
                {
                    StatusCode = 200
                };
            }
        }
    }
}
