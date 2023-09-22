using RestaurantReservation.Core.Exceptions;

namespace RestaurantReservation.Domain.RestaurantAggregate.Exceptions;

public class RestaurantDoesNotExistException : NotFoundException
{
    public RestaurantDoesNotExistException(int? code = default)
        : base("Restaurant was not found", code: code)
    {
    }
}
