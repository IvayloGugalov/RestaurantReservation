namespace RestaurantReservation.Infrastructure.Mongo.Data.Configurations;

internal sealed class IdSerializationProvider<TId> : SerializerBase<TId>, IBsonDocumentSerializer
    where TId : IEquatable<TId>
{
    private readonly IBsonSerializer<Guid> guidSerializer;

    public IdSerializationProvider(IBsonSerializer<Guid> guidSerializer)
    {
        this.guidSerializer = guidSerializer;
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TId id)
    {
        if (id is StronglyTypedId<Guid> guidId)
        {
            this.guidSerializer.Serialize(context, args, guidId.Value);
        }
        else
        {
            throw new InvalidOperationException($"Unsupported ID type: {typeof(TId).Name}");
        }
    }

    // MongoDB driver performs the deserialization of documents internally.
    public override TId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonType = context.Reader.GetCurrentBsonType();
        if (bsonType == BsonType.String)
        {
            var idValue = context.Reader.ReadString();
            // Parse the ID value and construct your strongly typed ID.
            var guidId = Activator.CreateInstance(typeof(TId), Guid.Parse(idValue));
            return (TId)guidId!;
        }
        throw new FormatException($"Cannot deserialize {bsonType} to {typeof(TId).Name}.");
    }

    // MongoDB performs the filtering and matching inside the driver.
    // When using LINQ
    public bool TryGetMemberSerializationInfo(string memberName, out BsonSerializationInfo serializationInfo)
    {
        serializationInfo = null!;
        var propTypeName = ValueType.GetProperty(memberName)?.PropertyType.Name;
        if (propTypeName == nameof(Guid) && memberName == "Value")
        {
            serializationInfo = new BsonSerializationInfo("_id", this.guidSerializer, this.guidSerializer.ValueType);
        }
        var result = serializationInfo?.Serializer != null;
        return result;
    }

    private static IBsonSerializer? GetProperlyBsonSerializer(string? propName)
    {
        return propName switch
        {
            "Guid" => new GuidSerializer(BsonType.String),
            _ => null
        };
    }
}
