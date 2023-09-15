namespace RestaurantReservation.Infrastructure.Mongo.Data.Configurations;

public static class ReservationSerialization
{
    public static void Register()
    {
        BsonClassMap.RegisterClassMap<Reservation>(
            map =>
            {
                map.AutoMap();
                map.MapProperty(x => x.RestaurantId).SetSerializer(
                    new IdSerializationProvider<RestaurantId>(new GuidSerializer(BsonType.String)));
                map.MapProperty(x => x.CustomerId).SetSerializer(
                    new IdSerializationProvider<CustomerId>(new GuidSerializer(BsonType.String)));
                map.MapProperty(x => x.TableId).SetSerializer(
                    new IdSerializationProvider<TableId>(new GuidSerializer(BsonType.String)));
                map.MapProperty(x => x.ReviewId).SetSerializer(
                    new IdSerializationProvider<ReviewId>(new GuidSerializer(BsonType.String)));
                map.SetIsRootClass(true);
            });
    }
}
