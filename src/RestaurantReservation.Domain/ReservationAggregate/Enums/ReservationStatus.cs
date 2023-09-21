namespace RestaurantReservation.Domain.ReservationAggregate.Enums;

public enum ReservationStatus
{
    Created,
    Confirmed,
    Unpaid,
    Paid,
    Delayed,
    Canceled,
    Completed
}