using System;
using System.Linq;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Domain.Models;
using Vinva.Gastronomy.Recipes.Persistence.Models;

namespace Vinva.Gastronomy.Recipes.Persistence.Mappings
{
    public static class CharacteristicRecordMapper
    {
        public static CharacteristicRecord ToRecord(RecipeCharacteristic characteristic)
        {
            var record = new CharacteristicRecord
            {
                RecipeId = characteristic.RecipeId,
                Name = characteristic.Name,
                Measure = characteristic.Measure,
                Type = characteristic.Type
            };

            switch (characteristic.Value)
            {
                case CharecteristicStringValue value:
                    record.StringValue = value.Data;
                    break;
                case CharecteristicNumberValue value:
                    record.NumberValue = value.Data;
                    break;
                case CharecteristicRangeValue value:
                    record.RangeMin = value.Min;
                    record.RangeMax = value.Max;
                    break;
                case CharecteristicArrayValue value:
                    record.ArrayItems = value.Items.ToArray();
                    break;
                default:
                    throw new ArgumentException($"Unsupported characteristic value: {characteristic.Value.GetType().Name}", nameof(characteristic));
            }

            return record;
        }

        public static RecipeCharacteristic ToDomain(CharacteristicRecord record)
        {
            ICharacteristicValue value = record.Type switch
            {
                CharacteristicType.String when record.StringValue is not null =>
                    new CharecteristicStringValue { Data = record.StringValue },
                CharacteristicType.Number when record.NumberValue.HasValue =>
                    new CharecteristicNumberValue { Data = record.NumberValue.Value },
                CharacteristicType.Range when record.RangeMin.HasValue && record.RangeMax.HasValue =>
                    new CharecteristicRangeValue
                    {
                        Min = record.RangeMin.Value,
                        Max = record.RangeMax.Value
                    },
                CharacteristicType.Array when record.ArrayItems is not null =>
                    new CharecteristicArrayValue { Items = record.ArrayItems },
                _ => throw new InvalidOperationException($"RecipeCharacteristic record '{record.Name}' has invalid value for type '{record.Type}'.")
            };

            return new RecipeCharacteristic(record.RecipeId, record.Name, record.Type, value)
            {
                Measure = record.Measure
            };
        }
    }
}