namespace RestaurantReservation.Domain.CustomerAggregate.ValueObjects;

public record CustomerId(Guid Value)
{
    public static implicit operator Guid(CustomerId customerId)
    {
        return customerId.Value;
    }
}