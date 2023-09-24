using Microsoft.AspNetCore.Routing;

namespace RestaurantReservation.Core.Web;

public interface IMinimalApiEndpoint
{
    IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder);
}
