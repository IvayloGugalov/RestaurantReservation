namespace RestaurantReservation.Infrastructure.Mongo.Data.Configurations;

public static class ReviewSerialization
{
    public static void Register()
    {
        BsonClassMap.RegisterClassMap<Review>(
            map =>
            {
                map.AutoMap();
                map.MapProperty(e => e.Rating).SetElementName(nameof(Review.Rating)).SetSerializer(new RatingSerializer());
            });
    }

    private class RatingSerializer : SerializerBase<Rating>
    {
        public override Rating Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var value = context.Reader.ReadDouble();
            return new Rating(value);
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Rating rating)
        {
            context.Writer.WriteDouble(rating.Value);
        }
    }
}
