using Car.Auction.Management.Api.Core.CustomExceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Car.Auction.Management.Api.Middlewares;

internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, $"{exception.GetType()} occurred: {exception.Message}");

        switch (exception)
        {
            case EntityCustomValidationException:
            {
                var errors = new Dictionary<string, string> { { "Creation resource validation error", exception.Message } };
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(errors, cancellationToken);
                break;
            }

            case EntityNotFoundException:
            {
                var errors = new Dictionary<string, string> { { "Retrieve resource error", exception.Message } };
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                await httpContext.Response.WriteAsJsonAsync(errors, cancellationToken);
                break;
            }
            
            case ValidationException validationException:
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                var errors = validationException!.Errors.ToDictionary(x => x.PropertyName, v => v.ErrorMessage);
                await httpContext.Response.WriteAsJsonAsync(errors, cancellationToken);
                break;
            }

            default:
            {
                var errors = new Dictionary<string, string> { { "Unexpected internal error", exception.Message } };
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(errors, cancellationToken);
                break;
            }
        }

        return true;
    }
}