using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RestaurantReservation.Domain.CustomerAggregate.Models;
using RestaurantReservation.Domain.ReservationAggregate.Models;
using RestaurantReservation.Domain.RestaurantAggregate.Models;
using RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;

namespace RestaurantReservation.Infrastructure.Mongo.Data;

public class AppMongoDbContext : MongoDbContext
{
    public IMongoCollection<Customer> Customers { get; }
    public IMongoCollection<Restaurant> Restaurants { get; }
    public IMongoCollection<Reservation> Reservations { get; }
    public IMongoCollection<Table> Tables { get; }
    public IMongoCollection<Review> Reviews { get; }

    public AppMongoDbContext(IOptions<MongoOptions> options) : base(options)
    {
        this.Customers = GetCollection<Customer>(nameof(Customers));
        this.Restaurants = GetCollection<Restaurant>(nameof(Restaurants));
        this.Reservations = GetCollection<Reservation>(nameof(Reservations));
        this.Tables = GetCollection<Table>(nameof(Tables));
        this.Reviews = GetCollection<Review>(nameof(Reviews));
    }
}
