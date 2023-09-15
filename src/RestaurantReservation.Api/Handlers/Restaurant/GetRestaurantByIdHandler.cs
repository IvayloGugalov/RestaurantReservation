using MongoDB.Driver;
using RestaurantReservation.Core.CQRS;
using RestaurantReservation.Domain.RestaurantAggregate.Dtos;
using RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;
using RestaurantReservation.Infrastructure.Mongo.Data;
using RestaurantReservation.Infrastructure.Mongo.Repositories;

namespace RestaurantReservation.Api.Handlers.Restaurant;

public class GetRestaurantByIdHandler : IQueryHandler<GetRestaurantById, GetRestaurantByIdResult>
{
    private readonly AppMongoDbContext dbContext;

    public GetRestaurantByIdHandler(AppMongoDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<GetRestaurantByIdResult> Handle(GetRestaurantById request, CancellationToken cancellationToken)
    {
        var restaurant =
            (await this.dbContext.Restaurants.AsQueryable().ToListAsync(cancellationToken)).SingleOrDefault(x => x.Id.Value == request.Id);


        // TODO:
        if (restaurant == null) throw new Exception("Restaurant does not exist");

        var restaurantDto = new RestaurantDto(
            Id: restaurant.Id,
            Name: restaurant.Name,
            Description: restaurant.Description,
            Phone: restaurant.Phone,
            Url: restaurant.Url,
            Website: restaurant.WebSite,
            WorkTime: new WorkTimeDto(
                restaurant.WorkTime.Monday.OpeningTime.ToString(),
                restaurant.WorkTime.Monday.ClosingTime.ToString(),
                restaurant.WorkTime.Tuesday.OpeningTime.ToString(),
                restaurant.WorkTime.Tuesday.ClosingTime.ToString(),
                restaurant.WorkTime.Wednesday.OpeningTime.ToString(),
                restaurant.WorkTime.Wednesday.ClosingTime.ToString(),
                restaurant.WorkTime.Thursday.OpeningTime.ToString(),
                restaurant.WorkTime.Thursday.ClosingTime.ToString(),
                restaurant.WorkTime.Friday.OpeningTime.ToString(),
                restaurant.WorkTime.Friday.ClosingTime.ToString(),
                restaurant.WorkTime.Saturday.OpeningTime.ToString(),
                restaurant.WorkTime.Saturday.ClosingTime.ToString(),
                restaurant.WorkTime.Sunday.OpeningTime.ToString(),
                restaurant.WorkTime.Sunday.ClosingTime.ToString()),
            Reviews: restaurant.Reviews.Select(r => new ReviewDto(r.Rating.Value, r.Comment, r.CustomerName))
        );

        return new GetRestaurantByIdResult(restaurantDto);
    }
}
