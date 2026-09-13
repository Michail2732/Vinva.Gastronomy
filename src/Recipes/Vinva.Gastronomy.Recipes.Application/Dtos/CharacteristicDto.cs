using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Models;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Application.Common
{
    public record CharacteristicDto
    {        
        public required Guid RecipeId { get; init; }
        public required string Name { get; init; }
        public required CharacteristicType Type { get; init; }
        public required JsonObject Value { get; init; }
        public string? Measure { get; set; }
        public DtoState State { get; set; }
    }
}
