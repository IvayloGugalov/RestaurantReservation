namespace RestaurantReservation.Infrastructure.Mongo.Data.Configurations;

public static class TableSerialization
{
    public static void Register()
    {
        BsonClassMap.RegisterClassMap<Table>(
            map =>
            {
                map.AutoMap();
                map.MapProperty(x => x.RestaurantId).SetSerializer(
                    new IdSerializationProvider<RestaurantId>(new GuidSerializer(BsonType.String)));
                map.SetIsRootClass(true);
            });
    }
}
