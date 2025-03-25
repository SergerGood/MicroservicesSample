using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var responseName = typeof(TResponse).Name;

        logger.LogInformation("Begin Handle <{Request},{Response}> {@RequestData}", requestName, responseName, request);
        var stopwatch = Stopwatch.StartNew();

        var response = await next();

        stopwatch.Stop();

        if (stopwatch.Elapsed.Seconds > 3)
        {
            logger.LogWarning("[PERF] The request {Request} took {TimeTaken} ms", requestName,
                stopwatch.ElapsedMilliseconds);
        }

        logger.LogInformation("End Handled <{Request},{Response}>", requestName, responseName);

        return response;
    }
}