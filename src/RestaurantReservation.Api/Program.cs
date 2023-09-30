using Serilog;
using RestaurantReservation.Api.Extensions;
using RestaurantReservation.Core.Logging;
using RestaurantReservation.Core.Mongo;
using RestaurantReservation.Core.Web;
using RestaurantReservation.Core.Web.MinimalApi;
using RestaurantReservation.Core.Web.Swagger;
using RestaurantReservation.Infrastructure.Mongo.Data;
using RestaurantReservation.Infrastructure.Mongo.Data.Configurations;

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

// UseOutputCache must be called after UseCors
app.UseOutputCache();

Serializers.RegisterAll();
app.UseMigration<AppMongoDbContext>(app.Environment);
// app.UseHttpsRedirection();

app.Run();
