﻿namespace RestaurantReservation.Infrastructure.Mongo.Data.Configurations;

public static class TableSerialization
{
    public static void Register()
    {
        BsonClassMap.RegisterClassMap<Table>(
            map =>
            {
                map.AutoMap();
                map.UnmapMember(x => x.Reservations);

                map.MapProperty(x => x.RestaurantId)
                    .SetSerializer(new IdSerializationProvider<RestaurantId>(new GuidSerializer(BsonType.String)));

            });
    }
}
