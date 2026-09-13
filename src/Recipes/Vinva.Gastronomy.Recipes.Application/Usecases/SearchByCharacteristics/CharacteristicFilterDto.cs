using System.Text.Json;
using Vinva.Gastronomy.Common.Models.Search;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.SearchByCharacteristics
{
    public sealed class CharacteristicFilterDto
    {
        public required string Name { get; init; }
        public required CharacteristicType Type { get; init; }
        public required Operator Operator { get; init; }
        public required JsonElement Value { get; init; }
        public Logic Logic { get; init; } = Logic.And;
    }
}
