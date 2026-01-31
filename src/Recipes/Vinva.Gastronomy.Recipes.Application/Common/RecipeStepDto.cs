using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Recipes.Application.Common
{
    public readonly record struct RecipeStepDto
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public int SeqNumber { get; init; }
        public Guid? PhotoId { get; init; }
    }
}
