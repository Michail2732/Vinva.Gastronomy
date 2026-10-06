using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Recipes.Domain.Models;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    [DisplayName("Свойство")]
    public class RecipeCharacteristic : Entity, IEquatable<RecipeCharacteristic?>
    {        
        public Guid RecipeId { get; internal set; }        
        public string Name { get; }        
        public string? Measure { get; set; }        
        public CharacteristicType Type { get; }        
        public ICharacteristicValue Value { get; }

#pragma warning disable CS8618
        private RecipeCharacteristic() { }
#pragma warning restore CS8618 

        public RecipeCharacteristic(Guid recipeId, string name, CharacteristicType type, ICharacteristicValue value)
            : this(name, type, value)
        {
            RecipeId = recipeId;            
        }

        public RecipeCharacteristic(string name, CharacteristicType type, ICharacteristicValue value)
        {            
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type;
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }        

        public CharecteristicArrayValue GetArrayValue()
        {
            return (CharecteristicArrayValue)Value;
        }

        public CharecteristicStringValue GetStringValue()
        {
            return (CharecteristicStringValue)Value;
        }

        public CharecteristicNumberValue GetNumberValue()
        {
            return (CharecteristicNumberValue)Value;
        }

        public CharecteristicRangeValue GetRangeValue()
        {
            return (CharecteristicRangeValue)Value;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as RecipeCharacteristic);
        }

        public override bool Equals(IEntity? obj)
        {
            return Equals(obj as RecipeCharacteristic);
        }

        public bool Equals(RecipeCharacteristic? other)
        {
            return other is not null &&
                   RecipeId.Equals(other.RecipeId) &&
                   Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RecipeId, Name);
        }

        public static bool operator ==(RecipeCharacteristic? left, RecipeCharacteristic? right)
        {
            return EqualityComparer<RecipeCharacteristic>.Default.Equals(left, right);
        }

        public static bool operator !=(RecipeCharacteristic? left, RecipeCharacteristic? right)
        {
            return !(left == right);
        }
    }    


}
