using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Netcool.Core.AspNetCore.Filters
{
    public class ValidateAttribute : ActionFilterAttribute
    {
        public bool AllowNull { get; set; }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!AllowNull)
            {
                var nullArguments = actionContext.ActionArguments
                    .Where(arg => arg.Value == null)
                    .Select(arg => new ValidateResult(arg.Key, $"Value of {arg.Key} cannot be null."))
                    .ToArray();

                if (nullArguments.Any())
                {
                    actionContext.Result = new BadRequestObjectResult(nullArguments);
                    return;
                }
            }

            if (!actionContext.ModelState.IsValid)
            {
                var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => new ValidateResult(e.Key, e.Value.Errors.First().ErrorMessage))
                    .ToArray();

                actionContext.Result = new BadRequestObjectResult(errors);
            }
        }
    }
}