namespace RestaurantReservation.Infrastructure.Mongo.Data.Configurations;

public static class ReservationSerialization
{
    public static void Register()
    {
        BsonClassMap.RegisterClassMap<Reservation>(
            map =>
            {
                map.AutoMap();
            });
    }
}
