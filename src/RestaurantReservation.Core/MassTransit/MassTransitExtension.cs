﻿using System.Reflection;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestaurantReservation.Core.Exceptions;
using RestaurantReservation.Core.Web;

namespace RestaurantReservation.Core.MassTransit;

public static class MassTransitExtension
{
    public static IServiceCollection AddCustomMassTransit(this IServiceCollection services,
        IWebHostEnvironment env, Assembly assembly)
    {
        services.AddValidateOptions<RabbitMqOptions>();

        if (env.IsEnvironment("test"))
        {
            services.AddMassTransitTestHarness(configure =>
            {
                SetupMasstransitConfigurations(services, configure, assembly);
            });
        }
        else
        {
            services.AddMassTransit(configure => SetupMasstransitConfigurations(services, configure, assembly));
        }

        return services;
    }

    private static void SetupMasstransitConfigurations(
        IServiceCollection services, IBusRegistrationConfigurator configure, Assembly assembly)
    {
        configure.AddConsumers(assembly);
        configure.AddSagaStateMachines(assembly);
        configure.AddSagas(assembly);
        configure.AddActivities(assembly);

        configure.UsingRabbitMq((context, configurator) =>
        {
            var rabbitMqOptions = services.GetOptions<RabbitMqOptions>(nameof(RabbitMqOptions));

            configurator.Host(rabbitMqOptions?.HostName, rabbitMqOptions?.Port ?? 5672, "/", h =>
            {
                h.Username(rabbitMqOptions?.UserName);
                h.Password(rabbitMqOptions?.Password);
            });

            configurator.ConfigureEndpoints(context);

            configurator.UseMessageRetry(AddRetryConfiguration);
        });
    }

    private static void AddRetryConfiguration(IRetryConfigurator retryConfigurator)
    {
        retryConfigurator.Exponential(
                3,
                TimeSpan.FromMilliseconds(200),
                TimeSpan.FromMinutes(120),
                TimeSpan.FromMilliseconds(200))
            .Ignore<ValidationException>(); // don't retry if we have invalid data and message goes to _error queue masstransit
    }
}
