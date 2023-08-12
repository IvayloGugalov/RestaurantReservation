using RestaurantReservation.Core.Model;
using RestaurantReservation.Domain.Common;
using RestaurantReservation.Domain.CustomerAggregate.ValueObjects;
using RestaurantReservation.Domain.RestaurantAggregate.Events;
using RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;

namespace RestaurantReservation.Domain.RestaurantAggregate;

public class Restaurant : AggregateRoot<RestaurantId>
{
    public string Name { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string Url { get; private set; } = null!;
    public string WebSite { get; private set; } = null!;
    public WorkTime? WorkTime { get; private set; }

    private readonly List<Review> reviews;
    public IReadOnlyCollection<Review> Reviews => this.reviews.AsReadOnly();

    private Restaurant()
    {
        this.reviews = new List<Review>();
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
        int ratingValue,
        string comment,
        CustomerId customerId,
        string customerName)
    {
        var review = Review.Create(
            id,
            ratingValue,
            comment,
            DateTime.UtcNow,
            this.Id,
            customerId,
            customerName);

        this.reviews.Add(review);

        var @event = new ReviewCreatedDomainEvent(review);
        this.AddDomainEvent(@event);

        return review;
    }
}
