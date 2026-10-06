using System.Text.Json.Nodes;
using Vinva.Gastronomy.Recipes.Application.Common;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Domain.Models;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Common
{
    internal static class CharacteristicDtoMapper
    {
        public static RecipeCharacteristic ToDomain(CharacteristicDto dto, Guid? recipeId = null)
        {
            var value = dto.Type switch
            {
                CharacteristicType.String => new CharecteristicStringValue
                {
                    Data = GetRequiredValue<string>(dto.Value, nameof(CharecteristicStringValue.Data))
                } as ICharacteristicValue,
                CharacteristicType.Number => new CharecteristicNumberValue
                {
                    Data = GetRequiredValue<double>(dto.Value, nameof(CharecteristicNumberValue.Data))
                },
                CharacteristicType.Range => new CharecteristicRangeValue
                {
                    Min = GetRequiredValue<double>(dto.Value, nameof(CharecteristicRangeValue.Min)),
                    Max = GetRequiredValue<double>(dto.Value, nameof(CharecteristicRangeValue.Max))
                },
                CharacteristicType.Array => new CharecteristicArrayValue
                {
                    Items = GetRequiredArray(dto.Value, nameof(CharecteristicArrayValue.Items))
                },
                _ => throw new ArgumentOutOfRangeException(nameof(dto.Type), dto.Type, "Unsupported characteristic type.")
            };

            return new RecipeCharacteristic(recipeId ?? Guid.Empty, dto.Name, dto.Type, value)
            {
                Measure = dto.Measure
            };
        }

        private static T GetRequiredValue<T>(JsonObject value, string propertyName)
        {
            if (value[propertyName] is not JsonValue jsonValue || !jsonValue.TryGetValue<T>(out var result))
                throw new ArgumentException($"RecipeCharacteristic value must contain a valid '{propertyName}' property.");

            return result;
        }

        private static IReadOnlyList<string> GetRequiredArray(JsonObject value, string propertyName)
        {
            if (value[propertyName] is not JsonArray array)
                throw new ArgumentException($"RecipeCharacteristic value must contain a '{propertyName}' array.");

            return array.Select(item => item?.GetValue<string>()
                ?? throw new ArgumentException($"All '{propertyName}' values must be strings.")).ToArray();
        }
    }
}
