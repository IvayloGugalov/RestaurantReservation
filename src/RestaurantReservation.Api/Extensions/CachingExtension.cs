namespace RestaurantReservation.Api.Extensions;

public static class CachingExtension
{
    public static WebApplicationBuilder AddCustomCaching(this WebApplicationBuilder builder)
    {
        builder.Services.AddOutputCache(options =>
        {
            options.AddBasePolicy(b =>
            {
                b.Cache();
                b.Expire(TimeSpan.FromMinutes(2));
            });
        });

        return builder;
    }
}
