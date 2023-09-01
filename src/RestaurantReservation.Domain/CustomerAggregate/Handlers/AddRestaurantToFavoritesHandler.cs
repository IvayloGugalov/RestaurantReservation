using RestaurantReservation.Domain.CustomerAggregate.Models;

namespace RestaurantReservation.Domain.CustomerAggregate.Handlers;

public class
    AddRestaurantToFavoritesHandler : ICommandHandler<AddRestaurantToFavoritesEvent, AddRestaurantToFavoritesResult>
{
    private readonly IRepositoryBase<Customer, CustomerId> customerRepository;
    private readonly IRepositoryBase<Restaurant, RestaurantId> restaurantRepository;

    public AddRestaurantToFavoritesHandler(IRepositoryBase<Customer, CustomerId> customerRepository, IRepositoryBase<Restaurant, RestaurantId> restaurantRepository)
    {
        this.customerRepository = customerRepository;
        this.restaurantRepository = restaurantRepository;
    }

    public async Task<AddRestaurantToFavoritesResult> Handle(AddRestaurantToFavoritesEvent command,
        CancellationToken cancellationToken)
    {
        command.Customer.AddRestaurantToFavorites(command.RestaurantId);
        await this.customerRepository.UpdateAsync(command.Customer, cancellationToken);

        return new AddRestaurantToFavoritesResult(command.RestaurantId);
    }
}
