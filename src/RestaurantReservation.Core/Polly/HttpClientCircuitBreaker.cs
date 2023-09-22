using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Polly;
using Polly.Extensions.Http;

using RestaurantReservation.Core.Web;

namespace RestaurantReservation.Core.Polly;

public static class HttpClientCircuitBreaker
{
    public static IHttpClientBuilder AddHttpClientCircuitBreakerPolicyHandler(this IHttpClientBuilder httpClientBuilder)
    {
        return httpClientBuilder.AddPolicyHandler((sp, _) =>
        {
            var options = sp.GetRequiredService<IConfiguration>().GetOptions<PolicyOptions>(nameof(PolicyOptions));

            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("PollyHttpClientCircuitBreakerPoliciesLogger");

            return HttpPolicyExtensions.HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.BadRequest)
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: options.CircuitBreaker?.RetryCount ?? 3,
                    durationOfBreak: TimeSpan.FromSeconds(options.CircuitBreaker?.BreakDuration ?? 60),
                    onBreak: (response, breakDuration) =>
                    {
                        if (response?.Exception != null)
                        {
                            logger.LogError(response.Exception,
                                "Service shutdown during {BreakDuration} after {RetryCount} failed retries",
                                breakDuration,
                                options.CircuitBreaker?.RetryCount ?? 3);
                        }
                    },
                    onReset: () =>
                    {
                        logger.LogInformation("Service restarted");
                    });
        });
    }
}
