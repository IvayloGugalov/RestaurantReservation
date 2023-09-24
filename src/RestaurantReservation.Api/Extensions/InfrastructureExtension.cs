using System.Threading.RateLimiting;
using RestaurantReservation.Core.Authentication;
using RestaurantReservation.Core.Logging;
using RestaurantReservation.Core.Mongo;
using RestaurantReservation.Core.Mongo.Data;
using RestaurantReservation.Core.Web;
using RestaurantReservation.Core.Web.MinimalApi;
using RestaurantReservation.Infrastructure.Mongo.Data;
using RestaurantReservation.Infrastructure.Mongo.Seeders;

namespace RestaurantReservation.Api.Extensions;

public static class InfrastructureExtension
{
    public static void AddInfrastructure(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

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

        #region Api

        builder.Services
            .AddProblemDetails()
            .AddEndpointsApiExplorer()
            .AddCustomVersioning()
            .AddHttpContextAccessor();
        builder.AddMinimalApiEndpoints();

        builder.Services.AddCustomSwagger(configuration, typeof(RestaurantReservationApi).Assembly);

        #endregion

        builder.Services
            .AddCustomHealthCheck(configuration)
            .AddCustomMediatR()
            .AddJwt()
            .AddMongoDbContext<AppMongoDbContext>(configuration)
            .AddScoped<ICurrentUserProvider, CurrentUserProvider>()
            .AddScoped<IDataSeeder, CustomerSeeder>()
            .AddScoped<IDataSeeder, RestaurantSeeder>()
            .AddScoped<IDataSeeder, ReservationSeeder>();
        builder.AddCustomCaching();
        builder.AddSerilog();
    }
}
