using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace RestaurantReservation.Core.Repository;

public static class RegisterRepositories
{
    public static WebApplicationBuilder AddCustomRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
        return builder;
    }
}
