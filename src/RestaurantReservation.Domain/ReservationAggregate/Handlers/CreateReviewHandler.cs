using RestaurantReservation.Core.Mongo;

namespace RestaurantReservation.Domain.ReservationAggregate.Handlers;

public class CreateReviewHandler : ICommandHandler<AddReview, CreateReviewResult>
{
    private readonly IMongoRepository<Reservation, ReservationId> reservationRepository;
    private readonly IMongoRepository<Restaurant, RestaurantId> restaurantRepository;
    private readonly IMongoRepository<Review, ReviewId> reviewRepository;

    public CreateReviewHandler(
        IMongoRepository<Reservation, ReservationId> reservationRepository,
        IMongoRepository<Restaurant, RestaurantId> restaurantRepository,
        IMongoRepository<Review, ReviewId> reviewRepository)
    {
        this.reservationRepository = reservationRepository;
        this.restaurantRepository = restaurantRepository;
        this.reviewRepository = reviewRepository;
    }

    public async Task<CreateReviewResult> Handle(AddReview command, CancellationToken ct)
    {
        var reservation = await this.reservationRepository.GetByIdAsync(new ReservationId(command.ReservationId), ct);
        if (reservation == null) throw new ReservationNotFoundException();

        var restaurant = await this.restaurantRepository.GetByIdAsync(reservation.RestaurantId, ct);
        if (restaurant == null) throw new RestaurantNotFoundException();
        var reviewEntity = restaurant.AddReview(
            new ReviewId(command.Id),
            reservation.CustomerId,
            command.Rating,
            command.Comment,
            command.CustomerName,
            reservation);

        await this.restaurantRepository.UpdateAsync(restaurant, ct);
        var newReview = await this.reviewRepository.AddAsync(reviewEntity, ct);

        return new CreateReviewResult(newReview.Id);
    }
}
