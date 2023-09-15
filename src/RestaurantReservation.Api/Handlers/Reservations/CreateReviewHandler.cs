using RestaurantReservation.Core.CQRS;
using RestaurantReservation.Domain.ReservationAggregate.Models;
using RestaurantReservation.Domain.ReservationAggregate.ValueObjects;
using RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;
using RestaurantReservation.Infrastructure.Mongo.Repositories;

namespace RestaurantReservation.Api.Handlers.Reservations;

public class CreateReviewHandler : ICommandHandler<CreateReviewEvent, CreateReviewResult>
{
    private readonly IMongoRepository<Reservation, ReservationId> reservationRepository;
    private readonly IMongoRepository<Review, ReviewId> reviewRepository;

    public CreateReviewHandler(
        IMongoRepository<Reservation, ReservationId> reservationRepository,
        IMongoRepository<Review, ReviewId> reviewRepository)
    {
        this.reservationRepository = reservationRepository;
        this.reviewRepository = reviewRepository;
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
