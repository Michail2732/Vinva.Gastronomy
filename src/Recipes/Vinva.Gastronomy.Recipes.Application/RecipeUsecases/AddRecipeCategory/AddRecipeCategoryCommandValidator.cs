using FluentValidation;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.AddRecipeCategory
{
    public class AddRecipeCategoryCommandValidator : AbstractValidator<AddRecipeCategoryCommand>
    {
        public AddRecipeCategoryCommandValidator()
        {
            RuleFor(a => a.CategoryIds)
                .NotEmpty()
                .WithMessage("Идентификаторы категорий не заполнены");
        }
    }
}
