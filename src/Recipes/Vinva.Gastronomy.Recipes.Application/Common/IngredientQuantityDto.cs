using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Recipes.Application.Common
{
    public record IngredientQuantityDto
    {
        public required string Measure { get; init; }
        public required Decimal Quantity { get; init; }
    }
}
