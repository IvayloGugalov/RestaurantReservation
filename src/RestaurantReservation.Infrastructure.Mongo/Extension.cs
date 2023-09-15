using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestaurantReservation.Core.Web;
using RestaurantReservation.Infrastructure.Mongo.Data;
using RestaurantReservation.Infrastructure.Mongo.Repositories;

namespace RestaurantReservation.Infrastructure.Mongo;

public static class Extensions
{
    public static IServiceCollection AddMongoDbContext<TContext>(
        this IServiceCollection services, IConfiguration configuration, Action<MongoOptions>? configurator = null)
        where TContext : MongoDbContext
    {
        return services.AddMongoDbContext<TContext, TContext>(configuration, configurator);
    }

    public static IServiceCollection AddMongoDbContext<TContextService, TContextImplementation>(
        this IServiceCollection services, IConfiguration configuration, Action<MongoOptions>? configurator = null)
        where TContextService : IMongoDbContext
        where TContextImplementation : MongoDbContext, TContextService
    {
        services.Configure<MongoOptions>(configuration.GetSection(nameof(MongoOptions)));

        if (configurator is { })
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

        return services;
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
}
