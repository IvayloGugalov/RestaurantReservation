using MongoDB.Bson.Serialization.Attributes;
using RestaurantReservation.Core.Model;

namespace RestaurantReservation.Core.MessageProcessor;

public class Message : IVersion
{
    [BsonId]
    public Guid Id { get; private set; }
    public string DataType { get; private set; }
    public string Data { get; private set; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Created { get; private set; }
    public int RetryCount { get; private set; }
    public MessageStatus MessageStatus { get; private set; }
    public MessageDeliveryType DeliveryType { get; private set; }
    public long Version { get; set; }

    public Message(Guid id, string dataType, string data, MessageDeliveryType deliveryType)
    {
        this.Id = id;
        this.DataType = dataType;
        this.Data = data;
        this.DeliveryType = deliveryType;
        this.Created = DateTime.Now;
        this.MessageStatus = MessageStatus.InProgress;
        this.RetryCount = 0;
    }

    public void ChangeState(MessageStatus messageStatus)
    {
        this.MessageStatus = messageStatus;
    }

    public void IncreaseRetry()
    {
        this.RetryCount++;
    }
}
