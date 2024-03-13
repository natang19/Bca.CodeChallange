using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace Car.Auction.Management.Api.Middlewares;

internal sealed class RequestsValidationExceptionHandler(ILogger<RequestsValidationExceptionHandler> logger) : IExceptionHandler
{

    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "validations exceptions");

        ValidationException validationException = exception as ValidationException;
        var errorsMessage = validationException.Errors.ToDictionary(x => x.PropertyName);

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(errorsMessage, cancellationToken);

        return true;
    }
}