﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RestaurantReservation.Core.Repository;

namespace RestaurantReservation.Infrastructure.EF.Data.Repository;

public static class RegisterRepositories
{
    public static WebApplicationBuilder AddCustomRepositories(this WebApplicationBuilder builder)
    {
        throw new NotImplementedException("Cant implement postgres");
        builder.Services.AddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
        return builder;
    }
}
