using MediatR;

namespace RestaurantReservation.Core.CQRS;

public interface IQuery<out T> : IRequest<T>
    where T : notnull
{
}
