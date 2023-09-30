namespace RestaurantReservation.Core.Events;

public class MessageEnvelope
{
    public object? Message { get; init; }
    public IDictionary<string, object?> Headers { get; init; }

    public MessageEnvelope(object? message, IDictionary<string, object?>? headers = null)
    {
        this.Message = message;
        this.Headers = headers ?? new Dictionary<string, object?>();
    }
}

// public class MessageEnvelope<TMessage> : MessageEnvelope
//     where TMessage : class, IMessage
// {
//     public MessageEnvelope(TMessage message, IDictionary<string, object?> header) : base(message, header)
//     {
//         Message = message;
//     }
//
//     public new TMessage? Message { get; }
// }
