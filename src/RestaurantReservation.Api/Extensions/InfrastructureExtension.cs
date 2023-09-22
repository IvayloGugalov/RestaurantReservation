using RestaurantReservation.Api.Endpoints;
using RestaurantReservation.Api.Swagger;
using RestaurantReservation.Core.Logging;
using RestaurantReservation.Infrastructure.Mongo;
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
            .AddHttpContextAccessor();
        builder.AddMinimalApiEndpoints();

        builder.Services.AddCustomSwagger(configuration, typeof(RestaurantReservationApi).Assembly);

        #endregion

        builder.Services
            .AddCustomHealthCheck(configuration)
            .AddCustomMediatR()
            .AddMongoDbContext<AppMongoDbContext>(configuration)
            .AddScoped<IDataSeeder, CustomerSeeder>()
            .AddScoped<IDataSeeder, RestaurantSeeder>()
            .AddScoped<IDataSeeder, ReservationSeeder>();
        builder.AddCustomCaching();
        builder.AddSerilog();
    }
}
