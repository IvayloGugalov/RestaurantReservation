using System.Text.RegularExpressions;

namespace RestaurantReservation.Domain.Common.ValueObjects;

public record Email
{
    public const string EmailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
        if (value.Length > 320) throw new ArgumentOutOfRangeException(nameof(value), "Email value is too long");
        if (!Regex.IsMatch(value, EmailRegex)) throw new ArgumentException("The provided value does not match an email");
        //more validations
        this.Value = value;
    }
}