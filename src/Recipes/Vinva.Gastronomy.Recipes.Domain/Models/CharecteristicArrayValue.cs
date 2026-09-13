using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Recipes.Domain.Models
{
    public class CharecteristicArrayValue : ICharacteristicValue
    {        
        public required IReadOnlyList<string> Items { get; init; }

        public JsonObject GetJson()
        {
            var jsonObj = new JsonObject();
            var jsonValues = Items.Select(a => JsonValue.Create(a)).ToArray();
            jsonObj[nameof(Items)] = new JsonArray(jsonValues);
            return jsonObj;
        }

        public object Parse(JsonObject obj)
        {
            var jsonArray = (JsonArray)obj[nameof(Items)]!;
            return new CharecteristicArrayValue
            {
                Items = jsonArray.Select(a => a.GetValue<string>()).ToArray()
            };
        }        
    }
}
