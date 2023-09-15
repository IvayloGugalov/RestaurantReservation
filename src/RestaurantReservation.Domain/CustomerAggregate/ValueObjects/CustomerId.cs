namespace RestaurantReservation.Domain.CustomerAggregate.ValueObjects;

public sealed record CustomerId(Guid Value) : StronglyTypedId<Guid>(Value)
{
    public static implicit operator Guid(CustomerId customerId)
    {
        return customerId.Value;
    }
}
