using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;

namespace Vinva.Gastronomy.Recipes.Domain.Models
{
    public sealed class RecipeProperties: IEnumerable<RecipeProperty>
    {       
        private readonly List<RecipeProperty> _properties;

        public int Count => _properties.Count;

        private RecipeProperties()
        {

        }
        public RecipeProperties(IEnumerable<RecipeProperty> properties)
        {            
            _properties = properties?.ToDictionary(a => a.Name).Values.ToList() ?? throw new ArgumentNullException(nameof(properties));
        }

        public RecipeProperty? this[string propertyName] 
        {
            get => _properties.Find(a => a.Name == propertyName);
        }

        public void Add(string name, IEnumerable<string>? values = null)
        {
            if (this[name] == null)
                throw new RecipeDomainException(GetType(), $"Property with name '{name}' already exits");
            _properties.Add(new RecipeProperty(name, values ?? Array.Empty<string>()));
        }

        public bool Remove(string name)
        {
            var property = this[name];
            if (property == null)
                return false;            
            return _properties.Remove(property);
        }


        public static RecipeProperties CreatEmpty() => new RecipeProperties(Array.Empty<RecipeProperty>());

        public IEnumerator<RecipeProperty> GetEnumerator() => _properties.GetEnumerator();        

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        
    }
}
