namespace RestaurantReservation.Domain.ReservationAggregate.ValueObjects;

public sealed record ReservationId(Guid Value)
{
    public static implicit operator Guid(ReservationId reservationId)
    {
        return reservationId.Value;
    }
};
