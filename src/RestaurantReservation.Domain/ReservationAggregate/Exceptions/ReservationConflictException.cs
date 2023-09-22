using RestaurantReservation.Core.Exceptions;

namespace RestaurantReservation.Domain.ReservationAggregate.Exceptions;

public class ReservationConflictException : ConflictException
{
    public ReservationConflictException(int? code = default)
        : base($"Can not make a reservation due to another reservation at this time", code: code)
    {
    }
}
