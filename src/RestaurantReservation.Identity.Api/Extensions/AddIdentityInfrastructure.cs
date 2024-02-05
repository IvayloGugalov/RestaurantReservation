using MediatR;
using RestaurantReservation.Core.Events;
using RestaurantReservation.Core.MassTransit;
using RestaurantReservation.Core.MessageProcessor;
using RestaurantReservation.Core.Mongo.Data;
using RestaurantReservation.Core.Validation;
using RestaurantReservation.Identity.Configuration;
using RestaurantReservation.Identity.Data.Seeders;

namespace RestaurantReservation.Identity.Api.Extensions;

public static class AddIdentityInfrastructure
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var env = builder.Environment;

        builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        builder.Services.AddScoped<IEventMapper, EventMapper>();
        builder.Services.AddScoped<IEventDispatcher, EventDispatcher>();

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        var appOptions = builder.Services.GetOptions<AppOptions>(nameof(AppOptions));

        builder.Services
            .AddEndpointsApiExplorer()
            .AddCustomMediatR()
            .AddCustomMassTransit(builder.Environment, typeof(IdentityRoot).Assembly)
            .AddJwt()
            .AddCustomRateLimiter()
            .AddCustomHealthCheck(configuration)
            .AddProblemDetails()
            .AddValidatorsFromAssembly(typeof(IdentityRoot).Assembly)
            .AddCustomVersioning()
            .AddCustomDbContext<IdentityContext>()
            .AddCustomSwagger(configuration, typeof(IdentityRoot).Assembly);

        builder
            .AddCustomIdentityServer()
            .AddSerilog()
            .AddPersistMessageProcessor(env);
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddScoped<IEventDispatcher, EventDispatcher>();
        builder.Services.AddScoped<IDataSeeder, RoleAndUserSeeder>();

        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

        return builder;
    }

    private static IServiceCollection AddCustomMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IdentityRoot).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return services;
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

        if (!env.IsProduction())
        {
            app.UseCustomSwagger();
        }

        return app;
    }
}
