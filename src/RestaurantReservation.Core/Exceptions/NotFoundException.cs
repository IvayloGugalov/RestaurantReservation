namespace RestaurantReservation.Core.Exceptions;

public class NotFoundException : CustomException
{
    protected NotFoundException(string message, int? code = 404) : base(message, code: code)
    {
    }
}
