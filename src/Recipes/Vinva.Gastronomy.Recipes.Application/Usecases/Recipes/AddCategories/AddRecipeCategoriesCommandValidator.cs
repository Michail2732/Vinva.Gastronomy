using FluentValidation;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.AddCategories
{
    public class AddRecipeCategoriesCommandValidator : AbstractValidator<AddRecipeCategoriesCommand>
    {
        public AddRecipeCategoriesCommandValidator()
        {
            RuleFor(a => a.CategoryIds)
                .NotEmpty()
                .WithMessage("Идентификаторы категорий не заполнены");
        }
    }
}
