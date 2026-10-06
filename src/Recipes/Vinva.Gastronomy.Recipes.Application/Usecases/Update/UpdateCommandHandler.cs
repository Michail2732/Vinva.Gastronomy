using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Models;
using Vinva.Gastronomy.Recipes.Application.Common;
using Vinva.Gastronomy.Recipes.Application.Usecases.Common;
using Vinva.Gastronomy.Recipes.Persistence;
using Vinva.Gastronomy.Recipes.Persistence.Mappings;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Update
{
    public sealed class UpdateCommandHandler : IRequestHandler<UpdateCommand>
    {
        private readonly RecipeDbContext dbContext;

        public UpdateCommandHandler(RecipeDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            await new UpdateCommandValidator().ValidateAndThrowAsync(request, cancellationToken);

            var recipe = await dbContext.Recipes
                .SingleOrDefaultAsync(item => item.Id == request.Id, cancellationToken)
                ?? throw new KeyNotFoundException($"Recipe '{request.Id}' was not found.");

            var records = await dbContext.CharacteristicRecords
                .Where(record => record.RecipeId == recipe.Id)
                .ToListAsync(cancellationToken);
            foreach (var record in records)
                recipe.AddCharacteristic(CharacteristicRecordMapper.ToDomain(record));

            var ingredientRecords = await dbContext.RecipeIngredientRecords
                .Where(record => record.RecipeId == recipe.Id)
                .ToListAsync(cancellationToken);
            recipe.ReplaceIngredients(ingredientRecords.Select(RecipeIngredientRecordMapper.ToDomain));

            recipe.UpdateDetails(
                request.Name ?? recipe.Name,
                request.Description ?? recipe.Description,
                request.Document ?? recipe.Document,
                request.BaseRecipe ?? recipe.BaseRecipe);

            if (request.Characteristics is not null)
                ApplyCharacteristics(recipe, records, request.Characteristics);

            if (request.Ingredients is not null)
            {
                dbContext.RecipeIngredientRecords.RemoveRange(ingredientRecords);
                var ingredients = request.Ingredients.Select(dto => RecipeIngredientDtoMapper.ToDomain(dto, recipe.Id)).ToArray();
                recipe.ReplaceIngredients(ingredients);
                dbContext.RecipeIngredientRecords.AddRange(ingredients.Select(RecipeIngredientRecordMapper.ToRecord));
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        private void ApplyCharacteristics(
            Domain.Entities.Recipe recipe,
            List<Persistence.Models.CharacteristicRecord> records,
            IEnumerable<CharacteristicDto> dtos)
        {
            foreach (var dto in dtos)
            {
                if (dto.State == DtoState.None)
                    continue;

                var existingRecord = records.SingleOrDefault(record => record.Name == dto.Name);
                var existingCharacteristic = recipe.Characteristics.SingleOrDefault(item => item.Name == dto.Name);

                if (dto.State == DtoState.New)
                {
                    if (existingRecord is not null)
                        throw new InvalidOperationException($"RecipeCharacteristic '{dto.Name}' already exists.");

                    var characteristic = CharacteristicDtoMapper.ToDomain(dto, recipe.Id);
                    recipe.AddCharacteristic(characteristic);
                    dbContext.CharacteristicRecords.Add(CharacteristicRecordMapper.ToRecord(characteristic));
                    records.Add(CharacteristicRecordMapper.ToRecord(characteristic));
                    continue;
                }

                if (existingRecord is null || existingCharacteristic is null)
                    throw new KeyNotFoundException($"RecipeCharacteristic '{dto.Name}' was not found.");

                if (dto.State == DtoState.Remove)
                {
                    recipe.RemoveCharacteristic(dto.Name);
                    dbContext.CharacteristicRecords.Remove(existingRecord);
                    records.Remove(existingRecord);
                    continue;
                }

                if (dto.State == DtoState.Change)
                {
                    recipe.RemoveCharacteristic(dto.Name);
                    var characteristic = CharacteristicDtoMapper.ToDomain(dto, recipe.Id);
                    recipe.AddCharacteristic(characteristic);

                    var replacement = CharacteristicRecordMapper.ToRecord(characteristic);
                    existingRecord.Measure = replacement.Measure;
                    existingRecord.Type = replacement.Type;
                    existingRecord.StringValue = replacement.StringValue;
                    existingRecord.NumberValue = replacement.NumberValue;
                    existingRecord.RangeMin = replacement.RangeMin;
                    existingRecord.RangeMax = replacement.RangeMax;
                    existingRecord.ArrayItems = replacement.ArrayItems;
                }
            }
        }
    }
}
