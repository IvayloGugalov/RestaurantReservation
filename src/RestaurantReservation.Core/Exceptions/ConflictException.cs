namespace RestaurantReservation.Core.Exceptions;

public class ConflictException : CustomException
{
    protected ConflictException(string message, int? code = 409) : base(message, code: code)
    {
    }
}
