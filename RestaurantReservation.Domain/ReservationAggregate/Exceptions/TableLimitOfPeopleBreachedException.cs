using RestaurantReservation.Core.Exceptions;

namespace RestaurantReservation.Domain.ReservationAggregate.Exceptions;

public class TableLimitOfPeopleBreachedException : CustomException
{
    public TableLimitOfPeopleBreachedException(ushort tableCapacity, ushort reservedFor, int? code = default)
        : base($"The limit of people is {tableCapacity}, but the reservation is made of {reservedFor} people",
            code: code)
    {
    }
}