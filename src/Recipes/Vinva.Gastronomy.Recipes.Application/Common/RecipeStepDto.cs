using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Recipes.Application.Common
{
    public record RecipeStepDto
    { 
        public required string Description { get; init; }
        public string? Comment { get; init; }
        public required int SeqNumber { get; init; }
        public Guid? PhotoId { get; init; }
    }
}
