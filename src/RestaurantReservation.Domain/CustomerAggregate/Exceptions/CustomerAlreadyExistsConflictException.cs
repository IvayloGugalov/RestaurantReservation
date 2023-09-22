using RestaurantReservation.Core.Exceptions;

namespace RestaurantReservation.Domain.CustomerAggregate.Exceptions;

public class CustomerAlreadyExistsConflictException : ConflictException
{
    public CustomerAlreadyExistsConflictException(int? code = default)
        : base("Customer already exists", code: code)
    {
    }
}
