using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Recipes.Application.Common
{
    public readonly record struct RecipeIngredientDto
    {
        public Guid IngredientId { get; init; }
        public string IngredientName { get; init; }
        public string Measure { get; init; }
        public bool IsRequired { get; init; }
    }
}
