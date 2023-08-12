using RestaurantReservation.Core.Model;
using RestaurantReservation.Domain.Common.ValueObjects;
using RestaurantReservation.Domain.CustomerAggregate.Events;
using RestaurantReservation.Domain.CustomerAggregate.ValueObjects;
using RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;

namespace RestaurantReservation.Domain.CustomerAggregate;

public class Customer : AggregateRoot<CustomerId>
{
    public CustomerName CustomerName { get; private set;}
    public Email Email { get; private set;}

    private readonly List<RestaurantId> favoriteRestaurants;
    public IReadOnlyCollection<RestaurantId> FavoriteRestaurants => this.favoriteRestaurants.AsReadOnly();

    private Customer()
    {
        this.favoriteRestaurants = new List<RestaurantId>();
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
            CustomerName = new CustomerName(FirstName: firstName, LastName: lastName),
            Email = new Email(emailValue)
        };

        var @event = new CustomerCreatedDomainEvent(customer.Id, firstName, lastName, emailValue);
        customer.AddDomainEvent(@event);

        return customer;
    }

    public void AddRestaurantToFavorites(RestaurantId restaurantId)
    {
        if (this.favoriteRestaurants.Contains(restaurantId)) return;

        this.favoriteRestaurants.Add(restaurantId);

        var @event = new RestaurantAddedToFavoritesDomainEvent(restaurantId);
        this.AddDomainEvent(@event);
    }
}
