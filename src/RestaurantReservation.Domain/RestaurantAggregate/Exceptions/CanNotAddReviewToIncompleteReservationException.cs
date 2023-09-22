using RestaurantReservation.Core.Exceptions;

namespace RestaurantReservation.Domain.RestaurantAggregate.Exceptions;

public class CanNotAddReviewToIncompleteReservationException : CustomException
{
    public CanNotAddReviewToIncompleteReservationException(int? code = 401)
        : base("The reservation must be completed before adding a review!", code: code)
    {
    }
}
