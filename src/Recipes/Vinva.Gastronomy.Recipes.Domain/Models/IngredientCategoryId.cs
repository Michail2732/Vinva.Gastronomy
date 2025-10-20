using System;
using System.Collections.Generic;
using System.Text;

namespace Vinva.Gastronomy.Recipes.Domain.Models
{
    public readonly struct IngredientCategoryId : IEquatable<IngredientCategoryId>
    {
        public readonly Guid IngredientId;
        public readonly string Name;

        public IngredientCategoryId(Guid ingredientId, string name)
        {
            IngredientId = ingredientId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public override bool Equals(object? obj)
        {
            return obj is IngredientCategoryId id && Equals(id);
        }

        public bool Equals(IngredientCategoryId other)
        {
            return IngredientId.Equals(other.IngredientId) &&
                   Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IngredientId, Name);
        }

        public static bool operator ==(IngredientCategoryId left, IngredientCategoryId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IngredientCategoryId left, IngredientCategoryId right)
        {
            return !(left == right);
        }
    }
}
