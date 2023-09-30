using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RestaurantReservation.Core.Web;

namespace RestaurantReservation.Core.Events;

public class EventDispatcher : IEventDispatcher
{
    private readonly ILogger<EventDispatcher> logger;
    private readonly IEventMapper eventMapper;
    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly IHttpContextAccessor httpContextAccessor;

    public EventDispatcher(
        ILogger<EventDispatcher> logger,
        IEventMapper eventMapper,
        IServiceScopeFactory serviceScopeFactory,
        IHttpContextAccessor httpContextAccessor)
    {
        this.logger = logger;
        this.eventMapper = eventMapper;
        this.serviceScopeFactory = serviceScopeFactory;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task SendAsync<T>(IReadOnlyList<T> events, Type? type = null, CancellationToken ct = default)
        where T : IEvent
    {
        if (events.Count == 0) return;

        var eventType = type != null && type.IsAssignableTo(typeof(IInternalCommand))
            ? EventType.InternalCommand
            : EventType.DomainEvent;

        switch (events)
        {
            case IReadOnlyList<IDomainEvent> domainEvents:
            {
                var integrationEvents = await this.MapDomainEventToIntegrationEventAsync(domainEvents);
                await PublishIntegrationEvent(integrationEvents);
                break;
            }
            case IReadOnlyList<IIntegrationEvent> integrationEvents:
                await PublishIntegrationEvent(integrationEvents);
                break;
        }

        if (type == null || eventType != EventType.InternalCommand) return;

        var internalMessages = await this.MapDomainEventToInternalCommandAsync(events as IReadOnlyList<IDomainEvent>)
            .ConfigureAwait(false);

        foreach (var internalMessage in internalMessages)
        {
            // await _persistMessageProcessor.AddInternalMessageAsync(internalMessage, ct);
        }

        return;

        async Task PublishIntegrationEvent(IEnumerable<IIntegrationEvent> integrationEvents)
        {
            foreach (var integrationEvent in integrationEvents)
            {
                // publish message
            }
        }
    }

    public async Task SendAsync<T>(T @event, Type? type = null, CancellationToken ct = default)
        where T : IEvent
    {
        await this.SendAsync(new[] { @event }, type, ct);
    }

    private Task<IReadOnlyList<IIntegrationEvent>> MapDomainEventToIntegrationEventAsync(
        IReadOnlyList<IDomainEvent> events)
    {
        this.logger.LogTrace("Processing integration events start...");

        var wrappedIntegrationEvents = GetWrappedIntegrationEvents(events.ToList())?.ToList();
        if (wrappedIntegrationEvents?.Count > 0)
            return Task.FromResult<IReadOnlyList<IIntegrationEvent>>(wrappedIntegrationEvents);

        var integrationEvents = new List<IIntegrationEvent>();
        using var scope = this.serviceScopeFactory.CreateScope();
        foreach (var @event in events)
        {
            var eventType = @event.GetType();
            this.logger.LogTrace($"Handling domain event: {eventType.Name}");

            var integrationEvent = this.eventMapper.MapToIntegrationEvent(@event);

            if (integrationEvent is null) continue;

            integrationEvents.Add(integrationEvent);
        }

        this.logger.LogTrace("Processing integration events done...");

        return Task.FromResult<IReadOnlyList<IIntegrationEvent>>(integrationEvents);
    }

    private Task<IReadOnlyList<IInternalCommand>> MapDomainEventToInternalCommandAsync(IEnumerable<IDomainEvent> events)
    {
        this.logger.LogTrace("Processing internal message start...");

        var internalCommands = new List<IInternalCommand>();
        using var scope = this.serviceScopeFactory.CreateScope();
        foreach (var @event in events)
        {
            var eventType = @event.GetType();
            this.logger.LogTrace($"Handling domain event: {eventType.Name}");

            var integrationEvent = this.eventMapper.MapToInternalCommand(@event);

            if (integrationEvent is null) continue;

            internalCommands.Add(integrationEvent);
        }

        this.logger.LogTrace("Processing internal message done...");

        return Task.FromResult<IReadOnlyList<IInternalCommand>>(internalCommands);
    }

    private IEnumerable<IIntegrationEvent> GetWrappedIntegrationEvents(IEnumerable<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            var genericType = typeof(IntegrationEventWrapper<>).MakeGenericType(domainEvent.GetType());
            var domainNotificationEvent = (IIntegrationEvent)Activator.CreateInstance(genericType, domainEvent)!;

            yield return domainNotificationEvent;
        }
    }

    private IDictionary<string, object?> SetHeaders()
    {
        var headers = new Dictionary<string, object?>
        {
            { "CorrelationId", this.httpContextAccessor?.HttpContext?.GetCorrelationId() },
            { "UserId", this.httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) },
            { "UserName", this.httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Name) }
        };

        return headers;
    }
}
