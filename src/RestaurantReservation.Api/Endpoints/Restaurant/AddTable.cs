using RestaurantReservation.Core.Web.MinimalApi;

namespace RestaurantReservation.Api.Endpoints.Restaurant;

public class AddTable : IMinimalApiEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/restaurant/add-table",
            async (RequestAddTableDto request, IMediator mediator, CancellationToken ct) =>
            {
                var command = TableMappings.Map(request);
                var result = await mediator.Send(command, ct);
                var response = TableMappings.Map(result);
                return Results.Ok(response);
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Description = "Add Table",
                Summary = "Add Table"
            })
            .WithName("Add Table")
            .Produces<ResponseAddTableDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithApiVersionSet(builder.NewApiVersionSet("Table").Build())
            .HasApiVersion(new ApiVersion(1, 0));

        builder.MapPost($"{EndpointConfig.BaseApiPath}/restaurant/add-tables",
            async (RequestAddMultipleTablesDto request, IMediator mediator, CancellationToken ct) =>
            {
                var command = TableMappings.Map(request);
                var result = await mediator.Send(command, ct);
                var response = TableMappings.Map(result);
                return Results.Ok(response);
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Description = "Add Multiple Table",
                Summary = "Add Multiple Table"
            })
            .WithName("Add Multiple Table")
            .Produces<ResponseAddMultipleTablesDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithApiVersionSet(builder.NewApiVersionSet("Table").Build())
            .HasApiVersion(new ApiVersion(1, 0));

        return builder;
    }
}
