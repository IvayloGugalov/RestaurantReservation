namespace RestaurantReservation.Api.Endpoints.Customer;

public class Create : IMinimalApiEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/customer/create",
            async (RequestCreateCustomerDto request, IMediator mediator, CancellationToken ct) =>
            {
                var command = CustomerMappings.Map(request);
                var result = await mediator.Send(command, ct);
                var response = CustomerMappings.Map(result);
                return Results.Ok(response);
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Description = "Create Customer",
                Summary = "Create Customer",
            })
            .WithName("Create Customer")
            .Produces<ResponseCreateCustomerDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithApiVersionSet(builder.NewApiVersionSet("Customer").Build())
            .HasApiVersion(new ApiVersion(1, 0));

        return builder;
    }
}
