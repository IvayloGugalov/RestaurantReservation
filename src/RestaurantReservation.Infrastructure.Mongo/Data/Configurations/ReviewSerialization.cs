namespace RestaurantReservation.Infrastructure.Mongo.Data.Configurations;

public static class ReviewSerialization
{
    public static void Register()
    {
        BsonClassMap.RegisterClassMap<Review>(
            map =>
            {
                map.AutoMap();
                map.MapProperty(x => x.RestaurantId).SetSerializer(
                    new IdSerializationProvider<RestaurantId>(new GuidSerializer(BsonType.String)));
                map.MapProperty(x => x.CustomerId).SetSerializer(
                    new IdSerializationProvider<CustomerId>(new GuidSerializer(BsonType.String)));
                map.MapProperty(x => x.ReservationId).SetSerializer(
                    new IdSerializationProvider<ReservationId>(new GuidSerializer(BsonType.String)));
                map.SetIsRootClass(true);
            });
    }
}
