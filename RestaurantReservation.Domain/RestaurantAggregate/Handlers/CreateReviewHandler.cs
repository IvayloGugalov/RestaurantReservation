using MediatR;
using RestaurantReservation.Core.CQRS;
using RestaurantReservation.Core.Repository;
using RestaurantReservation.Domain.Common;
using RestaurantReservation.Domain.CustomerAggregate.Exceptions;
using RestaurantReservation.Domain.RestaurantAggregate.Events;
using RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;

namespace RestaurantReservation.Domain.RestaurantAggregate.Handlers;

public class CreateReviewHandler : ICommandHandler<CreateReviewEvent, CreateReviewResult>
{
    private readonly IRepositoryBase<Restaurant, RestaurantId> restaurantRepository;
    private readonly IRepositoryBase<Review, ReviewId> reviewRepository;

    public CreateReviewHandler(
        IRepositoryBase<Restaurant, RestaurantId> restaurantRepository,
        IRepositoryBase<Review, ReviewId> reviewRepository)
    {
        this.restaurantRepository = restaurantRepository;
        this.reviewRepository = reviewRepository;
    }

    public async Task<CreateReviewResult> Handle(CreateReviewEvent request, CancellationToken cancellationToken)
    {
        var restaurant = await this.restaurantRepository.FirstOrDefaultAsync(x => x.Id == request.RestaurantId, cancellationToken);
        if (restaurant == null) throw new CustomerNotFoundException();

        var reviewEntity = restaurant.AddReview(
            new ReviewId(request.Id),
            request.Rating,
            request.Comment,
            request.CustomerId,
            request.CustomerName);

        await this.restaurantRepository.UpdateAsync(restaurant, cancellationToken);
        var newReview = await this.reviewRepository.AddAsync(reviewEntity, cancellationToken);

        return new CreateReviewResult(newReview.Id);
    }
}
