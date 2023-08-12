using RestaurantReservation.Core.Model;
using RestaurantReservation.Domain.Common;
using RestaurantReservation.Domain.CustomerAggregate.ValueObjects;

namespace RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;

public record ReviewId(Guid Value)
{
    public static implicit operator Guid(ReviewId reviewId)
    {
        return reviewId.Value;
    }
};

public record Rating
{
    public int Value { get; }

    public Rating(int value)
    {
        if (value is < 0 or > 5) throw new ArgumentException("Invalid Rating value provided");
        this.Value = value;
    }
}

public class Review : Entity<ReviewId>
{
    public Rating Rating { get; private set;  }
    public string Comment { get; private set;  }
    public RestaurantId RestaurantId { get; private set;  }
    public CustomerId CustomerId { get; private set;  }
    public string CustomerName { get; private set;  }
    public DateTime CreatedAt { get; private set;  }

    private Review () { }

    public static Review Create(
        ReviewId id,
        int ratingValue,
        string comment,
        DateTime createdAt,
        RestaurantId restaurantId,
        CustomerId customerId,
        string customerName)
    {
        var newReview = new Review
        {
            Id = id,
            Rating = new Rating(ratingValue),
            Comment = comment,
            CreatedAt = createdAt,
            RestaurantId = restaurantId,
            CustomerId = customerId,
            CustomerName = customerName
        };

        return newReview;
    }
}
