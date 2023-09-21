namespace RestaurantReservation.Domain.ReservationAggregate.Handlers;

public class CreateReviewHandler : ICommandHandler<CreateReviewEvent, CreateReviewResult>
{
    private readonly IMongoRepository<Reservation, ReservationId> reservationRepository;

    public CreateReviewHandler(IMongoRepository<Reservation, ReservationId> reservationRepository)
    {
        this.reservationRepository = reservationRepository;
    }

    public async Task<CreateReviewResult> Handle(CreateReviewEvent command, CancellationToken ct)
    {
        // var reservation = await this.reservationRepository.FirstOrDefaultAsync(x => x.Id == command.ReservationId, cancellationToken);
        // if (reservation == null) throw new ReservationNotFoundException();
        //
        // var reviewEntity = reservation.AddReview(
        //     new ReviewId(command.Id),
        //     command.Rating,
        //     command.Comment,
        //     command.CustomerName);
        //
        // await this.reservationRepository.UpdateAsync(reservation, cancellationToken);
        // var newReview = await this.reviewRepository.AddAsync(reviewEntity, cancellationToken);
        //
        // return new CreateReviewResult(newReview.Id);
        throw new NotImplementedException();
    }
}
