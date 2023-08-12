using RestaurantReservation.Core.Exceptions;
using RestaurantReservation.Domain.Common;

namespace RestaurantReservation.Domain.RestaurantAggregate.Exceptions;

public class RestaurantAlreadyExistsException : CustomException
{
    public RestaurantAlreadyExistsException(int? code = default)
        : base("Customer already exists", code: code)
    {
        
    }
}