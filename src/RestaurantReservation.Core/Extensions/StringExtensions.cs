using System.Globalization;

namespace RestaurantReservation.Core.Extensions;

public static class StringExtensions
{
    public static string ToCamelCase(this string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        var firstChar = input[..1].ToLower(CultureInfo.InvariantCulture);
        var restOfString = input[1..];

        return firstChar + restOfString;
    }

}
