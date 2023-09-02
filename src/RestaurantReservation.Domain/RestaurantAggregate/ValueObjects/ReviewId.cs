namespace RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;

public sealed record ReviewId(Guid Value)
{
    public static implicit operator Guid(ReviewId reviewId)
    {
        return reviewId.Value;
    }
};
