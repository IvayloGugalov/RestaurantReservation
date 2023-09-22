using System.Reflection;

namespace Restaurant.Reservation.Test;

[TestFixture]
public class ArchitectureTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    [TestCase(typeof(NotFoundException), "NotFound")]
    [TestCase(typeof(ConflictException), "Conflict")]
    public void Exceptions_ShouldContain_BaseExceptionName(Type exceptionClassType, string requiredString)
    {
        var result = Assembly.GetAssembly(typeof(RestaurantReservationDomain))
            ?.GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false } && exceptionClassType.IsAssignableFrom(type))
            .ToList();

        foreach (var type in result!)
        {
            Assert.That(
                type.Name.Contains(requiredString, StringComparison.OrdinalIgnoreCase),
                Is.True,
                $"{type.Name} does not contain {requiredString} in name");
        }
    }
}
