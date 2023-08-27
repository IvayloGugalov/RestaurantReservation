﻿using MassTransit;

namespace RestaurantReservation.Domain.RestaurantAggregate.Events;

public record CreateRestaurantResult(Guid Id);

public record CreateRestaurantEvent(
    string Name,
    string Phone,
    string Description,
    string Url,
    string WebSite,
    WorkTime WorkTime) : ICommand<CreateRestaurantResult>, IEvent
{
    public Guid Id { get; } = NewId.NextGuid();
}

public record RestaurantCreatedDomainEvent(
    string Name,
    string Phone,
    string Description,
    string Url,
    string WebSite,
    WorkTime WorkTime) : IDomainEvent;