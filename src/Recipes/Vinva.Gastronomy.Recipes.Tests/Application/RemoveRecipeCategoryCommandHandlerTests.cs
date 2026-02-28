using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.RemoveCategory;

namespace Vinva.Gastronomy.Recipes.Tests.Application
{
    [TestFixture]
    public class RemoveRecipeCategoryCommandHandlerTests : BaseApplicationTests
    {
        private static readonly Guid RecipeId = Guid.Parse("33333333-3333-3333-3333-333333333331"); // Bliny - has Desserts and Breakfasts
        private static readonly Guid BreakfastsCategoryId = Guid.Parse("11111111-1111-1111-1111-111111111113");

        [Test]
        public async Task Handle_WithValidCategoryIds_ShouldRemoveCategoriesFromRecipe()
        {
            var handler = new RemoveRecipeCategoryCommandHandler(DbContext);
            var recipeBefore = await DbContext.Recipes.Include(r => r.Categories).FirstAsync(r => r.Id == RecipeId);
            Assert.That(recipeBefore.Categories.Any(c => c.Id == BreakfastsCategoryId), Is.True);
            var countBefore = recipeBefore.Categories.Count;

            var command = new RemoveRecipeCategoryCommand
            {
                RecipeId = RecipeId,
                CategoryIds = [BreakfastsCategoryId]
            };

            await handler.Handle(command, CancellationToken.None);            

            var recipeAfter = await DbContext.Recipes.Include(r => r.Categories).FirstAsync(r => r.Id == RecipeId);
            Assert.That(recipeAfter.Categories.Count, Is.EqualTo(countBefore - 1));
            Assert.That(recipeAfter.Categories.Any(c => c.Id == BreakfastsCategoryId), Is.False);
        }

        [Test]
        public void Handle_WithNonExistentRecipeId_ShouldThrowBadRequestException()
        {
            var handler = new RemoveRecipeCategoryCommandHandler(DbContext);
            var command = new RemoveRecipeCategoryCommand
            {
                RecipeId = Guid.NewGuid(),
                CategoryIds = [BreakfastsCategoryId]
            };

            Assert.ThrowsAsync<BadRequestException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task Handle_WithEmptyCategoryIds_ShouldReturnValidationFailure()
        {
            var handler = new RemoveRecipeCategoryCommandHandler(DbContext);
            var command = new RemoveRecipeCategoryCommand
            {
                RecipeId = RecipeId,
                CategoryIds = []
            };

            Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(command, CancellationToken.None));            
        }

        [Test]
        public async Task Handle_WithMultipleCategoryIds_ShouldRemoveAll()
        {
            var handler = new RemoveRecipeCategoryCommandHandler(DbContext);
            var dessertsId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var recipeBefore = await DbContext.Recipes.Include(r => r.Categories).FirstAsync(r => r.Id == RecipeId);
            var countBefore = recipeBefore.Categories.Count;
            Assert.That(recipeBefore.Categories.Any(c => c.Id == BreakfastsCategoryId), Is.True);
            Assert.That(recipeBefore.Categories.Any(c => c.Id == dessertsId), Is.True);

            var command = new RemoveRecipeCategoryCommand
            {
                RecipeId = RecipeId,
                CategoryIds = [BreakfastsCategoryId, dessertsId]
            };

            await handler.Handle(command, CancellationToken.None);
            
            var recipeAfter = await DbContext.Recipes.Include(r => r.Categories).FirstAsync(r => r.Id == RecipeId);
            Assert.That(recipeAfter.Categories.Count, Is.EqualTo(countBefore - 2));
            Assert.That(recipeAfter.Categories.Any(c => c.Id == BreakfastsCategoryId), Is.False);
            Assert.That(recipeAfter.Categories.Any(c => c.Id == dessertsId), Is.False);
        }
    }
}
