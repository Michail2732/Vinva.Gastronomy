using System;
using System.Collections.Generic;
using System.ComponentModel;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Domain.Models;
using Vinva.Gastronomy.Recipes.Domain.Validations;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    [DisplayName("Ингредиент")]
    public class Ingredient: EntityOfT<Guid>, IAggregateRoot
    {
        public string Name { get; }
        public string Description { get; }
        public Guid? PhotoId { get; set; }

        public Guid? RecipeId { get; set; }        


#pragma warning disable CS8618
        private Ingredient() : base(GetDefaultGuid()) { }
#pragma warning restore CS8618 

        public Ingredient(string name, string description) : this(GenerateGuid(), name, description)
        {

        }        

        public Ingredient(Guid id, string name, string description) : base(id) 
        {
            ArgumentException.ThrowIfNullOrEmpty(name);
            ArgumentException.ThrowIfNullOrEmpty(description);
            Name = name;
            Description = description;            
        }
    }
}
