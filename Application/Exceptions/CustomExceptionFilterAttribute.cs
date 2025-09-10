using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Application.Exceptions
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is AutoMapperMappingException || context.Exception is ValidationException
                || context.Exception is NotFoundException || context.Exception is Exception)
            {
                context.Result = new BadRequestObjectResult(new { message = context.Exception.Message });
                context.ExceptionHandled = true;
                return;
            }

            context.ExceptionHandled = true;
        }
    }
}
