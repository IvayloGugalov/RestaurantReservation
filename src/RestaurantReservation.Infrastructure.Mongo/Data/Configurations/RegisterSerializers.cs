namespace RestaurantReservation.Infrastructure.Mongo.Data.Configurations;

public static class RegisterSerializers
{
    public static void Register()
    {
        try
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

            RegisterTypedIds();
            RegisterBaseEntities();

            CustomerSerialization.Register();
            RestaurantSerialization.Register();
            ReservationSerialization.Register();
            ReviewSerialization.Register();
            TableSerialization.Register();
        }
        catch
        {
            // ignored
        }
    }

    private static void RegisterBaseEntities()
    {
        RegisterBaseEntityMap<Entity<CustomerId>, CustomerId>(
            new IdSerializationProvider<CustomerId>(new GuidSerializer(BsonType.String)));
        RegisterBaseEntityMap<Entity<RestaurantId>, RestaurantId>(
            new IdSerializationProvider<RestaurantId>(new GuidSerializer(BsonType.String)));
        RegisterBaseEntityMap<Entity<ReservationId>, ReservationId>(
            new IdSerializationProvider<ReservationId>(new GuidSerializer(BsonType.String)));
        RegisterBaseEntityMap<Entity<ReviewId>, ReviewId>(
            new IdSerializationProvider<ReviewId>(new GuidSerializer(BsonType.String)));
        RegisterBaseEntityMap<Entity<TableId>, TableId>(
            new IdSerializationProvider<TableId>(new GuidSerializer(BsonType.String)));
    }

    private static void RegisterTypedIds()
    {
        BsonClassMap.RegisterClassMap<StronglyTypedId<Guid>>(
            map =>
            {
                map.AutoMap();
                map.MapProperty(x => x.Value).SetSerializer(new GuidSerializer(BsonType.String));
                map.SetIsRootClass(true);
            });
    }

    private static void RegisterBaseEntityMap<T, TId>(IBsonSerializer idSerializer)
        where T : Entity<TId>
        where TId : IEquatable<TId>
    {
        BsonClassMap.RegisterClassMap<T>(
            map =>
            {
                map.AutoMap();
                map.MapProperty(x => x.Id).SetSerializer(idSerializer);
                map.SetIsRootClass(true);
            });
    }
}
