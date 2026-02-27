using FluentValidation;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Categories.RemoveCategory
{
    public class RemoveCategoryCommandValidator : AbstractValidator<RemoveCategoryCommand>
    {
        public RemoveCategoryCommandValidator()
        {
            RuleFor(a => a.CategoryIds).NotEmpty();
        }
    }
}
