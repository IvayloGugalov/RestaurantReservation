namespace RestaurantReservation.Domain.ReservationAggregate.Mappings;

public static class ReservationMappings
{
    public static CreateReservation Map(RequestCreateReservationDto model) =>
        new(model.TableId, model.CustomerId, model.ReservationDate, model.Occupants);

    public static ResponseCreateReservationDto Map(CreateReservationResult model) => new(model.Id);

    public static AddReview Map(this RequestAddReviewDto model) =>
        new(model.ReservationId, model.Rating, model.Comment, model.CustomerName);
}
