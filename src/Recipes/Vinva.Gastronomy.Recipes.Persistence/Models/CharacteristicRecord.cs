using System;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Persistence.Models
{
    public class CharacteristicRecord
    {
        public Guid RecipeId { get; set; }
        public string Name { get; set; } = null!;
        public string? Measure { get; set; }
        public CharacteristicType Type { get; set; }
        public string? StringValue { get; set; }
        public double? NumberValue { get; set; }
        public double? RangeMin { get; set; }
        public double? RangeMax { get; set; }
        public string[]? ArrayItems { get; set; }
    }
}