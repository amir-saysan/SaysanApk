using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SaysanPwa.Api.Filters;

public class ValidateModelStateAttribute : Attribute, IAsyncActionFilter
{
    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ModelState.IsValid)
        {
            return next();
        }

        ExceptionHandling.ApplicationException ax = new(StatusCodes.Status422UnprocessableEntity,
            context
            .ModelState
            .SelectMany(ex => ex.Value.Errors)
            .Select(e => e.ErrorMessage)
            .ToList());

        context.Result = new UnprocessableEntityObjectResult(ax);

        return Task.CompletedTask;
    }
}
