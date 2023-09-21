
namespace RestaurantReservation.Domain.RestaurantAggregate.Handlers;

public class CreateRestaurantHandler : ICommandHandler<CreateRestaurant, CreateRestaurantResult>
{
    private readonly IMongoRepository<Restaurant, RestaurantId> restaurantRepository;

    public CreateRestaurantHandler(IMongoRepository<Restaurant, RestaurantId> restaurantRepository)
    {
        this.restaurantRepository = restaurantRepository;
    }

    public async Task<CreateRestaurantResult> Handle(CreateRestaurant command, CancellationToken ct)
    {
        var restaurant = await this.restaurantRepository.FindOneAsync(x => x.Name == command.Name, ct);
        if (restaurant != null) throw new RestaurantAlreadyExistsException();

        var restaurantEntity = Restaurant.Create(
            new RestaurantId(command.Id),
            name: command.Name,
            phone: command.Phone,
            description: command.Description,
            url: command.Url,
            webSite: command.WebSite);

        restaurantEntity.SetWorkTime(command.WorkTime!);
        this.restaurantRepository.AddAsync(restaurantEntity, ct);

        return new CreateRestaurantResult(restaurantEntity.Id.Value);
    }
}
