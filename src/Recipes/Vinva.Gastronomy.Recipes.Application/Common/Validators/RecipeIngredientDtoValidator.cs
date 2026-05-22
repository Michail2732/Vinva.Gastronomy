using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Models;
using Vinva.Gastronomy.Recipes.Application.Constants;

namespace Vinva.Gastronomy.Recipes.Application.Common.Validators
{
    public class RecipeIngredientDtoValidator : AbstractValidator<RecipeIngredientDto>
    {
        public RecipeIngredientDtoValidator()
        {
            RuleFor(x => x)
            .Must(ValidatePrivate)
            .WithMessage(RecipesApplicationErrors.IncorrectRecipeIngredientDto);
        }

        private bool ValidatePrivate(RecipeIngredientDto dto)
        {
            if (dto.State == DtoState.Change)
            {
                return dto.IngredientId != Guid.Empty && dto.Quantities.Any();
            }
            else if (dto.State == DtoState.New)
            {
                return dto.IngredientId != Guid.Empty && dto.Quantities.Any();
            }
            else if (dto.State == DtoState.Remove)
            {
                return dto.IngredientId != Guid.Empty;
            }
            return true;
        }

    }
}
