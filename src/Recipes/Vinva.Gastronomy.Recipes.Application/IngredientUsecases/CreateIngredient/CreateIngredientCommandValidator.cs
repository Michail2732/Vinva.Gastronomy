using FluentValidation;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Application.IngredientUsecases.CreateIngredient
{
    public class CreateIngredientCommandValidator : AbstractValidator<CreateIngredientCommand>
    {
        public CreateIngredientCommandValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .WithMessage("Название ингредиенты не заполнено");

            RuleFor(a => a.Description)
                .NotEmpty()
                .WithMessage("Описание ингредиенты не заполнено");            
        }
    }
}
