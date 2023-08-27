namespace RestaurantReservation.Domain.ReservationAggregate.Handlers;

public class CreateReviewHandler : ICommandHandler<CreateReviewEvent, CreateReviewResult>
{
    private readonly IRepositoryBase<Reservation, ReservationId> reservationRepository;
    private readonly IRepositoryBase<Review, ReviewId> reviewRepository;

    public CreateReviewHandler(
        IRepositoryBase<Reservation, ReservationId> reservationRepository,
        IRepositoryBase<Review, ReviewId> reviewRepository)
    {
        this.reservationRepository = reservationRepository;
        this.reviewRepository = reviewRepository;
    }

    public async Task<CreateReviewResult> Handle(CreateReviewEvent request, CancellationToken cancellationToken)
    {
        // var reservation = await this.reservationRepository.FirstOrDefaultAsync(x => x.Id == request.ReservationId, cancellationToken);
        // if (reservation == null) throw new ReservationNotFoundException();
        //
        // var reviewEntity = reservation.AddReview(
        //     new ReviewId(request.Id),
        //     request.Rating,
        //     request.Comment,
        //     request.CustomerName);
        //
        // await this.reservationRepository.UpdateAsync(reservation, cancellationToken);
        // var newReview = await this.reviewRepository.AddAsync(reviewEntity, cancellationToken);
        //
        // return new CreateReviewResult(newReview.Id);
        throw new NotImplementedException();
    }
}