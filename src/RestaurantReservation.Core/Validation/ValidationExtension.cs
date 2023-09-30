using FluentValidation;

namespace RestaurantReservation.Core.Validation;

internal static class ValidationExtension
{
    public static async Task HandleValidationAsync<TRequest>(this IValidator<TRequest> validator, TRequest request)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors?.First()?.ErrorMessage);
        }
    }
}
