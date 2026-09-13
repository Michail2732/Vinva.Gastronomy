using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Common.Entities;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    public class CharacteristicDefinition : Entity, IEquatable<CharacteristicDefinition?>
    {        
        public string Name { get; }
        public CharacteristicType Type { get; }        
        public string? Measure { get; set; }        


        public CharacteristicDefinition(string name, CharacteristicType type)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type;
        }


        public override bool Equals(IEntity? other)
        {
            return Equals(other as CharacteristicDefinition);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as CharacteristicDefinition);
        }

        public bool Equals(CharacteristicDefinition? other)
        {
            return other is not null &&
                   Name == other.Name &&
                   Type == other.Type;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Type);
        }

        public static bool operator ==(CharacteristicDefinition? left, CharacteristicDefinition? right)
        {
            return EqualityComparer<CharacteristicDefinition>.Default.Equals(left, right);
        }

        public static bool operator !=(CharacteristicDefinition? left, CharacteristicDefinition? right)
        {
            return !(left == right);
        }
    }
}
