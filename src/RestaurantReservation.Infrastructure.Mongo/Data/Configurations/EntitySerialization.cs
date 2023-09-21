namespace RestaurantReservation.Infrastructure.Mongo.Data.Configurations;

public static class EntitySerialization
{
    public static void RegisterBaseEntities()
    {
        RegisterBaseEntityMap<Entity<CustomerId>, CustomerId>();
        RegisterBaseEntityMap<Entity<RestaurantId>, RestaurantId>();
        RegisterBaseEntityMap<Entity<ReservationId>, ReservationId>();
        RegisterBaseEntityMap<Entity<ReviewId>, ReviewId>();
        RegisterBaseEntityMap<Entity<TableId>, TableId>();
    }

    private static void RegisterBaseEntityMap<T, TId>()
        where T : Entity<TId>
        where TId : IEquatable<TId>
    {
        BsonClassMap.RegisterClassMap<T>(
            map =>
            {
                map.AutoMap();
                map.SetIsRootClass(true);
            });
    }
}
