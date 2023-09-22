namespace RestaurantReservation.Api.Endpoints.Reservation;

public class AddReview : IMinimalApiEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/reservation/add-review",
                async (RequestAddReviewDto request, IMediator mediator, CancellationToken ct) =>
                {
                    var command = request.Map();
                    var result = await mediator.Send(command, ct);
                    var response = new ResponseAddReviewDto(result.Id);
                    return Results.Ok(response);
                })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Description = "Add Review to Reservation",
                Summary = "Add Review to Reservation",
            })
            .WithName("Add Review to Reservation")
            .Produces<ResponseAddReviewDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithApiVersionSet(builder.NewApiVersionSet("Reservation").Build())
            .HasApiVersion(new ApiVersion(1, 0));

        return builder;
    }
}
