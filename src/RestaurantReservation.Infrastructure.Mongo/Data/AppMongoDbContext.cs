﻿using Microsoft.Extensions.Options;
using RestaurantReservation.Core.Mongo;
using RestaurantReservation.Core.Mongo.Data;

namespace RestaurantReservation.Infrastructure.Mongo.Data;

public sealed class AppMongoDbContext : MongoDbContext
{
    public IMongoCollection<Customer> Customers { get; }
    public IMongoCollection<Restaurant> Restaurants { get; }
    public IMongoCollection<Reservation> Reservations { get; }
    public IMongoCollection<Table> Tables { get; }
    public IMongoCollection<Review> Reviews { get; }

    public AppMongoDbContext(IOptions<MongoOptions> options) : base(options)
    {
        this.Customers = this.GetCollection<Customer>();
        this.Restaurants = this.GetCollection<Restaurant>();
        this.Reservations = this.GetCollection<Reservation>();
        this.Tables = this.GetCollection<Table>();
        this.Reviews = this.GetCollection<Review>();
    }
}
