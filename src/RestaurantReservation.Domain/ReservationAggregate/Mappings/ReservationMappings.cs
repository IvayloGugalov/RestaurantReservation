namespace RestaurantReservation.Domain.ReservationAggregate.Mappings;

public static class ReservationMappings
{
    public static CreateReservation Map(RequestCreateReservationDto model) =>
        new(model.TableId, model.CustomerId, model.ReservationDate, model.Occupants);

    public static ResponseCreateReservationDto Map(CreateReservationResult model) => new(model.Id);

    public static AddReview Map(this RequestAddReviewDto model) =>
        new(model.ReservationId, model.Rating, model.Comment, model.CustomerName);

    public static UpdateReservation Map(this RequestUpdateReservation model) => new(
        ReservationId: new ReservationId(model.ReservationId),
        RestaurantId: new RestaurantId(model.RestaurantId),
        CustomerId: new CustomerId(model.CustomerId),
        TableId: new TableId(model.TableId),
        ReservationDate: model.ReservationDate,
        ReservationStatus: model.ReservationStatus,
        Occupants: model.Occupants,
        ReviewId: model.ReviewId == null ? null : new ReviewId(model.ReviewId.Value));

    public static ResponseUpdateReservation Map(this UpdateReservationResult model) => new(
        ReservationId: model.ReservationId,
        RestaurantId: model.RestaurantId,
        TableId: model.TableId,
        CustomerId: model.CustomerId,
        ReservationStatus: model.ReservationStatus,
        ReservationDate: model.ReservationDate,
        ReviewId: model.ReviewId!,
        Occupants: model.Occupants);
}
