using MongoDB.Driver;
using RestaurantReservation.Core.CQRS;
using RestaurantReservation.Domain.RestaurantAggregate.Dtos;
using RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;
using RestaurantReservation.Infrastructure.Mongo.Data;
using RestaurantReservation.Infrastructure.Mongo.Repositories;

namespace RestaurantReservation.Api.Handlers.Restaurant;

public class GetRestaurantByIdHandler : IQueryHandler<GetRestaurantById, GetRestaurantByIdResult>
{
    private readonly IMongoRepository<Domain.RestaurantAggregate.Models.Restaurant, RestaurantId, Guid> dbContext;
    private readonly AppMongoDbContext db;

    public GetRestaurantByIdHandler(IMongoRepository<Domain.RestaurantAggregate.Models.Restaurant, RestaurantId, Guid> dbContext, AppMongoDbContext db)
    {
        this.dbContext = dbContext;
        this.db = db;
    }

    public async Task<GetRestaurantByIdResult> Handle(GetRestaurantById request, CancellationToken ct)
    {
        var restaurant = await this.dbContext.GetByIdAsync(new RestaurantId(request.Id), ct);
        // var restaurant1 = await this.db.Restaurants
        //     .FindAsync(Builders<Domain.RestaurantAggregate.Models.Restaurant>.Filter.Eq("_id", request.Id), cancellationToken: ct);
        // var restaurant = restaurant1.SingleOrDefault();

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
            //TODO:
            Reviews: null);

        return new GetRestaurantByIdResult(restaurantDto);
    }
}
