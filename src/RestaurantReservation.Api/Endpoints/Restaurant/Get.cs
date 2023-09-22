using Microsoft.AspNetCore.Mvc;

namespace RestaurantReservation.Api.Endpoints.Restaurant;

public class Get : IMinimalApiEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet($"{EndpointConfig.BaseApiPath}/restaurant",
            async ([FromQuery] Guid id, IMediator mediator, CancellationToken ct) =>
            {
                var response = await mediator.Send(new GetRestaurantById(id), ct);
                return Results.Ok(response);
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Description = "Get Restaurant",
                Summary = "Get Restaurant"
            })
            .WithName("Get Restaurant")
            .Produces<ResponseCreateRestaurantDto>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithApiVersionSet(builder.NewApiVersionSet("Restaurant").Build())
            .HasApiVersion(new ApiVersion(1, 0));

        return builder;
    }
}
