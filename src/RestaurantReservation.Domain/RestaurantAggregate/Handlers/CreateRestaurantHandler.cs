namespace RestaurantReservation.Domain.RestaurantAggregate.Handlers;

public class CreateRestaurantHandler : ICommandHandler<CreateRestaurantEvent, CreateRestaurantResult>
{
    private readonly IRepositoryBase<Restaurant, RestaurantId> restaurantRepository;

    public CreateRestaurantHandler(IRepositoryBase<Restaurant, RestaurantId> restaurantRepository)
    {
        this.restaurantRepository = restaurantRepository;
    }

    public async Task<CreateRestaurantResult> Handle(CreateRestaurantEvent request, CancellationToken cancellationToken)
    {
        var restaurant =
            await this.restaurantRepository.SingleOrDefaultAsync(x => x.Name == request.Name, cancellationToken);
        if (restaurant != null) throw new RestaurantAlreadyExistsException();

        var restaurantEntity = Restaurant.Create(
            name: request.Name,
            phone: request.Phone,
            description: request.Description,
            url: request.Url,
            webSite: request.WebSite,
            workTime: request.WorkTime);

        var newRestaurant = await this.restaurantRepository.AddAsync(restaurantEntity, cancellationToken);
        return new CreateRestaurantResult(newRestaurant.Id);
    }
}