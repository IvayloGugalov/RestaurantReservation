namespace RestaurantReservation.Infrastructure.Mongo.Data.Configurations;

public static class StronglyTypedIdSerialization
{
    public static void RegisterTypedIds()
    {
        BsonSerializer.RegisterSerializer(new StronglyTypedIdSerializer<CustomerId, Guid>());
        BsonSerializer.RegisterSerializer(new StronglyTypedIdSerializer<RestaurantId, Guid>());
        BsonSerializer.RegisterSerializer(new StronglyTypedIdSerializer<ReservationId, Guid>());
        BsonSerializer.RegisterSerializer(new StronglyTypedIdSerializer<ReviewId, Guid>());
        BsonSerializer.RegisterSerializer(new StronglyTypedIdSerializer<TableId, Guid>());

        BsonClassMap.RegisterClassMap<StronglyTypedId<Guid>>(
            map =>
            {
                map.AutoMap();
                map.MapProperty(x => x.Value).SetSerializer(new GuidSerializer(BsonType.String));
                map.SetIsRootClass(true);
            });
    }
}
