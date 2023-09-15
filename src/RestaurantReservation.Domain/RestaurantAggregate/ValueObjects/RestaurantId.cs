namespace RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;

public sealed record RestaurantId(Guid Value) : StronglyTypedId<Guid>(Value)
{
    public static implicit operator Guid(RestaurantId restaurantId)
    {
        return restaurantId.Value;
    }

    public static RestaurantId From(string value)
    {
        var to = Guid.Parse(value);
        return new RestaurantId(to);
    }
}
