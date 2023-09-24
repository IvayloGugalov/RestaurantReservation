using Microsoft.AspNetCore.Routing;

namespace RestaurantReservation.Core.Web.MinimalApi;

public interface IMinimalApiEndpoint
{
    IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder);
}
