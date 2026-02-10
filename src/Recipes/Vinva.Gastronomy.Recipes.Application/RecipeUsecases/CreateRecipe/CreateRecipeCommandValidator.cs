using FluentValidation;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.CreateRecipe
{
    public class CreateRecipeCommandValidator : AbstractValidator<CreateRecipeCommand>
    {
        public CreateRecipeCommandValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .WithMessage("Название рецепта не заполнено");

            RuleFor(a => a.Description)
                .NotEmpty()
                .WithMessage("Описание рецепта не заполнено");

            RuleFor(a => a.CookingTime)
                .NotEmpty()
                .Must(a =>
                {
                    return a.TotalSeconds > TimeSpan.FromSeconds(60).TotalSeconds;
                })
                .WithMessage("Время приготовления рецепта должно быть больше 60 секунд");
        }
    }
}
