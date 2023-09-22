using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using RestaurantReservation.Core.Mongo;
using RestaurantReservation.Core.Web;
using RestaurantReservation.Infrastructure.Mongo.Data.Configurations;

namespace RestaurantReservation.Infrastructure.Mongo;

public static class Extensions
{
    private const string NAME = "mongodb";

    public static IServiceCollection AddMongoDbContext<TContext>(
        this IServiceCollection services, IConfiguration configuration, Action<MongoOptions>? configurator = null)
        where TContext : MongoDbContext
    {
        return services.AddMongoDbContext<TContext, TContext>(configuration, configurator);
    }

    private static IServiceCollection AddMongoDbContext<TContextService, TContextImplementation>(
        this IServiceCollection services, IConfiguration configuration, Action<MongoOptions>? configurator = null)
        where TContextService : IMongoDbContext
        where TContextImplementation : MongoDbContext, TContextService
    {
        services.Configure<MongoOptions>(configuration.GetSection(nameof(MongoOptions)));

        if (configurator is not null)
        {
            services.Configure(nameof(MongoOptions), configurator);
        }
        else
        {
            services.AddValidateOptions<MongoOptions>();
        }

        services.AddScoped(typeof(TContextService), typeof(TContextImplementation));
        services.AddScoped(typeof(TContextImplementation));

        services.AddScoped<IMongoDbContext>(sp => sp.GetRequiredService<TContextService>());

        services.AddTransient(typeof(IMongoRepository<,>), typeof(MongoRepository<,>));
        services.AddTransient(typeof(IMongoUnitOfWork<>), typeof(MongoUnitOfWork<>));

        Serializers.RegisterAll();
        RegisterConventions();

        return services;
    }

    private static void RegisterConventions()
    {
        ConventionRegistry.Register(
            "conventions",
            new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(BsonType.String),
                new IgnoreIfDefaultConvention(false)
            }, _ => true);
    }

    public static IApplicationBuilder UseMigration<TContext>(this IApplicationBuilder app, IWebHostEnvironment env)
        where TContext : MongoDbContext, IMongoDbContext
    {
        // MigrateDatabaseAsync<TContext>(app.ApplicationServices).GetAwaiter().GetResult();

        if (env.IsEnvironment("Development"))
        {
            var serviceProvider = app.ApplicationServices;
            using var scope = serviceProvider.CreateScope();
            var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
            foreach (var seeder in seeders)
            {
                seeder.SeedAllAsync();
            }
        }

        return app;
    }

    public static IHealthChecksBuilder AddMongoDb(
        this IHealthChecksBuilder builder,
        string mongodbConnectionString,
        string? name = default,
        HealthStatus? failureStatus = default,
        IEnumerable<string>? tags = default,
        TimeSpan? timeout = default)
    {
        return builder.Add(new HealthCheckRegistration(
            name ?? NAME,
            sp => new MongoHealthCheck(mongodbConnectionString),
            failureStatus,
            tags,
            timeout));
    }
}
