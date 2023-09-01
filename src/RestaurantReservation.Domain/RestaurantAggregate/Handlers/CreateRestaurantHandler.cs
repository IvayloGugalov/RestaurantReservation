namespace RestaurantReservation.Domain.RestaurantAggregate.Handlers;

public class CreateRestaurantHandler : ICommandHandler<CreateRestaurant, CreateRestaurantResult>
{
    private readonly IRepositoryBase<Restaurant, RestaurantId> restaurantRepository;

    public CreateRestaurantHandler(IRepositoryBase<Restaurant, RestaurantId> restaurantRepository)
    {
        this.restaurantRepository = restaurantRepository;
    }

    public async Task<CreateRestaurantResult> Handle(CreateRestaurant command, CancellationToken cancellationToken)
    {
        var restaurant =
            await this.restaurantRepository.SingleOrDefaultAsync(x => x.Name == command.Name, cancellationToken);
        if (restaurant != null) throw new RestaurantAlreadyExistsException();

        var restaurantEntity = Restaurant.Create(
            new RestaurantId(command.Id),
            name: command.Name,
            phone: command.Phone,
            description: command.Description,
            url: command.Url,
            webSite: command.WebSite,
            workTime: command.WorkTime);

        var newRestaurant = await this.restaurantRepository.AddAsync(restaurantEntity, cancellationToken);
        return new CreateRestaurantResult(newRestaurant.Id.Value);
    }
}
