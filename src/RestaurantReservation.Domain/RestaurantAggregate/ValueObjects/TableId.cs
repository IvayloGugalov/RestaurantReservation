namespace RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;

public sealed record TableId(Guid Value) : StronglyTypedId<Guid>(Value)
{
    public static implicit operator Guid(TableId tableId)
    {
        return tableId.Value;
    }
};
