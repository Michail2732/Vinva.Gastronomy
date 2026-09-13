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
        private readonly Dictionary<string, Characteristic> _characteristics = new();        

        public string Name { get; private set; }
        public string Description { get; private set; }
        public Guid? BaseRecipe { get; set; }                        
        public string Document { get; private set; }
        public Guid? TitleImageId {get; set;}
        public Guid[]? GalleryImageIds {get; set;}
        public IReadOnlyCollection<Characteristic> Characteristics
        {
            get => _characteristics.Values;
        }


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
        
                
        public void AddCharacteristic(Characteristic characteristic)
        {
            if (_characteristics.ContainsKey(characteristic.Name))
                throw RecipeDomainException.RecipeCharacteristicAlreadyExists(Id, characteristic.Name);

            characteristic.RecipeId = Id;
            _characteristics[characteristic.Name] = characteristic;
        }

        public void RemoveCharacteristic(string characteristicName)
        {            
            if (!_characteristics.Remove(characteristicName))
                throw RecipeDomainException.RecipeCharacteristicNotExists(Id, characteristicName);
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
