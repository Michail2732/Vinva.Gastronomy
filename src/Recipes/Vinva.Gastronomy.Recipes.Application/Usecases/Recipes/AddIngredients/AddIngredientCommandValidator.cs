using FluentValidation;
using Vinva.Gastronomy.Recipes.Application.Common;
using Vinva.Gastronomy.Recipes.Domain.Validations;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.AddIngredients
{
    public class AddIngredientCommandValidator : AbstractValidator<AddIngredientCommand>
    {
        public AddIngredientCommandValidator()
        {            
            RuleFor(a => a.RecipeId)
                .NotEmpty();

            RuleFor(a => a.Ingredients)
                .NotEmpty();

            RuleForEach(a => a.Ingredients)
                .ChildRules(a =>
                {
                    a.RuleFor(b => b.IngredientId)
                       .NotEmpty();

                    a.RuleFor(b => b.Quantities)
                        .NotEmpty();

                    a.RuleForEach(b => b.Quantities)
                        .ChildRules(b =>
                        {
                            b.RuleFor(c => c.Measure)
                                .NotEmpty()
                                .Must(RecipeDomainValidator.ValidateIngredientMeasure)
                                .WithMessage(RecipeDomainErrors.IncorrectIngredientMeasure);

                            b.RuleFor(c => c.Quantity)
                                .NotEmpty()
                                .Must(RecipeDomainValidator.ValidateIngredientQuantity)
                                .WithMessage(RecipeDomainErrors.IncorrectIngredientQuantity);
                        });
                });                            
        }
    }    
}
