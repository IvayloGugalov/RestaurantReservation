using RestaurantReservation.Core.Exceptions;
using RestaurantReservation.Domain.Common;

namespace RestaurantReservation.Domain.TableAggregate.Exceptions;

public class TableLimitOfPeopleBreachedException : CustomException
{
    public TableLimitOfPeopleBreachedException(ushort tableCapacity, ushort reservedFor, int? code = default)
        : base($"The limit of people is {tableCapacity}, but the reservation is made of {reservedFor} people", code: code)
    {
    }
}
