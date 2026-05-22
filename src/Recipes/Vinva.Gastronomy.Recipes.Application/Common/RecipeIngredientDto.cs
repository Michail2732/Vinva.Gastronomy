using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Models;

namespace Vinva.Gastronomy.Recipes.Application.Common
{
    public record RecipeIngredientDto
    {
        public required Guid IngredientId { get; init; }
        public required string IngredientName { get; init; }
        public string? Comment { get; init; }
        public bool IsRequired { get; init; }
        public required IngredientQuantityDto[] Quantities { get; init; }
        /// Используется в сценарии обновления рецепта
        public DtoState State { get; init; }
    }    
}
