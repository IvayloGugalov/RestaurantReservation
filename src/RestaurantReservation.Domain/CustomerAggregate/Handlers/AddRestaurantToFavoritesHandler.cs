using RestaurantReservation.Domain.CustomerAggregate.Models;

namespace RestaurantReservation.Domain.CustomerAggregate.Handlers;

public class
    AddRestaurantToFavoritesHandler : ICommandHandler<AddRestaurantToFavoritesEvent, AddRestaurantToFavoritesResult>
{
    private readonly IRepositoryBase<Customer, CustomerId> customerRepository;

    public AddRestaurantToFavoritesHandler(IRepositoryBase<Customer, CustomerId> customerRepository)
    {
        this.customerRepository = customerRepository;
    }

    public async Task<AddRestaurantToFavoritesResult> Handle(AddRestaurantToFavoritesEvent command,
        CancellationToken cancellationToken)
    {
        command.Customer.AddRestaurantToFavorites(command.RestaurantId);
        await this.customerRepository.UpdateAsync(command.Customer, cancellationToken);

        return new AddRestaurantToFavoritesResult(command.RestaurantId);
    }
}
