using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Recipes.Domain.ValueObjects;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    [DisplayName("Шаг приготовления рецепта")]
    public class RecipeStep : Entity<RecipeStepId>
    {
        public Guid? PhotoId { get; private set; }
        public string Description { get; private set; }
        public string Comment { get; private set; }

        public RecipeStep(RecipeStepId id, string description, string comment, Guid? photoId = null)
        {
            Id = id;
            PhotoId = photoId;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Comment = comment ?? throw new ArgumentNullException(nameof(comment));
        }
    }
}
