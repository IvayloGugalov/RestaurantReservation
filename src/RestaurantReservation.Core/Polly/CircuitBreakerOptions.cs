namespace RestaurantReservation.Core.Polly;

public class CircuitBreakerOptions
{
    public int RetryCount { get; set; }
    public int BreakDuration { get; set; }
}
