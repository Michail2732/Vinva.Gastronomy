using FluentValidation;
using Vinva.Gastronomy.Common.Models;
using Vinva.Gastronomy.Recipes.Application.Constants;

namespace Vinva.Gastronomy.Recipes.Application.Common.Validators
{
    public class RecipeStepDtoValidator : AbstractValidator<RecipeStepDto>
    {
        public RecipeStepDtoValidator()
        {
            RuleFor(x => x)
            .Must(ValidatePrivate)
            .WithMessage(RecipesApplicationErrors.IncorrectRecipeIngredientDto);
        }

        private bool ValidatePrivate(RecipeStepDto dto)
        {
            if (dto.State == DtoState.Change)
            {
                return dto.Id != Guid.Empty && !string.IsNullOrEmpty(dto.Description)
                    && dto.SeqNumber > 0;
            }
            else if (dto.State == DtoState.New)
            {
                return !string.IsNullOrEmpty(dto.Description)
                    && dto.SeqNumber > 0;
            }
            else if (dto.State == DtoState.Remove)
            {
                return dto.Id != Guid.Empty;
            }
            return true;
        }
    }    
}
