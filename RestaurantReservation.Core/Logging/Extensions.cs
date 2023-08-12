using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

using RestaurantReservation.Core.Web;

namespace RestaurantReservation.Core.Logging;

public static class Extensions
{
    public static WebApplicationBuilder AddSerilog(
        this WebApplicationBuilder builder,
        string sectionName = "Serilog")
    {
        builder.Host.UseSerilog((context, services, loggerConfiguration) =>
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var logOptions = context.Configuration.GetSection(nameof(LogOptions)).Get<LogOptions>();
            var appOptions = context.Configuration.GetSection(nameof(AppOptions)).Get<AppOptions>();

            var logLevel = Enum.TryParse<LogEventLevel>(logOptions.Level, true, out var level)
                ? level
                : LogEventLevel.Information;

            loggerConfiguration
                .MinimumLevel.Is(logLevel)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.WithExceptionDetails()
                .Enrich.WithProperty("Application", builder.Environment.ApplicationName)
                .Enrich.FromLogContext()
                // https://rehansaeed.com/logging-with-serilog-exceptions/
                .Enrich.WithExceptionDetails();

            // if (serilogOptions.UseConsole)
            {
                // https://github.com/serilog/serilog-sinks-async
                loggerConfiguration.WriteTo.Async(writeTo =>
                    writeTo.Console(outputTemplate: logOptions.LogTemplate));
            }

            // if (!string.IsNullOrEmpty(serilogOptions.SeqUrl))
            // {
            //     loggerConfiguration.WriteTo.Seq(serilogOptions.SeqUrl);
            // }
        });

        return builder;
    }
}
