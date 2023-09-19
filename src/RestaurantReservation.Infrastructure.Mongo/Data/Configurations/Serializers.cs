namespace RestaurantReservation.Infrastructure.Mongo.Data.Configurations;

public static class Serializers
{
    public static void RegisterAll()
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

        StronglyTypedIdSerialization.RegisterTypedIds();
        EntitySerialization.RegisterBaseEntities();

        CustomerSerialization.Register();
        RestaurantSerialization.Register();
        ReservationSerialization.Register();
        ReviewSerialization.Register();
        TableSerialization.Register();
    }
}
