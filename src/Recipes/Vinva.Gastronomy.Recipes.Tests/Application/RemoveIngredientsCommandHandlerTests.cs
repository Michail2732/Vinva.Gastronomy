using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Exceptions;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.RemoveIngredients;

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

            await handler.Handle(command, CancellationToken.None);            

            var recipeAfter = await DbContext.Recipes.Include(r => r.Ingredients).FirstAsync(r => r.Id == RecipeWithIngredientsId);
            Assert.That(recipeAfter.Ingredients.Count, Is.EqualTo(countBefore - 1));
            Assert.That(recipeAfter.Ingredients.Any(i => i.IngredientId == IngredientInRecipeId), Is.False);
        }

        [Test]
        public void Handle_WithNonExistentRecipeId_ShouldThrowBadRequestException()
        {            
            var command = new RemoveIngredientsCommand
            {
                RecipeId = Guid.NewGuid(),
                IngredientIds = [IngredientInRecipeId]
            };

            Assert.ThrowsAsync<NotFoundException>(async () =>
                await Mediator.Send(command, CancellationToken.None));
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

            await handler.Handle(command, CancellationToken.None);
            
            var recipeAfter = await DbContext.Recipes.Include(r => r.Ingredients).FirstAsync(r => r.Id == RecipeWithIngredientsId);
            Assert.That(recipeAfter.Ingredients.Count, Is.EqualTo(countBefore - 2));
            Assert.That(recipeAfter.Ingredients.Any(i => i.IngredientId == flourId), Is.False);
            Assert.That(recipeAfter.Ingredients.Any(i => i.IngredientId == sugarId), Is.False);
        }

        [Test]
        public async Task Handle_WithEmptyIngredientIds_ShouldReturnValidationFailure()
        {            
            var command = new RemoveIngredientsCommand
            {
                RecipeId = RecipeWithIngredientsId,
                IngredientIds = []
            };

            Assert.ThrowsAsync<BadRequestException>(() => Mediator.Send(command, CancellationToken.None));            
        }
    }
}
