using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Recipes.Application.RecipeUsecases.RemoveIngredients;

namespace Vinva.Gastronomy.Recipes.Tests.Application
{
    [TestFixture]
    public class RemoveIngredientsCommandHandlerTests : BaseApplicationTests
    {
        private static readonly Guid RecipeWithIngredientsId = Guid.Parse("33333333-3333-3333-3333-333333333331");
        private static readonly Guid IngredientInRecipeId = Guid.Parse("44444444-4444-4444-4444-444444444450"); // Oil in Bliny

        [Test]
        public async Task Handle_WithValidIngredientIds_ShouldRemoveIngredients()
        {
            var handler = new RemoveIngredientsCommandHandler(DbContext);
            var recipeBefore = await DbContext.Recipes.Include(r => r.Ingredients).FirstAsync(r => r.Id == RecipeWithIngredientsId);
            var countBefore = recipeBefore.Ingredients.Count;
            Assert.That(recipeBefore.Ingredients.Any(i => i.IngredientId == IngredientInRecipeId), Is.True);

            var command = new RemoveIngredientsCommand
            {
                RecipeId = RecipeWithIngredientsId,
                IngredientIds = [IngredientInRecipeId]
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);

            var recipeAfter = await DbContext.Recipes.Include(r => r.Ingredients).FirstAsync(r => r.Id == RecipeWithIngredientsId);
            Assert.That(recipeAfter.Ingredients.Count, Is.EqualTo(countBefore - 1));
            Assert.That(recipeAfter.Ingredients.Any(i => i.IngredientId == IngredientInRecipeId), Is.False);
        }

        [Test]
        public void Handle_WithNonExistentRecipeId_ShouldThrowBadRequestException()
        {
            var handler = new RemoveIngredientsCommandHandler(DbContext);
            var command = new RemoveIngredientsCommand
            {
                RecipeId = Guid.NewGuid(),
                IngredientIds = [IngredientInRecipeId]
            };

            Assert.ThrowsAsync<BadRequestException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task Handle_WithMultipleIngredientIds_ShouldRemoveAll()
        {
            var handler = new RemoveIngredientsCommandHandler(DbContext);
            var flourId = Guid.Parse("44444444-4444-4444-4444-444444444441");
            var sugarId = Guid.Parse("44444444-4444-4444-4444-444444444442");
            var recipeBefore = await DbContext.Recipes.Include(r => r.Ingredients).FirstAsync(r => r.Id == RecipeWithIngredientsId);
            var countBefore = recipeBefore.Ingredients.Count;
            Assert.That(recipeBefore.Ingredients.Any(i => i.IngredientId == flourId), Is.True);
            Assert.That(recipeBefore.Ingredients.Any(i => i.IngredientId == sugarId), Is.True);

            var command = new RemoveIngredientsCommand
            {
                RecipeId = RecipeWithIngredientsId,
                IngredientIds = [flourId, sugarId]
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            var recipeAfter = await DbContext.Recipes.Include(r => r.Ingredients).FirstAsync(r => r.Id == RecipeWithIngredientsId);
            Assert.That(recipeAfter.Ingredients.Count, Is.EqualTo(countBefore - 2));
            Assert.That(recipeAfter.Ingredients.Any(i => i.IngredientId == flourId), Is.False);
            Assert.That(recipeAfter.Ingredients.Any(i => i.IngredientId == sugarId), Is.False);
        }

        [Test]
        public async Task Handle_WithEmptyIngredientIds_ShouldReturnValidationFailure()
        {
            var handler = new RemoveIngredientsCommandHandler(DbContext);
            var command = new RemoveIngredientsCommand
            {
                RecipeId = RecipeWithIngredientsId,
                IngredientIds = []
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
        }
    }
}
