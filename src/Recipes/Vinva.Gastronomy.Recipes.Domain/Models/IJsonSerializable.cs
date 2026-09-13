using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Recipes.Domain.Models
{
    public interface IJsonSerializable
    {
        public abstract JsonObject GetJson();
        public abstract object Parse(JsonObject obj);        
    }
}
