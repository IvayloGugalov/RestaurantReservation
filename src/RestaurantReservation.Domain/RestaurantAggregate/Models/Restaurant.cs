﻿namespace RestaurantReservation.Domain.RestaurantAggregate.Models;

public class Restaurant : AggregateRoot<RestaurantId>
{
    public string Name { get; private init; } = null!;
    public string Phone { get; private init; } = null!;
    public string Description { get; private init; } = null!;
    public string Url { get; private init; } = null!;
    public string WebSite { get; private init; } = null!;
    public WorkTime WorkTime { get; private init; } = null!;

    private readonly List<Review> reviews;
    public IReadOnlyCollection<Review> Reviews => this.reviews.AsReadOnly();

    private readonly List<Reservation> reservations;
    public IReadOnlyCollection<Reservation> Reservations => this.reservations.AsReadOnly();

    private Restaurant()
    {
        this.reviews = new List<Review>();
        this.reservations = new List<Reservation>();
    }

    public static Restaurant Create(
        string name,
        string phone,
        string description,
        string url,
        string webSite,
        WorkTime workTime)
    {
        var restaurant = new Restaurant
        {
            Name = name,
            Phone = phone,
            Description = description,
            Url = url,
            WebSite = webSite,
            WorkTime = workTime
        };

        var @event = new RestaurantCreatedDomainEvent(
            restaurant.Name,
            restaurant.Phone,
            restaurant.Description,
            restaurant.Url,
            restaurant.WebSite,
            restaurant.WorkTime);
        restaurant.AddDomainEvent(@event);

        return restaurant;
    }

    public Review AddReview(
        ReviewId id,
        Guid customerId,
        int ratingValue,
        string comment,
        string customerName)
    {
        // if (this.Status != ReservationStatus.Completed || this.Status != ReservationStatus.Canceled) return null;

        // Create a review associated with this reservation
        var review = Review.Create(
            id,
            ratingValue,
            comment,
            this.Id,
            customerId,
            customerName,
            this.Id);

        // Raise domain events or perform other actions related to review creation
        var @event = new ReviewCreatedDomainEvent(review);
        this.AddDomainEvent(@event);

        return review;
    }
}