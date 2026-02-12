using FluentValidation;
using Vinva.Gastronomy.Recipes.Application.Common;
using Vinva.Gastronomy.Recipes.Domain.Validations;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.AddIngredients
{
    public class AddIngredientCommandValidator : AbstractValidator<AddIngredientCommand>
    {
        public AddIngredientCommandValidator()
        {
            RuleFor(a => a.RecipeId)
                .NotEmpty();

            RuleFor(a => a.Ingredients)
                .NotEmpty();
        }
    }

    public class AddIngredientIngredientValidator : AbstractValidator<RecipeIngredientDto>
    {
        public AddIngredientIngredientValidator()
        {
            RuleFor(a => a.IngredientId)
                .NotEmpty();

            RuleFor(a => a.Quantities)
                .NotEmpty();
        }
    }

    public class AddIngredientQuantiryValidator : AbstractValidator<IngredientQuantityDto>
    {
        public AddIngredientQuantiryValidator()
        {
            RuleFor(a => a.Measure)
                .NotEmpty()
                .Must(RecipeDomainValidator.ValidateIngredientMeasure)
                .WithMessage(RecipeDomainErrors.IncorrectIngredientMeasure);

            RuleFor(a => a.Quantity)
                .NotEmpty()
                .Must(RecipeDomainValidator.ValidateIngredientQuantity)
                .WithMessage(RecipeDomainErrors.IncorrectIngredientQuantity);
        }
    }
}
