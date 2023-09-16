using System.Reflection;

using MongoDB.Bson.Serialization;
using Moq;
using RestaurantReservation.Core.Model;
using RestaurantReservation.Domain;
using RestaurantReservation.Infrastructure.Mongo;
using RestaurantReservation.Infrastructure.Mongo.Data.Configurations;

namespace Restaurant.Reservation.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void AllStronglyTypedIdTypes_ShouldUse_IdSerializationProvider()
    {
        // Arrange
        var stronglyTypedIdTypes = Assembly.GetAssembly(typeof(RestaurantReservationDomain))
            ?.GetTypes()
            .Where(type => type.BaseType?.IsGenericType == true &&
                           type.BaseType.GetGenericTypeDefinition() == typeof(StronglyTypedId<>))
            .ToList();

        // Act
        var typesWithoutIdSerializer = stronglyTypedIdTypes!
            .Where(HasIdSerializationProviderUsage)
            .ToList();

        // Assert
        typesWithoutIdSerializer.Should().BeEmpty();
    }

    private static bool HasIdSerializationProviderUsage(Type type)
    {
        var idSerializationProviderType = typeof(IdSerializationProvider<>).MakeGenericType(type);

        return Assembly.GetAssembly(typeof(RestaurantReservationMongo))!
            .GetTypes()
            .Where(t => t.GetInterfaces().Contains(typeof(IBsonDocumentSerializer)))
            .Any(t => t == idSerializationProviderType);
    }
}
