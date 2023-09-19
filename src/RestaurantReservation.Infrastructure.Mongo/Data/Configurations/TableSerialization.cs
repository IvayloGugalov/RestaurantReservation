using RestaurantReservation.Core.Extensions;

namespace RestaurantReservation.Infrastructure.Mongo.Data.Configurations;

public static class TableSerialization
{
    public static void Register()
    {
        BsonClassMap.RegisterClassMap<Table>(
            map =>
            {
                map.AutoMap();
                map.MapField(nameof(Table.Reservations).ToCamelCase()).SetElementName(nameof(Table.Reservations));
            });
    }
}
