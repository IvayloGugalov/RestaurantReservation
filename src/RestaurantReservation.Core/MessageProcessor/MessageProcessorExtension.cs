using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RestaurantReservation.Core.MessageProcessor.Data;
using RestaurantReservation.Core.Web;

namespace RestaurantReservation.Core.MessageProcessor;

public static class MessageProcessorExtension
{
    public static WebApplicationBuilder AddPersistMessageProcessor(this WebApplicationBuilder builder, IWebHostEnvironment env)
    {
        builder.Services.AddValidateOptions<MessageOptions>();
        builder.Services.AddScoped<IMessageDbContext, MessageDbContext>();
        builder.Services.AddScoped<IMessageProcessor, MessageProcessor>();

        if (env.EnvironmentName != "test")
        {
            builder.Services.AddHostedService<PersistMessageBackgroundService>();
        }

        return builder;
    }
}
