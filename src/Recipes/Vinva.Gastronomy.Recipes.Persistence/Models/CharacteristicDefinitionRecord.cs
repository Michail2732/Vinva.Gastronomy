using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Persistence.Models
{
    public class CharacteristicDefinitionRecord
    {
        public string Name { get; set; } = string.Empty;
        public CharacteristicType Type { get; set; }
        public string? Measure { get; set; }
    }
}