using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RestaurantReservation.Core.Web;
using RestaurantReservation.Infrastructure.Mongo;

namespace RestaurantReservation.Api.Extensions;

public class HealthOptions
{
    public bool Enabled { get; set; } = true;
}

public static class HealthCheckExtension
{
    public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var healthOptions = configuration.GetSection(nameof(HealthOptions)).Get<HealthOptions>();

        if (!healthOptions?.Enabled ?? true) return services;

        var appOptions = configuration.GetSection(nameof(AppOptions)).Get<AppOptions>();
        // var postgresOptions = services.GetSection(nameof(PostgresOptions)).Get<PostgresOptions>();
        // var rabbitMqOptions = services.GetOptions<RabbitMqOptions>(nameof(RabbitMqOptions));
        var mongoOptions = services.GetOptions<MongoOptions>(nameof(MongoOptions));
        // var logOptions = configuration.GetSection(nameof(LogOptions)).Get<LogOptions>();

        var healthChecksBuilder = services.AddHealthChecks();
        // .AddRabbitMQ(
        //     rabbitConnectionString:
        //     $"amqp://{rabbitMqOptions.UserName}:{rabbitMqOptions.Password}@{rabbitMqOptions.HostName}")
        // .AddElasticsearch(logOptions.Elastic.ElasticServiceUrl);

        if (mongoOptions.ConnectionString is not null) healthChecksBuilder.AddMongoDb(mongoOptions.ConnectionString, mongoOptions.DatabaseName);

        // if (postgresOptions.ConnectionString is not null)
        // healthChecksBuilder.AddNpgSql(postgresOptions.ConnectionString);

        services.AddHealthChecksUI(setup =>
        {
            setup.SetEvaluationTimeInSeconds(60); // time in seconds between check
            setup.AddHealthCheckEndpoint($"Basic Health Check - {appOptions?.Name ?? nameof(RestaurantReservation)}", "/healthz");
        }).AddInMemoryStorage();

        return services;
    }

    public static WebApplication UseCustomHealthCheck(this WebApplication app)
    {
        var healthOptions = app.Configuration.GetSection(nameof(HealthOptions)).Get<HealthOptions>();

        if (!healthOptions?.Enabled ?? false) return app;

        app.UseHealthChecks("/healthz",
                new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                    ResultStatusCodes =
                    {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    }
                })
            .UseHealthChecksUI(options =>
            {
                options.ApiPath = "/healthcheck";
                options.UIPath = "/healthcheck-ui";
            });

        return app;
    }
}
