using MediatR;
using Vinva.Gastronomy.Recipes.Application.Usecases.Common;
using Vinva.Gastronomy.Recipes.Persistence;
using Vinva.Gastronomy.Recipes.Persistence.Mappings;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Create
{
    public sealed class CreateRecipeHandler : IRequestHandler<CreateRecipeCommand, CreateRecipeResponce>
    {
        private readonly RecipeDbContext dbContext;

        public CreateRecipeHandler(RecipeDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<CreateRecipeResponce> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
        {
            await new CreateRecipeCommandValidator().ValidateAndThrowAsync(request, cancellationToken);

            var recipe = new Domain.Entities.Recipe(
                request.Name,
                request.Description,
                request.Document,
                request.BaseRecipe);

            foreach (var dto in request.Characteristics)
                recipe.AddCharacteristic(CharacteristicDtoMapper.ToDomain(dto, recipe.Id));

            dbContext.Recipes.Add(recipe);
            dbContext.CharacteristicRecords.AddRange(
                recipe.Characteristics.Select(CharacteristicRecordMapper.ToRecord));
            await dbContext.SaveChangesAsync(cancellationToken);

            return new CreateRecipeResponce
            {
                RecipeId = recipe.Id,
                Name = recipe.Name
            };
        }
    }
}
