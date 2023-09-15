using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestaurantReservation.Core.Model;

public abstract record StronglyTypedId<TValue>(TValue Value) where TValue : IEquatable<TValue>
{
    public string StringValue => this.Value.ToString() ?? throw new Exception("Id values is empty");
}

