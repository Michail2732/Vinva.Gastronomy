using System;
using System.ComponentModel;
using Vinva.Gastronomy.Common.Entities;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    [DisplayName("Категория рецепта")]
    public class RecipeCategory : DescriptiveEntity
    {
        public Guid RecipeId { get; }
#pragma warning disable CS8618
        private RecipeCategory() { }
#pragma warning restore CS8618 

        public RecipeCategory(Guid recipeId, string name, string description) : base(name, description) 
        {
            RecipeId = recipeId;
        }

        public override bool Equals(IEntity? obj)
        {
            return obj is RecipeCategory category &&                   
                   Name == category.Name &&
                   RecipeId.Equals(category.RecipeId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, RecipeId);
        }
    }
}
