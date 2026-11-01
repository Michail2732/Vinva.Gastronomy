using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    public class RecipeIngredientsSet : EntityOfT<Guid>
    {
        private readonly List<RecipeIngredient> _ingredients;
        public IReadOnlyList<RecipeIngredient> Ingredients 
        {
             get => _ingredients;            
        }
        public Guid RecipeId { get;  internal set; }
        public string Name { get;  private set; }
        
        public List<Quantity> PortionsQuantities {get; private set;}
        public PortionsCalculationType PortionsCalculateType { get; set; }


        public RecipeIngredientsSet(Guid id, Guid recipeId, string name, IEnumerable<Quantity>? portionQuantities = null,
         IEnumerable<RecipeIngredient>? ingredients = null)
        : base(id)
        {
            RecipeId = recipeId;
            Name = name;
            PortionsQuantities = portionQuantities?.ToList() ?? new List<Quantity>();            
            _ingredients = ingredients?.ToList() ?? new List<RecipeIngredient>();
        }


        public void AddIngredient(RecipeIngredient ingredient)
        {
            ArgumentNullException.ThrowIfNull(ingredient);
            if (_ingredients.Any(a => a.Name == ingredient.Name))
                throw RecipeDomainException.RecipeIngredientAlreadyExists(RecipeId, ingredient.Name);

            ingredient.RecipeId = RecipeId;
            _ingredients.Add(ingredient);
        }

        public void RemoveIngredient(string ingredientName)
        {
            var ingredient = _ingredients.FirstOrDefault(a => a.Name == ingredientName);
            if (ingredient == null)
                throw RecipeDomainException.RecipeIngredientNotExists(RecipeId, ingredientName);
            _ingredients.Remove(ingredient);
        }        
    }
}