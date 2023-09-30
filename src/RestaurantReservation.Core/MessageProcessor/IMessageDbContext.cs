using MongoDB.Driver;

namespace RestaurantReservation.Core.MessageProcessor;

public interface IMessageDbContext
{
    IMongoCollection<Message> Messages { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
