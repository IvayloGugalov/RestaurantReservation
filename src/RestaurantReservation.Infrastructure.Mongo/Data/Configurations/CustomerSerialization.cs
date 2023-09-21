using RestaurantReservation.Core.Extensions;
using RestaurantReservation.Domain.Common.ValueObjects;

namespace RestaurantReservation.Infrastructure.Mongo.Data.Configurations;

public static class CustomerSerialization
{
    public static void Register()
    {
        BsonClassMap.RegisterClassMap<Customer>(
            map =>
            {
                map.AutoMap();
                map.MapProperty(e => e.Email).SetElementName(nameof(Customer.Email)).SetSerializer(new EmailSerializer());
                map.MapField(nameof(Customer.FavouriteRestaurants).ToCamelCase()).SetElementName(nameof(Customer.FavouriteRestaurants));
                map.MapField(nameof(Customer.Reservations).ToCamelCase()).SetElementName(nameof(Customer.Reservations));
            });
    }

    private class EmailSerializer : SerializerBase<Email>
    {
        public override Email Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var value = context.Reader.ReadString();
            return new Email(value);
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Email value)
        {
            context.Writer.WriteString(value.Value);
        }
    }
}
