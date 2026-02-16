using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Recipes.Application.RecipeUsecases.AddRecipeCategories;

namespace Vinva.Gastronomy.Recipes.Tests.Application
{
    [TestFixture]
    public class AddRecipeCategoriesCommandHandlerTests : BaseApplicationTests
    {
        private static readonly Guid RecipeId = Guid.Parse("33333333-3333-3333-3333-333333333332"); // Omelette - has only Breakfast
        private static readonly Guid SoupsCategoryId = Guid.Parse("11111111-1111-1111-1111-111111111114");

        [Test]
        public async Task Handle_WithValidCategoryIds_ShouldAddCategoriesToRecipe()
        {
            var handler = new AddRecipeCategoriesCommandHandler(DbContext);
            var recipeBefore = await DbContext.Recipes.Include(r => r.Categories).FirstAsync(r => r.Id == RecipeId);
            Assert.That(recipeBefore.Categories.Any(c => c.Id == SoupsCategoryId), Is.False);

            var command = new AddRecipeCategoriesCommand
            {
                RecipeId = RecipeId,
                CategoryIds = [SoupsCategoryId]
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);

            var recipeAfter = await DbContext.Recipes.Include(r => r.Categories).FirstAsync(r => r.Id == RecipeId);
            Assert.That(recipeAfter.Categories.Any(c => c.Id == SoupsCategoryId), Is.True);
        }

        [Test]
        public void Handle_WithNonExistentRecipeId_ShouldThrowBadRequestException()
        {
            var handler = new AddRecipeCategoriesCommandHandler(DbContext);
            var command = new AddRecipeCategoriesCommand
            {
                RecipeId = Guid.NewGuid(),
                CategoryIds = [SoupsCategoryId]
            };

            Assert.ThrowsAsync<BadRequestException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void Handle_WithNonExistentCategoryId_ShouldThrowBadRequestException()
        {
            var handler = new AddRecipeCategoriesCommandHandler(DbContext);
            var command = new AddRecipeCategoriesCommand
            {
                RecipeId = RecipeId,
                CategoryIds = [Guid.NewGuid()]
            };

            Assert.ThrowsAsync<BadRequestException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task Handle_WithMultipleCategoryIds_ShouldAddAllCategories()
        {
            var handler = new AddRecipeCategoriesCommandHandler(DbContext);
            var mainDishesId = Guid.Parse("11111111-1111-1111-1111-111111111112");
            var soupsId = Guid.Parse("11111111-1111-1111-1111-111111111114");
            var recipeBefore = await DbContext.Recipes.Include(r => r.Categories).FirstAsync(r => r.Id == RecipeId);
            Assert.That(recipeBefore.Categories.Any(c => c.Id == mainDishesId), Is.False);
            Assert.That(recipeBefore.Categories.Any(c => c.Id == soupsId), Is.False);

            var command = new AddRecipeCategoriesCommand
            {
                RecipeId = RecipeId,
                CategoryIds = [mainDishesId, SoupsCategoryId]
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            var recipeAfter = await DbContext.Recipes.Include(r => r.Categories).FirstAsync(r => r.Id == RecipeId);
            Assert.That(recipeAfter.Categories.Any(c => c.Id == mainDishesId), Is.True);
            Assert.That(recipeAfter.Categories.Any(c => c.Id == SoupsCategoryId), Is.True);
        }

        [Test]
        public async Task Handle_WithEmptyCategoryIds_ShouldReturnValidationFailure()
        {
            var handler = new AddRecipeCategoriesCommandHandler(DbContext);
            var command = new AddRecipeCategoriesCommand
            {
                RecipeId = RecipeId,
                CategoryIds = []
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public async Task Handle_WithDuplicateCategoryIds_ShouldAddCategoryOnce()
        {
            var handler = new AddRecipeCategoriesCommandHandler(DbContext);
            var recipeBefore = await DbContext.Recipes.Include(r => r.Categories).FirstAsync(r => r.Id == RecipeId);
            var countBefore = recipeBefore.Categories.Count;
            Assert.That(recipeBefore.Categories.Any(c => c.Id == SoupsCategoryId), Is.False);

            var command = new AddRecipeCategoriesCommand
            {
                RecipeId = RecipeId,
                CategoryIds = [SoupsCategoryId, SoupsCategoryId]
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            var recipeAfter = await DbContext.Recipes.Include(r => r.Categories).FirstAsync(r => r.Id == RecipeId);
            Assert.That(recipeAfter.Categories.Count, Is.EqualTo(countBefore + 1));
            Assert.That(recipeAfter.Categories.Count(c => c.Id == SoupsCategoryId), Is.EqualTo(1));
        }
    }
}
