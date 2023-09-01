namespace RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;

public sealed record ReviewId(Guid Value) : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; } = Value;

    public static implicit operator Guid(ReviewId reviewId)
    {
        return reviewId.Value;
    }
};
