using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Recipes.Domain.Models
{
    public class CharecteristicRangeValue : ICharacteristicValue
    {
        public required double Min { get; init; }
        public required double Max { get; init; }

        public JsonObject GetJson()
        {
            var jsonObj = new JsonObject();

            var minJsonValue = JsonValue.Create(Min);
            jsonObj[nameof(Min)] = minJsonValue;

            var maxJsonValue = JsonValue.Create(Max);
            jsonObj[nameof(Max)] = maxJsonValue;

            return jsonObj;
        }

        public object Parse(JsonObject obj)
        {
            var minJsonValue = (JsonValue)obj[nameof(Min)]!;
            var maxJsonValue = (JsonValue)obj[nameof(Max)]!;
            return new CharecteristicRangeValue
            {
                Min = minJsonValue.GetValue<double>(),
                Max = maxJsonValue.GetValue<double>(),
            };
        }


    }
}
