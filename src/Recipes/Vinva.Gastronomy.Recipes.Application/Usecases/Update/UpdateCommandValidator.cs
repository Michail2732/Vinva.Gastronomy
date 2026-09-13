using FluentValidation;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Update
{
    public sealed class UpdateCommandValidator : AbstractValidator<UpdateCommand>
    {
        public UpdateCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.Name).NotEmpty().When(command => command.Name is not null);
            RuleFor(command => command.Description).NotEmpty().When(command => command.Description is not null);
            RuleForEach(command => command.Characteristics)
                .ChildRules(characteristic => characteristic.RuleFor(value => value.Name).NotEmpty());
        }
    }
}
