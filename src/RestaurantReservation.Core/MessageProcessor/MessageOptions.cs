namespace RestaurantReservation.Core.MessageProcessor;

public class MessageOptions
{
    public int? Interval { get; set; } = 30;
    public bool Enabled { get; set; } = true;
    public string? ConnectionString { get; set; }
    public string? DatabaseName { get; set; }
}
