namespace RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;

public sealed record RestaurantId(Guid Value) : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; } = Value;

    public static implicit operator Guid(RestaurantId restaurantId)
    {
        return restaurantId.Value;
    }

}
