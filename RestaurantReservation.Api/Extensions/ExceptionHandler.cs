using Microsoft.AspNetCore.Diagnostics;
using RestaurantReservation.Core.Exceptions;

namespace RestaurantReservation.Api.Extensions;

public static class ExceptionHandler
{
    public static IApplicationBuilder UseCustomExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                context.Response.ContentType = "application/problem+json";

                if (context.RequestServices.GetService<IProblemDetailsService>() is { } problemDetailsService)
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exceptionType = exceptionHandlerFeature?.Error;

                    if (exceptionType is not null)
                    {
                        (string Detail, string Title, int StatusCode) details = exceptionType switch
                        {
                            ConflictException =>
                            (
                                exceptionType.Message,
                                exceptionType.GetType().Name,
                                context.Response.StatusCode = StatusCodes.Status409Conflict
                            ),
                            BadRequestException =>
                            (
                                exceptionType.Message,
                                exceptionType.GetType().Name,
                                context.Response.StatusCode = StatusCodes.Status400BadRequest
                            ),
                            NotFoundException =>
                            (
                                exceptionType.Message,
                                exceptionType.GetType().Name,
                                context.Response.StatusCode = StatusCodes.Status404NotFound
                            ),
                            _ =>
                            (
                                exceptionType.Message,
                                exceptionType.GetType().Name,
                                context.Response.StatusCode = StatusCodes.Status500InternalServerError
                            )
                        };

                        var problem = new ProblemDetailsContext
                        {
                            HttpContext = context,
                            ProblemDetails =
                            {
                                Title = details.Title,
                                Detail = details.Detail,
                                Status = details.StatusCode
                            }
                        };

                        if (app.Environment.IsDevelopment())
                        {
                            problem.ProblemDetails.Extensions.Add("exception",
                                exceptionHandlerFeature?.Error.ToString());
                        }

                        await problemDetailsService.WriteAsync(problem);
                    }
                }
            });
        });
        return app;
    }
}
