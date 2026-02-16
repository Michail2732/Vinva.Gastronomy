using FluentValidation;

namespace Vinva.Gastronomy.Recipes.Application.CategoryUsecases.RemoveCategory
{
    public class RemoveCategoryCommandValidator : AbstractValidator<RemoveCategoryCommand>
    {
        public RemoveCategoryCommandValidator()
        {
            RuleFor(a => a.CategoryIds).NotEmpty();
        }
    }
}
