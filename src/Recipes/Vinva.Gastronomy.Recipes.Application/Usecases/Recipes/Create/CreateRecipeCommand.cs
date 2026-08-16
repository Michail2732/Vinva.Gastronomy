using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;


namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.Create
{
    public readonly record struct CreateRecipeCommand: IRequest<CreateRecipeResponce>
    {
        public string Name { get; init; }
        public string Description { get; init; }        
        public TimeSpan CookingTime { get; init; }
        public Guid? BaseRecipe { get; init; }
        public string Document { get; init; }
        public Guid? TitleImageId { get; init; }
        public List<Guid> OtherImageIds { get; init; }
    }
}
