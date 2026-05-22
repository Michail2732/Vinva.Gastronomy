using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Models;

namespace Vinva.Gastronomy.Recipes.Application.Common
{
    public record RecipeStepDto
    {
        public Guid Id { get; init; }        
        public required string Description { get; init; }
        public string? Comment { get; init; }
        public required int SeqNumber { get; init; }
        public Guid? PhotoId { get; init; }
        /// Используется в сценарии обновления рецепта
        public DtoState State { get; init; }
    }
}
