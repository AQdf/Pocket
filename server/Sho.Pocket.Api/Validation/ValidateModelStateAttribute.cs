using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sho.Pocket.Api.Models;
using System.Collections.Generic;

namespace Sho.Pocket.Api.Validation
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                List<ResponseError> errors = new List<ResponseError>();

                foreach (var key in context.ModelState.Keys)
                {
                    if (context.ModelState.TryGetValue(key, out ModelStateEntry fieldState))
                    {
                        string errorCode = $"{key}{fieldState.ValidationState}";

                        foreach (var error in fieldState.Errors)
                        {
                            errors.Add(new ResponseError(errorCode, error.ErrorMessage));
                        }
                    }
                }

                context.Result = new BadRequestObjectResult(errors);
            }
        }
    }
}
