using FluentValidation;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Create
{
    public sealed class CreateRecipeCommandValidator : AbstractValidator<CreateRecipeCommand>
    {
        public CreateRecipeCommandValidator()
        {
            RuleFor(command => command.Name).NotEmpty();
            RuleFor(command => command.Description).NotEmpty();
            RuleForEach(command => command.Characteristics)
                .ChildRules(characteristic => characteristic.RuleFor(value => value.Name).NotEmpty());
            RuleForEach(command => command.Ingredients)
                .ChildRules(ingredient => ingredient.RuleFor(value => value.Name).NotEmpty());
        }
    }
}
