using System;
using System.ComponentModel;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Common.Entities;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    [DisplayName("Шаг приготовления рецепта")]
    public class RecipeStep :  Entity
    {
        public string Description { get; private set; }
        public string? Comment { get; set; }
        public Guid RecipeId { get; private set; }
        public int SeqNumber { get; internal set; }
        public Guid? PhotoId { get; private set; }


#pragma warning disable CS8618
        private RecipeStep() { }
#pragma warning restore CS8618 

        public RecipeStep(Guid recipeId, string description, Guid? photoId = null)            
        {
            ArgumentException.ThrowIfNullOrEmpty(description);
            RecipeId = recipeId;            
            PhotoId = photoId;  
            Description = description;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as RecipeStep);
        }

        public override bool Equals(IEntity? obj)
        {
            return obj is RecipeStep step &&                   
                   RecipeId.Equals(step.RecipeId) &&
                   SeqNumber == step.SeqNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RecipeId, SeqNumber);
        }        
    }
}
