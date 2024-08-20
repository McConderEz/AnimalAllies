using AnimalAllies.Domain.Models;
using FluentValidation;

namespace AnimalAllies.Application.Validators;

public static class CustomValidators
{
    public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
        this IRuleBuilder<T, TElement> ruleBuilder,
        Func<TElement, Result<TValueObject>> factoryMethod)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            Result<TValueObject> result = factoryMethod(value);

            if (result.IsSuccess)
                return;
            
            context.AddFailure(result.Error.ErrorMessage);
        });
    }
}