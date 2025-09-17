using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.RegularExpressions;

namespace Presentation.Filters
{
    public class ValidateEmailAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.RouteData.Values.TryGetValue("email", out var emailValue) && emailValue is string email)
            {
                var emailPattern = @"^\S+@\S+\.\S+$";

                if (!Regex.IsMatch(email, emailPattern))
                {
                    context.Result = new BadRequestObjectResult(new
                    {
                        message = "Please enter a valid email address."
                    });
                }
            }
        }
    }
}
