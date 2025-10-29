using System;
using System.ComponentModel;
using Vinva.Gastronomy.Common.Entities;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    [DisplayName("Шаг приготовления рецепта")]
    public class RecipeStep : DescriptiveEntity
    {
        public Guid RecipeId { get; private set; }
        public int SeqNumber { get; private set; }
        public Guid? PhotoId { get; private set; }


#pragma warning disable CS8618
        private RecipeStep() { }
#pragma warning restore CS8618 

        public RecipeStep(Guid recipeId, int seqNumber, string name, string description, Guid? photoId = null) 
            : base(name, description)
        {
            RecipeId = recipeId;
            SeqNumber = seqNumber;
            PhotoId = photoId;            
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
