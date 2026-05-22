using System;
using System.Collections.Generic;
using System.ComponentModel;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Domain.Models;
using Vinva.Gastronomy.Recipes.Domain.Validations;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    [DisplayName("Ингредиент")]
    public class Ingredient: DescriptiveSoftDeleteEntityOfT<Guid>, IAggregateRoot
    {                                
        public Guid? PhotoId { get; set; }

        public Guid? RecipeId { get; set; }        


#pragma warning disable CS8618
        private Ingredient() { }
#pragma warning restore CS8618 

        public Ingredient(string name, string description) : base(name, description) { }        

        public Ingredient(Guid id, string name, string description) : base(id, name, description) { }                                            
    }
}
