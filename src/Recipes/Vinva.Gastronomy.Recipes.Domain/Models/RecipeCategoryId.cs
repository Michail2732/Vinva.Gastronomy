using System;
using System.Collections.Generic;
using System.Text;

namespace Vinva.Gastronomy.Recipes.Domain.ValueObjects
{
    public readonly struct RecipeCategoryId : IEquatable<RecipeCategoryId>
    {
        public readonly Guid RecipeId;
        public readonly ReadOnlyMemory<char> Name;
        public RecipeCategoryId(Guid recipeId, string name)
        {
            RecipeId = recipeId;
            Name = name?.AsMemory() ?? throw new ArgumentNullException(nameof(name));
        }

        public override bool Equals(object? obj)
        {
            return obj is RecipeCategoryId id && Equals(id);
        }

        public bool Equals(RecipeCategoryId other)
        {
            return RecipeId.Equals(other.RecipeId) &&
                   Name.Equals(other.Name);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RecipeId, Name);
        }

        public static bool operator ==(RecipeCategoryId left, RecipeCategoryId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RecipeCategoryId left, RecipeCategoryId right)
        {
            return !(left == right);
        }
    }
}
