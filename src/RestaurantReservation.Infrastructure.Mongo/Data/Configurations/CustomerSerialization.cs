namespace RestaurantReservation.Infrastructure.Mongo.Data.Configurations;

public static class CustomerSerialization
{
    public static void Register()
    {
        BsonClassMap.RegisterClassMap<Customer>(
            map =>
            {
                map.AutoMap();
                map.UnmapMember(x => x.Reservations);
                map.UnmapMember(x => x.FavouriteRestaurants);
            });
    }
}
