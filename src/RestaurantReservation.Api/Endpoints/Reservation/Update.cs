namespace RestaurantReservation.Api.Endpoints.Reservation;

public class Update : IMinimalApiEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPut($"{EndpointConfig.BaseApiPath}/reservation/update",
                async (RequestUpdateReservation request, IMediator mediator, CancellationToken ct) =>
                {
                    var command = request.Map();
                    var result = await mediator.Send(command, ct);
                    var response = result.Map();
                    return Results.Ok(response);
                })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Description = "Update Reservation",
                Summary = "Update Reservation",
            })
            .WithName("Update Reservation")
            .Produces<ResponseUpdateReservation>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithApiVersionSet(builder.NewApiVersionSet("Reservation").Build())
            .HasApiVersion(new ApiVersion(1, 0));

        return builder;
    }
}
