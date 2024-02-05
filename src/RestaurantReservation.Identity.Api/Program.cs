using RestaurantReservation.Identity;
using RestaurantReservation.Identity.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

builder.AddMinimalApiEndpoints(typeof(IdentityRoot).Assembly);
builder.AddInfrastructure();

var app = builder.Build();

if (!env.IsProduction())
{
    app.MapGet("/api/v1", () => "identity");
}

app.MapMinimalApiEndpoints();
app.UseAuthentication();
app.UseAuthorization();
app.UseInfrastructure();

app.Run();
