using RestaurantReservation.Core.Exceptions;

namespace RestaurantReservation.Domain.ReservationAggregate.Exceptions;

public class TableNotFoundException : CustomException
{
    public TableNotFoundException(int? code = default)
        : base("Table not found", code: code)
    {
    }
}