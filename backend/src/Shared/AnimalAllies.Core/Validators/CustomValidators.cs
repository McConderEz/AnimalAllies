using AnimalAllies.SharedKernel.Shared;
using FluentValidation;

namespace AnimalAllies.Core.Validators;

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

            var errorsSerialized = result.Errors.Select(e => e.Serialize());
            
            context.AddFailure(string.Join('\n', errorsSerialized));
        });
    }

    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule, Error error)
    {
        return rule.WithMessage(error.Serialize());
    }
}