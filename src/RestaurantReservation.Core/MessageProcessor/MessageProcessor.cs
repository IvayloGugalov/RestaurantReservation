using System.Linq.Expressions;
using System.Text.Json;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using RestaurantReservation.Core.Events;
using RestaurantReservation.Core.Extensions;

namespace RestaurantReservation.Core.MessageProcessor;

public class MessageProcessor : IMessageProcessor
{
    private readonly ILogger<MessageProcessor> logger;
    private readonly IMediator mediator;
    private readonly IMessageDbContext messagesDbContext;
    private readonly IPublishEndpoint publishEndpoint;

    public MessageProcessor(
        ILogger<MessageProcessor> logger,
        IMediator mediator,
        IMessageDbContext messagesDbContext,
        IPublishEndpoint publishEndpoint)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.messagesDbContext = messagesDbContext;
        this.publishEndpoint = publishEndpoint;
    }

    public Task PublishMessageAsync<TMessageEnvelope>(
        TMessageEnvelope messageEnvelope,
        CancellationToken ct = default)
        where TMessageEnvelope : MessageEnvelope
    {
        return this.SavePersistMessageAsync(messageEnvelope, MessageDeliveryType.Outbox, ct);
    }

    public Task<Guid> AddReceivedMessageAsync<TMessageEnvelope>(
        TMessageEnvelope messageEnvelope,
        CancellationToken ct = default)
        where TMessageEnvelope : MessageEnvelope
    {
        return this.SavePersistMessageAsync(messageEnvelope, MessageDeliveryType.Inbox, ct);
    }

    public Task AddInternalMessageAsync<TCommand>(
        TCommand internalCommand,
        CancellationToken ct = default)
        where TCommand : class, IInternalCommand
    {
        return this.SavePersistMessageAsync(new MessageEnvelope(internalCommand), MessageDeliveryType.Internal, ct);
    }

    public async Task<IReadOnlyList<Message>> GetByFilterAsync(
        Expression<Func<Message, bool>> predicate,
        CancellationToken ct = default)
    {
        return (await this.messagesDbContext.Messages.FindSync(predicate, cancellationToken: ct).ToListAsync(ct))
            .AsReadOnly();
    }

    public Task<Message> ExistMessageAsync(Guid messageId, CancellationToken ct = default)
    {
        return this.messagesDbContext.Messages.FindSync(
                x =>
                    x.Id == messageId &&x.DeliveryType == MessageDeliveryType.Inbox &&
                    x.MessageStatus == MessageStatus.Processed,
                cancellationToken: ct)
            .FirstOrDefaultAsync(ct);
    }

    public async Task ProcessInboxAsync(Guid messageId, CancellationToken ct = default)
    {
        var message = await this.messagesDbContext.Messages.FindSync(
                x => x.Id == messageId &&
                    x.DeliveryType == MessageDeliveryType.Inbox &&
                    x.MessageStatus == MessageStatus.InProgress,
                cancellationToken: ct)
            .FirstOrDefaultAsync(ct);

        await this.ChangeMessageStatusAsync(message, ct);
    }

    public async Task ProcessAsync(Guid messageId, MessageDeliveryType deliveryType, CancellationToken ct = default)
    {
        var message = await this.messagesDbContext.Messages.FindSync(
                x => x.Id == messageId && x.DeliveryType == deliveryType, cancellationToken: ct)
            .FirstOrDefaultAsync(ct);

        if (message is null) return;

        switch (deliveryType)
        {
            case MessageDeliveryType.Internal:
                var sentInternalMessage = await this.ProcessInternalAsync(message, ct);
                if (!sentInternalMessage) return;

                await this.ChangeMessageStatusAsync(message, ct);
                break;

            case MessageDeliveryType.Outbox:
                var sentOutbox = await this.ProcessOutboxAsync(message, ct);
                if (!sentOutbox) return;

                await this.ChangeMessageStatusAsync(message, ct);
                break;
        }
    }

    public async Task ProcessAllAsync(CancellationToken ct = default)
    {
        var messages = this.messagesDbContext.Messages.AsQueryable()
            .Where(x => x.MessageStatus != MessageStatus.Processed);

        foreach (var message in messages)
        {
            await this.ProcessAsync(message.Id, message.DeliveryType, ct);
        }
    }

    private async Task<Guid> SavePersistMessageAsync(
        MessageEnvelope messageEnvelope,
        MessageDeliveryType deliveryType,
        CancellationToken ct = default)
    {
        _ = messageEnvelope.Message ?? throw new NullReferenceException(nameof(messageEnvelope.Message));

        Guid id;
        if (messageEnvelope.Message is IEvent message) id = message.EventId;
        else id = NewId.NextGuid();

        await this.messagesDbContext.Messages.InsertOneAsync(
            new Message(
                id,
                messageEnvelope.Message.GetType().ToString(),
                JsonSerializer.Serialize(messageEnvelope),
                deliveryType),
            new InsertOneOptions(),
            ct);

        this.logger.LogInformation(
            "Message with id: {MessageID} and delivery type: {DeliveryType} saved in persistence message store.",
            id,
            deliveryType.ToString());

        return id;
    }

    private async Task<bool> ProcessOutboxAsync(Message message, CancellationToken cancellationToken)
    {
        var messageEnvelope = JsonSerializer.Deserialize<MessageEnvelope>(message.Data);

        if (messageEnvelope?.Message is null) return false;

        var data = JsonSerializer.Deserialize(messageEnvelope.Message.ToString() ?? string.Empty,
            TypeProvider.GetFirstMatchingTypeFromCurrentDomainAssembly(message.DataType) ?? typeof(object));

        if (data is not IEvent) return false;

        await this.publishEndpoint.Publish(data, context =>
        {
            foreach (var header in messageEnvelope.Headers) context.Headers.Set(header.Key, header.Value);
        }, cancellationToken);

        this.logger.LogInformation(
            "Message with id: {MessageId} and delivery type: {DeliveryType} processed from the persistence message store.",
            message.Id,
            message.DeliveryType);

        return true;
    }

    private async Task<bool> ProcessInternalAsync(Message message, CancellationToken cancellationToken)
    {
        var messageEnvelope = JsonSerializer.Deserialize<MessageEnvelope>(message.Data);

        if (messageEnvelope?.Message is null) return false;

        var data = JsonSerializer.Deserialize(messageEnvelope.Message.ToString() ?? string.Empty,
            TypeProvider.GetFirstMatchingTypeFromCurrentDomainAssembly(message.DataType) ?? typeof(object));

        if (data is not IInternalCommand internalCommand)
            return false;

        await this.mediator.Send(internalCommand, cancellationToken);

        this.logger.LogInformation(
            "InternalCommand with id: {EventID} and delivery type: {DeliveryType} processed from the persistence message store.",
            message.Id,
            message.DeliveryType);

        return true;
    }

    private async Task ChangeMessageStatusAsync(Message message, CancellationToken ct)
    {
        message.ChangeState(MessageStatus.Processed);

        message.Version++;
        await this.messagesDbContext.Messages.ReplaceOneAsync(m => m.Id == message.Id, message, new ReplaceOptions(), ct);
    }
}
