
namespace RestaurantReservation.Api.Endpoints.Reservation;

public class Create : IMinimalApiEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/reservation/create",
                async (RequestCreateReservationDto request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var command = ReservationMappings.Map(request);
                    var result = await mediator.Send(command, cancellationToken);
                    var response = ReservationMappings.Map(result);
                    return Results.Ok(response);
                })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Description = "Create Reservation",
                Summary = "Create Reservation",
            })
            .WithName("Create Reservation")
            .Produces<ResponseCreateReservationDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithApiVersionSet(builder.NewApiVersionSet("Reservation").Build())
            .HasApiVersion(new ApiVersion(1, 0));

        return builder;
    }
}
