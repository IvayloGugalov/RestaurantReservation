using RestaurantReservation.Core.Extensions;

namespace RestaurantReservation.Infrastructure.Mongo.Data.Configurations;

public static class RestaurantSerialization
{
    public static void Register()
    {
        BsonClassMap.RegisterClassMap<Restaurant>(
            map =>
            {
                map.AutoMap();
                map.MapField(nameof(Restaurant.Reviews).ToCamelCase()).SetElementName(nameof(Restaurant.Reviews));
                map.MapField(nameof(Restaurant.Tables).ToCamelCase()).SetElementName(nameof(Restaurant.Tables));
            });
    }
}
