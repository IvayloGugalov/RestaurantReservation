using RestaurantReservation.Core.Exceptions;
using RestaurantReservation.Domain.Common;

namespace RestaurantReservation.Domain.TableAggregate.Exceptions;

public class ReservationConflictException : CustomException
{
    public ReservationConflictException(int? code = default)
        : base($"Can not make a reservation due to another reservation at this time", code: code)
    {

    }
}
