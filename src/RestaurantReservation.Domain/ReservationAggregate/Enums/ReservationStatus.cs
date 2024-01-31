namespace RestaurantReservation.Domain.ReservationAggregate.Enums;

[Flags]
public enum ReservationStatus
{
    Created = 1,
    Confirmed = 2,
    Unpaid = 4,
    Paid = 8,
    Delayed = 16,
    Canceled = 32,
    Completed = 64
}
