namespace RestaurantReservation.Core.Polly;

public class PolicyOptions
{
    public RetryOptions? Retry { get; set; }
    public CircuitBreakerOptions? CircuitBreaker { get; set; }
}
