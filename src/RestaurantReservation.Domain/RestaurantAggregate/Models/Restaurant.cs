namespace RestaurantReservation.Domain.RestaurantAggregate.Models;

public class Restaurant : AggregateRoot<RestaurantId>
{
    public string Name { get; private init; } = null!;
    public string Phone { get; private init; } = null!;
    public string Description { get; private init; } = null!;
    public string Url { get; private init; } = null!;
    public string WebSite { get; private init; } = null!;
    public WorkTime WorkTime { get; private set; } = null!;

    private readonly List<ReviewId> reviews;
    public IReadOnlyCollection<ReviewId> Reviews => this.reviews.AsReadOnly();

    private readonly List<TableId> tables;
    public IReadOnlyCollection<TableId> Tables => this.tables.AsReadOnly();

    private Restaurant()
    {
        this.reviews = new List<ReviewId>();
        this.tables = new List<TableId>();
    }

    public static Restaurant Create(
        RestaurantId restaurantId,
        string name,
        string phone,
        string description,
        string url,
        string webSite)
    {
        var restaurant = new Restaurant
        {
            Id = restaurantId,
            Name = name,
            Phone = phone,
            Description = description,
            Url = url,
            WebSite = webSite
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

    public void SetWorkTime(WorkTime workTime)
    {
        this.WorkTime = workTime;
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

        this.tables.Add(table.Id);

        return table;
    }

    public Review AddReview(
        ReviewId id,
        Customer customer,
        int ratingValue,
        string comment,
        string customerName,
        Reservation? reservation)
    {
        // if (this.Status != ReservationStatus.Completed || this.Status != ReservationStatus.Canceled) return null;

        // Create a review associated with this reservation
        var review = Review.Create(
            id,
            ratingValue,
            comment,
            this,
            customer,
            customerName,
            reservation);

        // Raise domain events or perform other actions related to review creation
        var @event = new ReviewCreatedDomainEvent(review);
        this.AddDomainEvent(@event);
        this.reviews.Add(review.Id);

        return review;
    }
}
