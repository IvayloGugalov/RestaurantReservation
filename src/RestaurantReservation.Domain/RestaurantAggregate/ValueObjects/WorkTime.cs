﻿namespace RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;

public record WorkTime
{
    public WorkingHours Monday { get; private init; } = null!;
    public WorkingHours Tuesday { get; private init; } = null!;
    public WorkingHours Wednesday { get; private init; } = null!;
    public WorkingHours Thursday { get; private init; } = null!;
    public WorkingHours Friday { get; private init; } = null!;
    public WorkingHours Saturday { get; private init; } = null!;
    public WorkingHours Sunday { get; private init; } = null!;

    private WorkTime() { }

    public static WorkTime? CreateEmpty() => new();

    public static WorkTime Create(
        WorkingHours Monday,
        WorkingHours Tuesday,
        WorkingHours Wednesday,
        WorkingHours Thursday,
        WorkingHours Friday,
        WorkingHours Saturday,
        WorkingHours Sunday)
    {
        return new WorkTime
        {
            Monday = Monday,
            Tuesday = Tuesday,
            Wednesday = Wednesday,
            Thursday = Thursday,
            Friday = Friday,
            Saturday = Saturday,
            Sunday = Sunday
        };
    }
}

public record WorkingHours
{
    public TimeSpan OpeningTime { get; private init; }
    public TimeSpan ClosingTime { get; private init; }

    private WorkingHours() { }

    public static WorkingHours CreateEmpty()
        => new();

    public static WorkingHours Create(TimeSpan openingTime, TimeSpan closingTime)
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
