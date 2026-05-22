using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Recipes.Domain.Models
{
    public sealed class RecipeProperty : IEquatable<RecipeProperty?>
    {
        public string Name { get; init; }

        public List<string> Values { get; init; }

        public bool IsSingleValue => Values.Count == 1;

        public bool IsEmpty => Values.Count == 0;


        public RecipeProperty(string name, IEnumerable<string> values)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Values = new List<string>(values) ?? throw new ArgumentNullException(nameof(values));
        }
                

        public override bool Equals(object? obj)
        {
            return Equals(obj as RecipeProperty);
        }

        public bool Equals(RecipeProperty? other)
        {
            return other is not null &&
                   Name == other.Name &&
                   EqualityComparer<IReadOnlyList<string>>.Default.Equals(Values, other.Values);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Values);
        }        
    }
}
