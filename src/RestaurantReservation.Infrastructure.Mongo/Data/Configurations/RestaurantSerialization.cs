namespace RestaurantReservation.Infrastructure.Mongo.Data.Configurations;

public static class RestaurantSerialization
{
    public static void Register()
    {
        BsonClassMap.RegisterClassMap<Restaurant>(
            map =>
            {
                map.AutoMap();
                map.UnmapMember(x => x.Reviews);
                map.UnmapMember(x => x.Tables);
            });
    }
}
