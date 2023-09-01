namespace RestaurantReservation.Domain.ReservationAggregate.ValueObjects;

public sealed record ReservationId(Guid Value) : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; } = Value;

    public static implicit operator Guid(ReservationId reservationId)
    {
        return reservationId.Value;
    }
};
