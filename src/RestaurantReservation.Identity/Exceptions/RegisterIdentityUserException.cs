namespace RestaurantReservation.Identity.Exceptions;

public class RegisterIdentityUserException : CustomException
{
    public RegisterIdentityUserException(string error) : base(error)
    {
    }
}