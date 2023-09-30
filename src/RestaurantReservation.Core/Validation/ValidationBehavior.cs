using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace RestaurantReservation.Core.Validation;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    private readonly IServiceProvider serviceProvider;

    public ValidationBehavior(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var validator = this.serviceProvider.GetService<IValidator<TRequest>>();
        if (validator is null) return await next();

        await validator.HandleValidationAsync(request);

        return await next();
    }
}
