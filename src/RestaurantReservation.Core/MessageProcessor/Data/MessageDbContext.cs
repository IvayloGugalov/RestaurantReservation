using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RestaurantReservation.Core.Mongo.Data;

namespace RestaurantReservation.Core.MessageProcessor.Data;

public class MessageDbContext : MongoDbContext, IMessageDbContext
{
    public IMongoCollection<Message> Messages { get; }

    public MessageDbContext(IOptions<MessageOptions> options)
        : base(options.Value.ConnectionString, options.Value.DatabaseName)
    {
        this.Messages = this.GetCollection<Message>();
    }
}
