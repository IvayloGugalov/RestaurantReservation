using RestaurantReservation.Api.Endpoints;
using RestaurantReservation.Api.Swagger;
using RestaurantReservation.Core.Logging;
using RestaurantReservation.Infrastructure.EF;
using RestaurantReservation.Infrastructure.EF.Data;
using RestaurantReservation.Infrastructure.EF.Data.Repository;

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
        builder.Services.AddCustomDbContext<AppDbContext>();
        builder.AddCustomRepositories();
        builder.AddSerilog();
    }
}