namespace RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;

public sealed record TableId(Guid Value) : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; } = Value;

    public static implicit operator Guid(TableId tableId)
    {
        return tableId.Value;
    }
};
