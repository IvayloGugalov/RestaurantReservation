namespace RestaurantReservation.Api.Endpoints.Restaurant;

public class Create : IMinimalApiEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/restaurant/create",
            async (RequestCreateRestaurantDto request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var command = RestaurantMappings.Map(request);
                var result = await mediator.Send(command, cancellationToken);
                var response = RestaurantMappings.Map(result);
                return Results.Ok(response);
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Description = "Create Restaurant",
                Summary = "Create Restaurant"
            })
            .WithName("Create Restaurant")
            .Produces<ResponseCreateRestaurantDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithApiVersionSet(builder.NewApiVersionSet("Restaurant").Build())
            .HasApiVersion(new ApiVersion(1, 0));

        return builder;
    }
}
