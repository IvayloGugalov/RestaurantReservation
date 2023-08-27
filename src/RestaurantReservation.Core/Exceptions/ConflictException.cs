namespace RestaurantReservation.Core.Exceptions;

public class ConflictException : CustomException
{
    public ConflictException(string message, int? code = null) : base(message, code: code)
    {
    }
}