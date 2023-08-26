namespace RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;

public record WorkTime
{
    public WorkingHours Monday { get; private set; } = null!;
    public WorkingHours Tuesday { get; private set; } = null!;
    public WorkingHours Wednesday { get; private set; } = null!;
    public WorkingHours Thursday { get; private set; } = null!;
    public WorkingHours Friday { get; private set; } = null!;
    public WorkingHours Saturday { get; private set; } = null!;
    public WorkingHours Sunday { get; private set; } = null!;

    private WorkTime()
    {
    }

    public WorkTime(
        WorkingHours Monday,
        WorkingHours Tuesday,
        WorkingHours Wednesday,
        WorkingHours Thursday,
        WorkingHours Friday,
        WorkingHours Saturday,
        WorkingHours Sunday)
    {
        this.Monday = Monday;
        this.Tuesday = Tuesday;
        this.Wednesday = Wednesday;
        this.Thursday = Thursday;
        this.Friday = Friday;
        this.Saturday = Saturday;
        this.Sunday = Sunday;
    }
}

public record WorkingHours
{
    public TimeSpan OpeningTime { get; private init; }
    public TimeSpan ClosingTime { get; private init; }

    private WorkingHours()
    {
    }

    public static WorkingHours Add(TimeSpan openingTime, TimeSpan closingTime)
    {
        if (openingTime >= closingTime) throw new ArgumentException("Opening time must be before closing time.");

        return new WorkingHours
        {
            OpeningTime = openingTime,
            ClosingTime = closingTime
        };
    }

    public bool IsWithinWorkingHours(DateTime time)
    {
        var currentTime = time.TimeOfDay;
        return currentTime >= this.OpeningTime && currentTime <= this.ClosingTime;
    }
}