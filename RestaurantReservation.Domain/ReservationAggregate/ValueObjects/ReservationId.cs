namespace RestaurantReservation.Domain.ReservationAggregate.ValueObjects;

public record ReservationId(Guid Value)
{
    public static implicit operator Guid(ReservationId reservationId)
    {
        return reservationId.Value;
    }
};