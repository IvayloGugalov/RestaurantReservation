using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace RestaurantReservation.Core.Web.MinimalApi;

public static class MinimalApiEndpointsExtensions
{
    public static WebApplicationBuilder AddMinimalApiEndpoints(this WebApplicationBuilder builder, params Assembly[] assemblies)
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
