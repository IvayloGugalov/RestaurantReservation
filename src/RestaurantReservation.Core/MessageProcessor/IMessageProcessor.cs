using System.Linq.Expressions;
using RestaurantReservation.Core.Events;

namespace RestaurantReservation.Core.MessageProcessor;

public interface IMessageProcessor
{
    Task PublishMessageAsync<TMessageEnvelope>(
        TMessageEnvelope messageEnvelope,
        CancellationToken ct = default)
        where TMessageEnvelope : MessageEnvelope;

    Task<Guid> AddReceivedMessageAsync<TMessageEnvelope>(
        TMessageEnvelope messageEnvelope,
        CancellationToken ct = default)
        where TMessageEnvelope : MessageEnvelope;

    Task AddInternalMessageAsync<TCommand>(
        TCommand internalCommand,
        CancellationToken ct = default)
        where TCommand : class, IInternalCommand;

    Task<IReadOnlyList<Message>> GetByFilterAsync(
        Expression<Func<Message, bool>> predicate,
        CancellationToken ct = default);

    Task<Message> ExistMessageAsync(Guid messageId,
        CancellationToken ct = default);

    Task ProcessInboxAsync(
        Guid messageId,
        CancellationToken ct = default);

    Task ProcessAsync(Guid messageId, MessageDeliveryType deliveryType, CancellationToken ct = default);

    Task ProcessAllAsync(CancellationToken ct = default);
}
