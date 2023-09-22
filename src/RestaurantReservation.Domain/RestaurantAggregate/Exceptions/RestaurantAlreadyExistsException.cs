using RestaurantReservation.Core.Exceptions;

namespace RestaurantReservation.Domain.RestaurantAggregate.Exceptions;

public class RestaurantAlreadyExistsException : ConflictException
{
    public RestaurantAlreadyExistsException(int? code = default)
        : base("Restaurant already exists", code: code)
    {
    }
}
