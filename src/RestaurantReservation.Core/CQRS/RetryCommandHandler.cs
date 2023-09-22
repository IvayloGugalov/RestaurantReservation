using MediatR;

namespace RestaurantReservation.Core.CQRS;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class RetryCommandHandlerAttribute : Attribute
{
}

public sealed class RetryCommandHandler<TCommand>: ICommandHandler<TCommand>
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> handler;

    public RetryCommandHandler(ICommandHandler<TCommand> handler)
    {
        this.handler = handler;
    }

    public async Task<Unit> Handle(TCommand request, CancellationToken ct)
    {
        for (var i = 0; ; i++)
        {
            try
            {
                var result = await this.handler.Handle(request, ct);
                return result;
            }
            catch (Exception ex)
            {
                if (i >= 5 || !IsDatabaseException(ex)) throw;
            }
        }
    }

    private static bool IsDatabaseException(Exception exception)
    {
        var message = exception.InnerException?.Message;

        if (message == null)
            return false;

        return message.Contains("The connection is broken and recovery is not possible")
               || message.Contains("error occurred while establishing a connection");
    }
}
