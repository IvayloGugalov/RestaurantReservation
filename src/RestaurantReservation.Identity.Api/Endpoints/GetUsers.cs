using Asp.Versioning;
using Microsoft.OpenApi.Models;
using RestaurantReservation.Core.Web.MinimalApi;
using RestaurantReservation.Identity.Models;
using RestaurantReservation.Identity.Services;

namespace RestaurantReservation.Identity.Api.Endpoints;

public class GetUsers : IMinimalApiEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet($"{EndpointConfig.BaseApiPath}/identity/users", async (
            IUserService userService, CancellationToken ct) => await userService.GetUsersAsync())
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Description = "Get Users",
            Summary = "Get Users",
        })
        .WithName("GetUsers")
        // .RequireAuthorization()
        .Produces<List<User>>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithApiVersionSet(builder.NewApiVersionSet("Identity").Build())
        .HasApiVersion(new ApiVersion(1, 0));

        return builder;
    }
}
