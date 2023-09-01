namespace RestaurantReservation.Domain.RestaurantAggregate.Models;

public class Restaurant : AggregateRoot<RestaurantId, Guid>
{
    public string Name { get; private init; } = null!;
    public string Phone { get; private init; } = null!;
    public string Description { get; private init; } = null!;
    public string Url { get; private init; } = null!;
    public string WebSite { get; private init; } = null!;
    public WorkTime WorkTime { get; private init; } = null!;

    private readonly List<Review> reviews;
    public IReadOnlyCollection<Review> Reviews => this.reviews.AsReadOnly();

    private readonly List<Table> tables;
    public IReadOnlyCollection<Table> Tables => this.tables.AsReadOnly();

    private Restaurant()
    {
        this.reviews = new List<Review>();
        this.tables = new List<Table>();
    }

    public static Restaurant Create(
        RestaurantId restaurantId,
        string name,
        string phone,
        string description,
        string url,
        string webSite,
        WorkTime? workTime)
    {
        var restaurant = new Restaurant
        {
            Id = restaurantId,
            Name = name,
            Phone = phone,
            Description = description,
            Url = url,
            WebSite = webSite,
            WorkTime = (workTime ?? WorkTime.CreateEmpty())!
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

    public Table AddTable(
        TableId tableId,
        string number,
        ushort capacity)
    {
        var table = Table.Create(
            tableId,
            number,
            capacity,
            this);

        this.tables.Add(table);

        return table;
    }

    public Review AddReview(
        ReviewId id,
        CustomerId customerId,
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
            this,
            customerId,
            customerName,
            (ReservationId?)this.Id);

        // Raise domain events or perform other actions related to review creation
        var @event = new ReviewCreatedDomainEvent(review);
        this.AddDomainEvent(@event);
        this.reviews.Add(review);

        return review;
    }
}
