using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NCS.Application.Common.Exceptions;

namespace NCS.WebApi.Middleware;

public sealed class GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            await HandleAsync(context, ex);
        }
    }

    private static async Task HandleAsync(HttpContext context, Exception exception)
    {
        var problem = new ProblemDetails
        {
            Instance = context.Request.Path
        };

        switch (exception)
        {
            case ValidationException validationException:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                problem.Title = "Validation error";
                problem.Status = StatusCodes.Status400BadRequest;
                problem.Extensions["errors"] = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());
                break;

            case NotFoundException notFoundException:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                problem.Title = "Not found";
                problem.Status = StatusCodes.Status404NotFound;
                problem.Detail = notFoundException.Message;
                break;

            case UnauthorizedException unauthorizedException:
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                problem.Title = "Unauthorized";
                problem.Status = StatusCodes.Status401Unauthorized;
                problem.Detail = unauthorizedException.Message;
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                problem.Title = "Server error";
                problem.Status = StatusCodes.Status500InternalServerError;
                problem.Detail = "An unexpected error occurred.";
                break;
        }

        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problem);
    }
}
