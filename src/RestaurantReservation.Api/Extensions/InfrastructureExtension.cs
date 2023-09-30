using RestaurantReservation.Core.Authentication;
using RestaurantReservation.Core.Logging;
using RestaurantReservation.Core.Mongo;
using RestaurantReservation.Core.Mongo.Data;
using RestaurantReservation.Core.Web;
using RestaurantReservation.Core.Web.MinimalApi;
using RestaurantReservation.Core.Web.Swagger;
using RestaurantReservation.Infrastructure.Mongo.Data;
using RestaurantReservation.Infrastructure.Mongo.Seeders;

namespace RestaurantReservation.Api.Extensions;

public static class InfrastructureExtension
{
    public static void AddInfrastructure(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        #region Api

        builder.Services
            .AddProblemDetails()
            .AddEndpointsApiExplorer()
            .AddCustomVersioning()
            .AddHttpContextAccessor()
            .AddCustomRateLimiter();
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
