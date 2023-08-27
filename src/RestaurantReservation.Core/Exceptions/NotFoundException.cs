namespace RestaurantReservation.Core.Exceptions;

public class NotFoundException : CustomException
{
    public NotFoundException(string message, int? code = null) : base(message, code: code)
    {
    }
}