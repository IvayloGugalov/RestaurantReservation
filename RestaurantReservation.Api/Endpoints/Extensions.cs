using System.Reflection;

namespace RestaurantReservation.Api.Endpoints;

public static class Extensions
{
    public static WebApplicationBuilder AddMinimalApiEndpoints(
        this WebApplicationBuilder builder)
    {
        var classes = Assembly.GetCallingAssembly().GetTypes()
            .Where(c => typeof(IMinimalApiEndpoint).IsAssignableFrom(c) && !c.IsInterface);

        foreach (var type in classes)
        {
            builder.Services.AddScoped(typeof(IMinimalApiEndpoint), type);
        }

        return builder;
    }

    public static IEndpointRouteBuilder MapMinimalApiEndpoints(this IEndpointRouteBuilder builder)
    {
        var scope = builder.ServiceProvider.CreateScope();
        var endpoints = scope.ServiceProvider.GetServices<IMinimalApiEndpoint>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return builder;
    }
}
