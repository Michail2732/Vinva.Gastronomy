using System;
using System.Collections.Generic;
using System.Text;

namespace Vinva.Gastronomy.Recipes.Domain.Models
{
    public readonly struct RecipeIngredientId : IEquatable<RecipeIngredientId>
    {
        public readonly Guid RecipeId;
        public readonly Guid IngredientId;

        public RecipeIngredientId(Guid recipeId, Guid ingredientId)
        {
            RecipeId = recipeId;
            IngredientId = ingredientId;
        }

        public override bool Equals(object? obj)
        {
            return obj is RecipeIngredientId id && Equals(id);
        }

        public bool Equals(RecipeIngredientId other)
        {
            return RecipeId.Equals(other.RecipeId) &&
                   IngredientId.Equals(other.IngredientId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RecipeId, IngredientId);
        }

        public static bool operator ==(RecipeIngredientId left, RecipeIngredientId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RecipeIngredientId left, RecipeIngredientId right)
        {
            return !(left == right);
        }
    }
}
