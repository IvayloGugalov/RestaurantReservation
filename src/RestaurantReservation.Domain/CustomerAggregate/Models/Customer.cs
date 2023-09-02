namespace RestaurantReservation.Domain.CustomerAggregate.Models;

public class Customer : AggregateRoot<CustomerId>
{
    public CustomerName FullName { get; private init; } = null!;
    public Email Email { get; private init; } = null!;

    private readonly List<RestaurantId> favouriteRestaurants;
    public IReadOnlyCollection<RestaurantId> FavouriteRestaurants => this.favouriteRestaurants.AsReadOnly();

    private readonly List<ReservationId> reservations;
    public IReadOnlyCollection<ReservationId> Reservations => this.reservations.AsReadOnly();

    private Customer()
    {
        this.favouriteRestaurants = new List<RestaurantId>();
        this.reservations = new List<ReservationId>();
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

        var @event = new CustomerCreatedDomainEvent(customer.Id.Value, firstName, lastName, emailValue);
        customer.AddDomainEvent(@event);

        return customer;
    }

    public bool AddRestaurantToFavorites(RestaurantId restaurantId)
    {
        if (this.favouriteRestaurants.Contains(restaurantId)) return false;

        this.favouriteRestaurants.Add(restaurantId);

        var @event = new RestaurantAddedToFavoritesDomainEvent(restaurantId);
        this.AddDomainEvent(@event);

        return true;
    }

    public bool AddReservation(ReservationId reservationId)
    {
        if (this.reservations.Contains(reservationId)) return false;

        this.reservations.Add(reservationId);

        // var @event = new RestaurantAddedToFavoritesDomainEvent(reservationId);
        // this.AddDomainEvent(@event);

        return true;
    }
}
