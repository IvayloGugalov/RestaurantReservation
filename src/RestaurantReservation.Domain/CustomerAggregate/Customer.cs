namespace RestaurantReservation.Domain.CustomerAggregate;

public class Customer : AggregateRoot<CustomerId>
{
    public CustomerName FullName { get; private init; } = null!;
    public Email Email { get; private init; } = null!;

    private readonly List<Guid> favouriteRestaurants;
    public IReadOnlyCollection<Guid> FavouriteRestaurants => this.favouriteRestaurants.AsReadOnly();

    private readonly List<Reservation> reservations;
    public IReadOnlyCollection<Reservation> Reservations => this.reservations.AsReadOnly();

    private Customer()
    {
        this.favouriteRestaurants = new List<Guid>();
        this.reservations = new List<Reservation>();
    }

    public static Customer Create(
        CustomerId id,
        string firstName,
        string lastName,
        string emailValue)
    {
        var customer = new Customer
        {
            Id = id,
            FullName = new CustomerName(FirstName: firstName, LastName: lastName),
            Email = new Email(emailValue)
        };

        var @event = new CustomerCreatedDomainEvent(customer.Id, firstName, lastName, emailValue);
        customer.AddDomainEvent(@event);

        return customer;
    }

    public void AddRestaurantToFavorites(RestaurantId restaurantId)
    {
        if (this.favouriteRestaurants.Contains(restaurantId)) return;

        this.favouriteRestaurants.Add(restaurantId);

        var @event = new RestaurantAddedToFavoritesDomainEvent(restaurantId);
        this.AddDomainEvent(@event);
    }
}