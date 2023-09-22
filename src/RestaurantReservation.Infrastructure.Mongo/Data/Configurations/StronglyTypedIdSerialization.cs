using System.Reflection;
using RestaurantReservation.Domain;

namespace RestaurantReservation.Infrastructure.Mongo.Data.Configurations;

public static class StronglyTypedIdSerialization
{
    public static void RegisterTypedIds()
    {
        var aggregateIdTypes = Assembly.GetAssembly(typeof(RestaurantReservationDomain))
            ?.GetTypes()
            .Where(t => t is { IsAbstract: false, BaseType.IsGenericType: true } &&
                        t.BaseType.GetGenericTypeDefinition() == typeof(StronglyTypedId<>))
            .ToArray();

        foreach (var type in aggregateIdTypes!)
        {
            var serializerType = typeof(StronglyTypedIdSerializer<,>).MakeGenericType(type, type.BaseType!.GetGenericArguments()[0]);
            var serializerInstance = (IBsonSerializer)Activator.CreateInstance(serializerType)!;

            BsonSerializer.RegisterSerializer(type, serializerInstance);
        }

        BsonClassMap.RegisterClassMap<StronglyTypedId<Guid>>(
            map =>
            {
                map.AutoMap();
                map.MapProperty(x => x.Value).SetSerializer(new GuidSerializer(BsonType.String));
                map.SetIsRootClass(true);
            });
    }
}
