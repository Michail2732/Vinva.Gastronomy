using System;
using System.ComponentModel;
using Vinva.Gastronomy.Common.Entities;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    [DisplayName("Категория ингредиента")]
    public class IngredientCategory: DescriptiveEntity
    {
        public Guid IngredientId { get; private set; }
#pragma warning disable CS8618
        private IngredientCategory() { }
#pragma warning restore CS8618 

        public IngredientCategory(Guid ingredientId, string name, string description) : base(name, description) 
        {
            IngredientId = ingredientId;
        }

        public override bool Equals(IEntity? obj)
        {
            return obj is IngredientCategory category &&                   
                   Name == category.Name &&
                   IngredientId.Equals(category.IngredientId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, IngredientId);
        }
    }
}
