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

            var record = await dbContext.Recipes
                .Include(item => item.Characteristics)
                .Include(item => item.IngredientsSets)
                    .ThenInclude(item => item.Ingredients)
                .AsSplitQuery()
                .SingleOrDefaultAsync(item => item.Id == request.Id, cancellationToken)
                ?? throw new KeyNotFoundException($"Recipe '{request.Id}' was not found.");

            var recipe = RecipeRecordMapper.ToDomain(record);

            recipe.UpdateDetails(
                request.Name ?? recipe.Name,
                request.Description ?? recipe.Description,
                request.Document ?? recipe.Document,
                request.BaseRecipe ?? recipe.BaseRecipe);
            RecipeRecordMapper.ApplyScalars(recipe, record);

            if (request.Characteristics is not null)
                ApplyCharacteristics(recipe, record.Characteristics, request.Characteristics);

            if (request.IngredientsSets is null)
            {
                await dbContext.SaveChangesAsync(cancellationToken);
                return;
            }

            // Старые наборы удаляются отдельным сохранением, чтобы переданные Id и уникальные имена не конфликтовали с новыми записями.
            var newSets = request.IngredientsSets
                .Select(dto => RecipeIngredientDtoMapper.ToDomain(dto, recipe.Id))
                .ToArray();
            recipe.ReplaceIngredientsSets(newSets);

            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            dbContext.RecipeIngredientsSetRecords.RemoveRange(record.IngredientsSets);
            await dbContext.SaveChangesAsync(cancellationToken);

            dbContext.RecipeIngredientsSetRecords.AddRange(
                recipe.IngredientsSets.Select(RecipeIngredientsSetRecordMapper.ToRecord));
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
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
                    var newRecord = CharacteristicRecordMapper.ToRecord(characteristic);
                    dbContext.CharacteristicRecords.Add(newRecord);
                    records.Add(newRecord);
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
