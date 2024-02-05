using Asp.Versioning;
using RestaurantReservation.Identity.DTOs;
using Microsoft.OpenApi.Models;

namespace RestaurantReservation.Identity.Api.Endpoints;

public class RegisterNewUserEndpoint : IMinimalApiEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/register-user", async (
                RegisterNewUserRequestDto request, IUserService userService, CancellationToken ct) =>
            {
                var result = await userService.RegisterNewUserAsync(request, ct);
                return Results.Ok(result);
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Description = "Register User",
                Summary = "Register User",
            })
            .WithName("RegisterUser")
            // .RequireAuthorization()
            .Produces<RegisterNewUserResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithApiVersionSet(builder.NewApiVersionSet("Identity").Build())
            .HasApiVersion(new ApiVersion(1, 0));

        return builder;
    }
}
