using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.Create
{
    public readonly record struct CreateRecipeCommand: IRequest<CreateRecipeResponce>
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public string? Comment { get; init; }
        public TimeSpan CookingTime { get; init; }
        public Guid? BaseRecipe { get; init; }
        public string? StorageComment { get; init; }
        public string? UsageComment { get; init; }
    }
}
