using System;
using System.ComponentModel;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Common.Entities;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    [DisplayName("Шаг приготовления рецепта")]
    public class RecipeStep :  EntityOfT<Guid>
    {
        public string Description { get; private set; }
        public string? Comment { get; set; }
        public Guid RecipeId { get; internal set; }
        public int SeqNumber { get; internal set; } = 1;
        public Guid? PhotoId { get; set; }


#pragma warning disable CS8618
        private RecipeStep() { }
#pragma warning restore CS8618 

        public RecipeStep(Guid id, Guid recipeId, string description, int seqNumber, Guid? photoId = null) : this(recipeId, description, seqNumber, photoId)
        {
            Id = id;
        }

        public RecipeStep(Guid recipeId, string description, int seqNumber, Guid? photoId = null) : this(description, photoId)
        {            
            RecipeId = recipeId;
            SeqNumber = seqNumber;
        }

        public RecipeStep(string description, Guid? photoId = null) : base()
        {
            ArgumentException.ThrowIfNullOrEmpty(description);            
            PhotoId = photoId;
            Description = description;
        }       
    }
}
