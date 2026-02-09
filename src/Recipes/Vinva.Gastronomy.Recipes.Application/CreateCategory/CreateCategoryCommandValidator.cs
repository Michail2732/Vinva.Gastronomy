using FluentValidation;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Application.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .WithMessage("Название категории не заполнено");

            RuleFor(a => a.Description)
                .NotEmpty()
                .WithMessage("Описание категории не заполнено");

            RuleFor(a => a.Type)
                .NotEmpty()
                .Must(a =>
                {
                    return Enum.GetNames<CategoryType>().Contains(Enum.GetName(a));                    
                })
                .WithMessage("Описание категории не заполнено");
        }
    }
}
