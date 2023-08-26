using RestaurantReservation.Core.Exceptions;

namespace RestaurantReservation.Domain.ReservationAggregate.Exceptions;

public class ReservationNotFoundException : CustomException
{
    public ReservationNotFoundException(int? code = default)
        : base($"Reservation was not found", code: code)
    {
    }
}