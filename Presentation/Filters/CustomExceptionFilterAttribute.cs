using Application.Exceptions;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.Filters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case AutoMapperMappingException ex:
                    context.Result = new BadRequestObjectResult(
                        new { message = ex.Message });
                    break;

                case ValidationException ex:
                    context.Result = new BadRequestObjectResult(
                        new { message = string.Join(" ", ex.Errors.Select(e => e.ErrorMessage)) });
                    break;

                case NotFoundException ex:
                    context.Result = new NotFoundObjectResult(
                        new { message = ex.Message });
                    break;

                case Exception ex:
                    context.Result = new BadRequestObjectResult(
                        new { message = ex.Message });
                    break;
            }

            context.ExceptionHandled = true;
        }
    }
}