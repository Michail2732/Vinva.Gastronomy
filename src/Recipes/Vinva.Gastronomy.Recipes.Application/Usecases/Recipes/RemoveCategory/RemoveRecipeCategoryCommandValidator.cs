using FluentValidation;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.RemoveCategory
{
    public class RemoveRecipeCategoryCommandValidator : AbstractValidator<RemoveRecipeCategoryCommand>
    {
        public RemoveRecipeCategoryCommandValidator()
        {
            RuleFor(a => a.CategoryIds)
                .NotEmpty()
                .WithMessage("Идентификаторы категорий не заполнены");
        }
    }
}
