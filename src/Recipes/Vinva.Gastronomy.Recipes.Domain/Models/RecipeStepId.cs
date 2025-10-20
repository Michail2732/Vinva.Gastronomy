using System;
using System.Collections.Generic;
using System.Text;

namespace Vinva.Gastronomy.Recipes.Domain.ValueObjects
{
    public readonly struct RecipeStepId : IEquatable<RecipeStepId>
    {
        public readonly Guid RecipeId;
        public readonly int SeqNumber;

        public RecipeStepId(Guid recipeId, int seqNumber)
        {
            RecipeId = recipeId;
            SeqNumber = seqNumber;
        }

        public override bool Equals(object? obj)
        {
            return obj is RecipeStepId id && Equals(id);
        }

        public bool Equals(RecipeStepId other)
        {
            return RecipeId.Equals(other.RecipeId) &&
                   SeqNumber == other.SeqNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RecipeId, SeqNumber);
        }

        public static bool operator ==(RecipeStepId left, RecipeStepId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RecipeStepId left, RecipeStepId right)
        {
            return !(left == right);
        }
    }
}
