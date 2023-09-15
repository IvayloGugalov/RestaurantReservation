using RestaurantReservation.Core.CQRS;
using RestaurantReservation.Domain.RestaurantAggregate.Exceptions;
using RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;
using RestaurantReservation.Infrastructure.Mongo.Repositories;

namespace RestaurantReservation.Api.Handlers.Restaurant;

public class CreateRestaurantHandler : ICommandHandler<CreateRestaurant, CreateRestaurantResult>
{
    // private readonly IRepositoryBase<Restaurant, RestaurantId> restaurantRepository;
    //
    // public CreateRestaurantHandler(IRepositoryBase<Restaurant, RestaurantId> restaurantRepository)
    // {
    //     this.restaurantRepository = restaurantRepository;
    // }

    private readonly IMongoRepository<Domain.RestaurantAggregate.Models.Restaurant, RestaurantId> restaurantRepository;

    public CreateRestaurantHandler(IMongoRepository<Domain.RestaurantAggregate.Models.Restaurant, RestaurantId> restaurantRepository)
    {
        this.restaurantRepository = restaurantRepository;
    }


    public async Task<CreateRestaurantResult> Handle(CreateRestaurant command, CancellationToken cancellationToken)
    {
        var restaurant =
            await this.restaurantRepository.SingleOrDefaultAsync(x => x.Name == command.Name, cancellationToken);
        if (restaurant != null) throw new RestaurantAlreadyExistsException();

        var restaurantEntity = Domain.RestaurantAggregate.Models.Restaurant.Create(
            new RestaurantId(command.Id),
            name: command.Name,
            phone: command.Phone,
            description: command.Description,
            url: command.Url,
            webSite: command.WebSite);

        restaurantEntity.SetWorkTime(command.WorkTime!);
        var newRestaurant = await this.restaurantRepository.AddAsync(restaurantEntity, cancellationToken);

        return new CreateRestaurantResult(newRestaurant.Id.Value);
    }
}
