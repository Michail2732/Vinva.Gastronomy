using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Domain.Models;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    [DisplayName("Рецепт")]
    public class Recipe : EntityOfT<Guid>, IAggregateRoot
    {                                
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Guid? BaseRecipe { get; set; }                        
        public string Document { get; private set; }
        public Guid? TitleImageId {get; set;}
        public Guid[]? GalleryImageIds {get; set;}
        private readonly List<RecipeCharacteristic> _characteristics = new();
        public IReadOnlyList<RecipeCharacteristic> Characteristics
        {
            get => _characteristics;
        }

        private readonly List<RecipeIngredientsSet> _ingredientsSets = new();
        public IReadOnlyCollection<RecipeIngredientsSet> IngredientsSets => _ingredientsSets;


#pragma warning disable CS8618
        private Recipe() : base(GetDefaultGuid()) { }
#pragma warning restore CS8618 

        public Recipe(string name, string description, string document, Guid? baseRecipe = null) 
        : this(GenerateGuid(), name, description, document, baseRecipe)
        {
            
        }

        public Recipe(Guid id, string name, string description, string document, Guid? baseRecipe = null) : base(id)
        {
            ArgumentException.ThrowIfNullOrEmpty(name);
            ArgumentException.ThrowIfNullOrEmpty(description);
            Name = name;
            Description = description;
            BaseRecipe = baseRecipe;            
            Document = document;
        }                
        
                
        public void AddCharacteristic(RecipeCharacteristic characteristic)
        {            
            if (_characteristics.Contains(characteristic))
                throw RecipeDomainException.RecipeCharacteristicAlreadyExists(Id, characteristic.Name);

            characteristic.RecipeId = Id;
            _characteristics.Add(characteristic);
        }

        public void RemoveCharacteristic(string characteristicName)
        {            
            var characteristic = _characteristics.FirstOrDefault(a => a.Name == characteristicName);
            if (characteristic == null)
                throw RecipeDomainException.RecipeCharacteristicNotExists(Id, characteristicName);
            _characteristics.Remove(characteristic);
        }

        public void AddIngredientsSet(RecipeIngredientsSet ingredientSet)
        {            
            ArgumentNullException.ThrowIfNull(ingredientSet);                        
            if (_ingredientsSets.Any(a => a.Name == ingredientSet.Name))
                throw RecipeDomainException.RecipeIngredientAlreadyExists(Id, ingredientSet.Name);

            ingredientSet.RecipeId = Id;
            _ingredientsSets.Add(ingredientSet);
        }

        public void RemoveIngredientsSet(string ingredientName)
        {
            var ingredientSet = _ingredientsSets.FirstOrDefault(a => a.Name == ingredientName);
            if (ingredientSet == null)
                throw RecipeDomainException.RecipeIngredientNotExists(Id, ingredientName);
            _ingredientsSets.Remove(ingredientSet);
        }

        public void ReplaceIngredientsSets(IEnumerable<RecipeIngredientsSet> ingredients)
        {
            _ingredientsSets.Clear();
            foreach (var ingredient in ingredients)
                AddIngredientsSet(ingredient);
        }

        public void UpdateDetails(string name, string description, string document, Guid? baseRecipe)
        {
            ArgumentException.ThrowIfNullOrEmpty(name);
            ArgumentException.ThrowIfNullOrEmpty(description);
            Name = name;
            Description = description;
            Document = document;
            BaseRecipe = baseRecipe;
        }
    }
}
