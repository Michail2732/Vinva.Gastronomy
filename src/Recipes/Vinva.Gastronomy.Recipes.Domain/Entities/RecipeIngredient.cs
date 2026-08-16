using System;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Recipes.Domain.Models;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    public class RecipeIngredient : Entity
    {
        public string Name { get; private set; }
        public Guid RecipeId { get; private set; }
        public Guid IngredientId { get; private set; }
        public IngredientQuantities Quantities { get; private set; }
        public bool IsRequired { get; private set; }        


#pragma warning disable CS8618 
        private RecipeIngredient() { }
#pragma warning restore CS8618 

        public RecipeIngredient(Guid recipeId, Guid ingredientId, string ingredientName, IngredientQuantities quantities, bool isRequired = true)
        {
            ArgumentException.ThrowIfNullOrEmpty(ingredientName);
            Name = ingredientName;
            RecipeId = recipeId;
            IngredientId = ingredientId;
            if (quantities.IsEmpty)
                throw new ArgumentException(nameof(quantities));
            Quantities = quantities;
            IsRequired = isRequired;            
        }


        public override bool Equals(object? obj)
        {
            return Equals(obj as RecipeIngredient);
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
