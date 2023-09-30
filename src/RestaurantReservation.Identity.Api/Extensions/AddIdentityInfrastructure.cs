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

        builder.AddSerilog();
        builder.Services
            .AddEndpointsApiExplorer()
            .AddCustomDbContext<IdentityContext>()
            .AddJwt()
            .AddCustomRateLimiter()
            .AddCustomHealthCheck(configuration)
            .AddProblemDetails()
            .AddValidatorsFromAssembly(typeof(IdentityRoot).Assembly)
            .AddCustomVersioning()
            .AddCustomSwagger(configuration, typeof(IdentityRoot).Assembly);

        builder.AddCustomIdentityServer();
        builder.Services.AddTransient<IUserService, UserService>();

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
