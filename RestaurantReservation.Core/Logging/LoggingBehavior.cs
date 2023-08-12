﻿using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace RestaurantReservation.Core.Logging;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        this.logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        const string prefix = nameof(LoggingBehavior<TRequest, TResponse>);

        this.logger.LogInformation("[{Prefix}] Handle request={X-RequestData} and response={X-ResponseData}",
            prefix, typeof(TRequest).Name, typeof(TResponse).Name);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();
        var timeTaken = timer.Elapsed;
        if (timeTaken.Seconds > 3) // if the request is greater than 3 seconds, then log the warnings
            this.logger.LogWarning(
                "[{Perf-Possible}] The request {X-RequestData} took {TimeTaken} seconds.",
                prefix, typeof(TRequest).Name, timeTaken.Seconds);

        this.logger.LogInformation("[{Prefix}] Handled {X-RequestData}", prefix, typeof(TRequest).Name);
        return response;
    }
}
