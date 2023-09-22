using RestaurantReservation.Core.Exceptions;

namespace RestaurantReservation.Domain.CustomerAggregate.Exceptions;

public class CustomerNotFoundException : NotFoundException
{
    public CustomerNotFoundException(int? code = default)
        : base("Customer not found", code: code)
    {
    }
}
