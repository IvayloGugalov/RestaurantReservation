using System.Threading.RateLimiting;
using FluentValidation;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Core.Authentication;
using RestaurantReservation.Core.Logging;
using RestaurantReservation.Core.Web;
using RestaurantReservation.Identity.Data;
using RestaurantReservation.Identity.Extensions;
using RestaurantReservation.Core.EFCore;
using RestaurantReservation.Core.Web.MinimalApi;
using RestaurantReservation.Identity.Services;
using Serilog;

namespace RestaurantReservation.Identity.Api.Extensions;

public static class AddIdentityInfrastructure
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var env = builder.Environment;

        builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        // builder.Services.AddScoped<IEventMapper, EventMapper>();
        // builder.Services.AddScoped<IEventDispatcher, EventDispatcher>();

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        var appOptions = builder.Services.GetOptions<AppOptions>(nameof(AppOptions));

        builder.Services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true, PermitLimit = 10, QueueLimit = 0, Window = TimeSpan.FromMinutes(1)
                    }));
        });

        builder.Services.AddEndpointsApiExplorer();
        // builder.Services.AddPersistMessageProcessor(env);
        builder.Services.AddCustomDbContext<IdentityContext>();
        // builder.Services.AddScoped<IDataSeeder, IdentityDataSeeder>();
        builder.AddSerilog();
        builder.Services.AddJwt();
        builder.Services.AddCustomSwagger(configuration, typeof(IdentityRoot).Assembly);
        builder.Services.AddCustomVersioning();
        // builder.Services.AddCustomMediatR();
        builder.Services.AddValidatorsFromAssembly(typeof(IdentityRoot).Assembly);
        builder.Services.AddProblemDetails();
        builder.Services.AddCustomHealthCheck(configuration);

        // builder.Services.AddCustomMassTransit(env, typeof(IdentityRoot).Assembly);

        builder.Services.AddTransient<IUserService, UserService>();

        builder.AddCustomIdentityServer();

        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

        return builder;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        var env = app.Environment;
        var appOptions = app.GetOptions<AppOptions>(nameof(AppOptions));

        app.UseForwardedHeaders();

        app.UseCustomExceptionHandler();
        app.UseSerilogRequestLogging(options =>
        {
            options.EnrichDiagnosticContext = LogEnrichHelper.EnrichFromRequest;
        });
        app.UseMigration<IdentityContext>(env);
        app.UseCorrelationId();
        app.UseCustomHealthCheck();
        app.UseIdentityServer();

        app.MapGet("/", x => x.Response.WriteAsync("Identity"));

        if (!env.IsProduction())
        {
            app.UseCustomSwagger();
        }

        return app;
    }
}
