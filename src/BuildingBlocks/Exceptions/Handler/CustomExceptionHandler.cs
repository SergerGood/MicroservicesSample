using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions.Handler;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError("Error Message: {ExceptionMessage}, Time of occurrence {Time}",
            exception.Message, DateTime.UtcNow);

        var (details, title, statusCode) = GetExceptionDetails(httpContext, exception);

        var problemDetails = new ProblemDetails
        {
            Title = title,
            Status = statusCode,
            Detail = details,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
        }

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static (string, string, int) GetExceptionDetails(HttpContext httpContext, Exception exception)
    {
        var statusCode = exception switch
        {
            InternalServerException => httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError,
            ValidationException => httpContext.Response.StatusCode = StatusCodes.Status400BadRequest,
            BadRequestException => httpContext.Response.StatusCode = StatusCodes.Status400BadRequest,
            NotFoundException => httpContext.Response.StatusCode = StatusCodes.Status404NotFound,
            _ => httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
        };

        return (exception.Message, exception.GetType().Name, statusCode);
    }
}