using RestaurantReservation.Api.Endpoints;
using RestaurantReservation.Api.Swagger;
using RestaurantReservation.Core.Logging;
using RestaurantReservation.Domain;
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

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddCustomVersioning();
        builder.Services.AddHttpContextAccessor();
        builder.AddMinimalApiEndpoints();

        builder.Services.AddCustomSwagger(configuration, typeof(RestaurantReservationApi).Assembly);

        #endregion

        builder.Services.AddCustomMediatR();
        builder.Services.AddMongoDbContext<AppMongoDbContext>(configuration);
        builder.Services.AddScoped<IDataSeeder, CustomerSeeder>();
        builder.Services.AddScoped<IDataSeeder, RestaurantSeeder>();
        builder.AddSerilog();
    }
}
