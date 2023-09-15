using RestaurantReservation.Core.CQRS;
using RestaurantReservation.Infrastructure.Mongo.Data;

namespace RestaurantReservation.Api.Handlers.Reservations;

public class CreateReviewHandler : ICommandHandler<CreateReviewEvent, CreateReviewResult>
{
    private readonly AppMongoDbContext dbContext;

    public CreateReviewHandler(AppMongoDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<CreateReviewResult> Handle(CreateReviewEvent command, CancellationToken cancellationToken)
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
