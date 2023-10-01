using RestaurantReservation.Identity;
using RestaurantReservation.Identity.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseDefaultServiceProvider((context, options) =>
{
    // Service provider validation
    // ref: https://andrewlock.net/new-in-asp-net-core-3-service-provider-validation/
    options.ValidateScopes = context.HostingEnvironment.IsDevelopment() || context.HostingEnvironment.IsStaging();
    options.ValidateOnBuild = true;
});

builder.AddMinimalApiEndpoints(typeof(IdentityRoot).Assembly);
builder.AddInfrastructure();

var app = builder.Build();

app.MapMinimalApiEndpoints();
app.UseAuthentication();
app.UseAuthorization();
app.UseInfrastructure();


app.Run();
