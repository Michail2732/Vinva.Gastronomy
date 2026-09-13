using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Recipes.Domain.Models
{
    public class CharecteristicNumberValue : ICharacteristicValue
    {
        public required double Data { get; init; }

        public JsonObject GetJson()
        {
            var jsonObj = new JsonObject();
            var jsonValue = JsonValue.Create(Data);
            jsonObj[nameof(Data)] = jsonValue;
            return jsonObj;
        }

        public object Parse(JsonObject obj)
        {
            var jsonValue = (JsonValue)obj[nameof(Data)]!;
            return new CharecteristicNumberValue
            {
                Data = jsonValue.GetValue<double>()
            };
        }


    }
}
