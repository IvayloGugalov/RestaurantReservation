namespace RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;

public sealed record RestaurantId(Guid Value)
{
    public static implicit operator Guid(RestaurantId restaurantId)
    {
        return restaurantId.Value;
    }

}
