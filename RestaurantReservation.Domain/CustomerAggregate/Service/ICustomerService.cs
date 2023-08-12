using RestaurantReservation.Domain.CustomerAggregate.ValueObjects;
using RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;

namespace RestaurantReservation.Domain.CustomerAggregate.Service;

public interface ICustomerService
{
    Task<Customer> CreateAsync(string firstName, string lastName, string email);

    Task AddRestaurantToFavorites(CustomerId customerId, RestaurantId restaurantId);
}
