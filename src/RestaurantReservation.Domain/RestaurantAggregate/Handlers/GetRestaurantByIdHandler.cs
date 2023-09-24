using RestaurantReservation.Core.Mongo;
using RestaurantReservation.Domain.RestaurantAggregate.Dtos;

namespace RestaurantReservation.Domain.RestaurantAggregate.Handlers;

using Restaurant = Restaurant;

public class GetRestaurantByIdHandler : IQueryHandler<GetRestaurantById, GetRestaurantByIdResult>
{
    private readonly IMongoRepository<Restaurant, RestaurantId> restaurantRepository;

    public GetRestaurantByIdHandler(IMongoRepository<Restaurant, RestaurantId> restaurantRepository)
    {
        this.restaurantRepository = restaurantRepository;
    }

    public async Task<GetRestaurantByIdResult> Handle(GetRestaurantById request, CancellationToken ct)
    {
        var restaurant = await this.restaurantRepository.GetByIdAsync(new RestaurantId(request.Id), ct);

        // TODO:
        if (restaurant == null) throw new RestaurantNotFoundException();

        var restaurantDto = new RestaurantDto(
            Id: restaurant.Id,
            Name: restaurant.Name,
            Description: restaurant.Description,
            Phone: restaurant.Phone,
            Url: restaurant.Url,
            Website: restaurant.WebSite,
            WorkTime: new WorkTimeDto(
                restaurant.WorkTime?.Monday?.OpeningTime.ToString(),
                restaurant.WorkTime?.Monday?.ClosingTime.ToString(),
                restaurant.WorkTime?.Tuesday?.OpeningTime.ToString(),
                restaurant.WorkTime?.Tuesday?.ClosingTime.ToString(),
                restaurant.WorkTime?.Wednesday?.OpeningTime.ToString(),
                restaurant.WorkTime?.Wednesday?.ClosingTime.ToString(),
                restaurant.WorkTime?.Thursday?.OpeningTime.ToString(),
                restaurant.WorkTime?.Thursday?.ClosingTime.ToString(),
                restaurant.WorkTime?.Friday?.OpeningTime.ToString(),
                restaurant.WorkTime?.Friday?.ClosingTime.ToString(),
                restaurant.WorkTime?.Saturday?.OpeningTime.ToString(),
                restaurant.WorkTime?.Saturday?.ClosingTime.ToString(),
                restaurant.WorkTime?.Sunday?.OpeningTime.ToString(),
                restaurant.WorkTime?.Sunday?.ClosingTime.ToString()),
            //TODO:
            Reviews: null);

        return new GetRestaurantByIdResult(restaurantDto);
    }
}
