using RestaurantReservation.Api.Endpoints;
using RestaurantReservation.Api.Extensions;
using RestaurantReservation.Api.Swagger;
using RestaurantReservation.Core.Logging;
using RestaurantReservation.Core.Web;
using RestaurantReservation.Infrastructure.EF;
using RestaurantReservation.Infrastructure.EF.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddInfrastructure();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCustomSwagger();
}

app.UseSerilogRequestLogging(options => { options.EnrichDiagnosticContext = LogEnrichHelper.EnrichFromRequest; });

app.MapMinimalApiEndpoints();
app.UseCorrelationId();
app.UseCustomExceptionHandler();
app.UseCustomHealthCheck();
app.UseMigration<AppDbContext>(app.Environment);
// app.UseHttpsRedirection();

app.Run();