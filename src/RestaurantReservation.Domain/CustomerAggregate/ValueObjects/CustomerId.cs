namespace RestaurantReservation.Domain.CustomerAggregate.ValueObjects;

public sealed record CustomerId(Guid Value) : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; } = Value;

    public static implicit operator Guid(CustomerId customerId)
    {
        return customerId.Value;
    }
}
