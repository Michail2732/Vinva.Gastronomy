using System;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Recipes.Domain.Models;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    public class RecipeIngredient : DescriptiveEntity
    {
        public Guid RecipeId { get; private set; }
        public Guid IngredientId { get; private set; }
        public IngredientQuantities Quantities { get; private set; }
        public bool IsRequired { get; private set; }


#pragma warning disable CS8618 
        private RecipeIngredient() { }
#pragma warning restore CS8618 

        public RecipeIngredient(Guid recipeId, Guid ingredientId, string ingredientName, string description, string quantities, bool isRequired) 
            : base(ingredientName, description)
        {
            RecipeId = recipeId;
            IngredientId = ingredientId;
            Quantities = quantities ?? throw new ArgumentNullException(nameof(quantities));
            IsRequired = isRequired;
        }

        public override bool Equals(IEntity? obj)
        {
            return obj is RecipeIngredient ingredient &&
                   RecipeId.Equals(ingredient.RecipeId) &&
                   IngredientId.Equals(ingredient.IngredientId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RecipeId, IngredientId);
        }
    }
}
