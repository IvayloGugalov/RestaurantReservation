using MediatR;
using RestaurantReservation.Core.Logging;
using RestaurantReservation.Domain;

namespace RestaurantReservation.Api.Extensions;

public static class MediatRExtension
{
    public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RestaurantReservationDomain).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return services;
    }
}