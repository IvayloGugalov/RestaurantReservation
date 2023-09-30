using Figgle;
using RestaurantReservation.Core.Logging;
using RestaurantReservation.Core.Web;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var appOptions = builder.Services.GetOptions<AppOptions>("AppOptions");
Console.WriteLine(FiggleFonts.Slant.Render(appOptions.Name ?? throw new InvalidOperationException("App name is null")));

builder.AddSerilog();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("Yarp"));

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseCorrelationId();
app.UseRouting();
app.UseRateLimiter();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    endpoints.MapReverseProxy();
});
#pragma warning restore ASP0014

app.MapGet("/", x => x.Response.WriteAsync(appOptions.Name));

app.Run();
