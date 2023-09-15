using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using RestaurantReservation.Core.Model;
using RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;

namespace RestaurantReservation.Infrastructure.Mongo;

internal sealed class IdSerializer: SerializerBase<RestaurantId>, IBsonDocumentSerializer
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, RestaurantId id)
    {
        UpdateIdIfNone(id);
        context.Writer.WriteString(id.StringValue);
    }

    private void UpdateIdIfNone(RestaurantId id)
    {

    }

    public override RestaurantId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var objectId = context.Reader.ReadString();
        return RestaurantId.From(objectId);
    }

    // Maybe we should think of using a better implementation of the repository and call AppMongoDbContext there

    public bool TryGetMemberSerializationInfo(string memberName, out BsonSerializationInfo serializationInfo)
    {
        var propTypeName = ValueType.GetProperty(memberName)?.PropertyType.Name;
        serializationInfo = new BsonSerializationInfo(memberName, GetProperlyBsonSerializer(propTypeName), typeof(string));
        var result = serializationInfo.Serializer != null;
        return result;
    }

    private static IBsonSerializer? GetProperlyBsonSerializer(string? propName)
    {
        return propName switch
        {
            "Int32" => new Int32Serializer(),
            "String" => new StringSerializer(),
            "Guid" => new GuidSerializer(BsonType.String),
            _ => null
        };
    }
}

public class IdSerializationProvider : IBsonSerializationProvider
{
    private readonly IBsonSerializer<Guid> guidSerializer;

    public IdSerializationProvider(IBsonSerializer<Guid> guidSerializer)
    {
        this.guidSerializer = guidSerializer;
    }

    public IBsonSerializer? GetSerializer(Type type)
    {
        if (type.BaseType != null && type.BaseType == typeof(StronglyTypedId<Guid>))
        {
            return new TestModelSerializer(type, guidSerializer);
        }

        return null;
    }
}

public class TestModelSerializer : SerializerBase<StronglyTypedId<Guid>>, IBsonDocumentSerializer
{
    private readonly Type targetType;
    private readonly IBsonSerializer<Guid> guidSerializer;

    public TestModelSerializer(Type targetType, IBsonSerializer<Guid> guidSerializer)
    {
        this.targetType = targetType;
        this.guidSerializer = guidSerializer;
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, StronglyTypedId<Guid> value)
    {
        guidSerializer.Serialize(context, args, value.Value);
    }

    public override StronglyTypedId<Guid> Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var guidString = context.Reader.ReadString();
        if (!Guid.TryParse(guidString, out var guid)) throw new FormatException($"Failed to parse Guid from string: {guidString}");
        var idType = Activator.CreateInstance(targetType, guid);
        return (idType as StronglyTypedId<Guid>)!;
    }

    public bool TryGetMemberSerializationInfo(string memberName, out BsonSerializationInfo serializationInfo)
    {
        switch (memberName)
        {
            case "Value":
                serializationInfo = new BsonSerializationInfo(memberName, new GuidSerializer(), typeof(string));
                return true;
            default:
                serializationInfo = null!;
                return false;
        }
    }
}
