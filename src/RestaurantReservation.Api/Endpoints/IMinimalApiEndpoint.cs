namespace RestaurantReservation.Api.Endpoints;

public interface IMinimalApiEndpoint
{
    IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder);
}