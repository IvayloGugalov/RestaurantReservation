using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;

[Owned]
public class WorkTime
{
    public WorkingHours Monday { get; private set; } = null!;
    public WorkingHours Tuesday { get; private set; } = null!;
    public WorkingHours Wednesday { get; private set; } = null!;
    public WorkingHours Thursday { get; private set; } = null!;
    public WorkingHours Friday { get; private set; } = null!;
    public WorkingHours Saturday { get; private set; } = null!;
    public WorkingHours Sunday { get; private set; } = null!;

    private WorkTime() { }

    public static WorkTime CreateEmpty() => new();

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

[Owned]
public class WorkingHours
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

public class WorkTimeConverter : JsonConverter<WorkTime>
{
    public override WorkTime ReadJson(JsonReader reader, Type objectType, WorkTime? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return WorkTime.CreateEmpty()!;
        if (reader.TokenType != JsonToken.StartObject) throw new JsonSerializationException("Expected StartObject token.");

        var workingHoursDict = new Dictionary<string, WorkingHours>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                var propertyName = (string)reader.Value!;

                if (!reader.Read())
                {
                    throw new JsonSerializationException($"Unexpected end when reading value for property: {propertyName}");
                }

                var workingHours = serializer.Deserialize<WorkingHours>(reader);

                workingHoursDict.Add(propertyName, workingHours);
            }
            else if (reader.TokenType == JsonToken.EndObject)
            {
                return WorkTime.Create(
                    Monday: workingHoursDict.GetValueOrDefault("Monday"),
                    Tuesday: workingHoursDict.GetValueOrDefault("Tuesday"),
                    Wednesday: workingHoursDict.GetValueOrDefault("Wednesday"),
                    Thursday: workingHoursDict.GetValueOrDefault("Thursday"),
                    Friday: workingHoursDict.GetValueOrDefault("Friday"),
                    Saturday: workingHoursDict.GetValueOrDefault("Saturday"),
                    Sunday: workingHoursDict.GetValueOrDefault("Sunday"));
            }
        }

        throw new JsonSerializationException("Unexpected end when reading JSON.");
    }

    public override void WriteJson(JsonWriter writer, WorkTime? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteStartObject();
        WriteProperty(writer, serializer, "Monday", value.Monday);
        WriteProperty(writer, serializer, "Tuesday", value.Tuesday);
        WriteProperty(writer, serializer, "Wednesday", value.Wednesday);
        WriteProperty(writer, serializer, "Thursday", value.Thursday);
        WriteProperty(writer, serializer, "Friday", value.Friday);
        WriteProperty(writer, serializer, "Saturday", value.Saturday);
        WriteProperty(writer, serializer, "Sunday", value.Sunday);
        writer.WriteEndObject();
    }

    private static void WriteProperty(JsonWriter writer, JsonSerializer serializer, string propertyName, WorkingHours value)
    {
        writer.WritePropertyName(propertyName);
        serializer.Serialize(writer, value);
    }
}

public class WorkingHoursConverter : JsonConverter<WorkingHours>
{
    public override void WriteJson(JsonWriter writer, WorkingHours? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }
        writer.WriteStartObject();
        writer.WritePropertyName(nameof(WorkingHours.OpeningTime));
        writer.WriteValue(value.OpeningTime.Ticks);
        writer.WritePropertyName(nameof(WorkingHours.ClosingTime));
        writer.WriteValue(value.ClosingTime.Ticks);
        writer.WriteEndObject();
    }

    public override WorkingHours? ReadJson(JsonReader reader, Type objectType, WorkingHours? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null || reader.Value == null) return null;
        if (reader.TokenType != JsonToken.StartObject)
        {
            throw new JsonSerializationException("Expected StartObject token.");
        }
        TimeSpan openingTime = default;
        TimeSpan closingTime = default;

        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                var propertyName = (string)reader.Value;

                if (!reader.Read())
                {
                    throw new JsonSerializationException($"Unexpected end when reading value for property: {propertyName}");
                }

                if (propertyName.Equals("OpeningTime", StringComparison.OrdinalIgnoreCase))
                {
                    openingTime = TimeSpan.Parse((string)reader.Value);
                }
                else if (propertyName.Equals("ClosingTime", StringComparison.OrdinalIgnoreCase))
                {
                    closingTime = TimeSpan.Parse((string)reader.Value);
                }
            }
            else if (reader.TokenType == JsonToken.EndObject)
            {
                return WorkingHours.Create(openingTime, closingTime);
            }
        }

        throw new JsonSerializationException("Unexpected end when reading JSON.");
    }
}
