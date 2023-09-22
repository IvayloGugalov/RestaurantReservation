using RestaurantReservation.Core.Exceptions;

namespace RestaurantReservation.Domain.RestaurantAggregate.Exceptions;

public class RestaurantNotFoundException : NotFoundException
{
    public RestaurantNotFoundException(int? code = default)
        : base("Restaurant was not found", code: code)
    {
    }
}
