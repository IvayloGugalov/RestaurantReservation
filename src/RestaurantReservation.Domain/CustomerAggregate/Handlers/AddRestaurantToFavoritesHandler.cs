namespace RestaurantReservation.Domain.CustomerAggregate.Handlers;

public class
    AddRestaurantToFavoritesHandler : ICommandHandler<AddRestaurantToFavoritesEvent, AddRestaurantToFavoritesResult>
{
    private readonly IRepositoryBase<Customer, CustomerId> customerRepository;

    public AddRestaurantToFavoritesHandler(IRepositoryBase<Customer, CustomerId> customerRepository)
    {
        this.customerRepository = customerRepository;
    }

    public async Task<AddRestaurantToFavoritesResult> Handle(AddRestaurantToFavoritesEvent request,
        CancellationToken cancellationToken)
    {
        request.Customer.AddRestaurantToFavorites(request.RestaurantId);
        await this.customerRepository.UpdateAsync(request.Customer, cancellationToken);

        return new AddRestaurantToFavoritesResult(request.RestaurantId);
    }
}