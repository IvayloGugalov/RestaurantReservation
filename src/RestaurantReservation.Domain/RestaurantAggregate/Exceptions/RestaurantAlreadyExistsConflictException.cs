using RestaurantReservation.Core.Exceptions;

namespace RestaurantReservation.Domain.RestaurantAggregate.Exceptions;

public class RestaurantAlreadyExistsConflictException : ConflictException
{
    public RestaurantAlreadyExistsConflictException(int? code = default)
        : base("Restaurant already exists", code: code)
    {
    }
}
