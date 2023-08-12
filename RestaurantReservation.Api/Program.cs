using RestaurantReservation.Api;
using RestaurantReservation.Api.Endpoints;
using RestaurantReservation.Api.Extensions;
using RestaurantReservation.Api.Swagger;
using RestaurantReservation.Core.Logging;
using RestaurantReservation.Core.Repository;
using RestaurantReservation.Core.Web;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomVersioning();
builder.Services.AddCustomMediatR();
builder.AddCustomRepositories();
builder.AddSerilog();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCustomSwagger(configuration, typeof(RestaurantReservationApi).Assembly);
builder.Services.AddApiVersioning();
builder.AddMinimalApiEndpoints();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCustomSwagger();
}

app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = LogEnrichHelper.EnrichFromRequest;
});

app.MapMinimalApiEndpoints();
app.UseCorrelationId();
app.UseCustomExceptionHandler();
// app.UseHttpsRedirection();

app.Run();
