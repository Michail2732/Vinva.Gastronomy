using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Recipes.Application.RecipeUsecases.GetRecipeByCategory;
using Vinva.Gastronomy.Recipes.Application.RecipeUsecases.GetRecipeByIngredients;

namespace Vinva.Gastronomy.Recipes.Tests.ApplicationTests
{
    [TestFixture]
    public class GetRecipeByCategoryHandlerTests : BaseApplicationTests
    {
        private readonly Guid DessertsCategoryId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        private readonly Guid BreakfastsCategoryId = Guid.Parse("11111111-1111-1111-1111-111111111113");
        private readonly Guid MainDishesCategoryId = Guid.Parse("11111111-1111-1111-1111-111111111112");
        private readonly Guid SoupsCategoryId = Guid.Parse("11111111-1111-1111-1111-111111111114");

        [Test]
        public async Task Handle_WhenIncludeSingleCategory_ShouldReturnRecipesWithThatCategory()
        {
            // Arrange
            var handler = new GetRecipeByCategoryHandler(DbContext);
            var request = new GetRecipeByCategoryRequest
            {
                Include = new List<Guid> { DessertsCategoryId },
                Exclude = null,
                IncludeLogicAnd = false
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value == default, Is.False);
            Assert.That(result.Value.Recipes, Is.Not.Null);
            Assert.That(result.Value.Recipes.Count, Is.GreaterThan(0));
            
            // Verify all returned recipes have the desserts category
            var allHaveDessertsCategory = result.Value.Recipes.All(r =>
                r.Categories.Any(c => c.Id == DessertsCategoryId));
            Assert.That(allHaveDessertsCategory, Is.True);
        }

        [Test]
        public async Task Handle_WhenIncludeMultipleCategoriesWithOrLogic_ShouldReturnRecipesWithAnyCategory()
        {
            // Arrange
            var handler = new GetRecipeByCategoryHandler(DbContext);
            var request = new GetRecipeByCategoryRequest
            {
                Include = new List<Guid> { DessertsCategoryId, BreakfastsCategoryId },
                Exclude = null,
                IncludeLogicAnd = false
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value == default, Is.False);
            Assert.That(result.Value.Recipes, Is.Not.Null);
            Assert.That(result.Value.Recipes.Count, Is.GreaterThan(0));
            
            // Verify all returned recipes have at least one of the categories
            var allHaveAtLeastOneCategory = result.Value.Recipes.All(r =>
                r.Categories.Any(c => c.Id == DessertsCategoryId || c.Id == BreakfastsCategoryId));
            Assert.That(allHaveAtLeastOneCategory, Is.True);
        }

        [Test]
        public async Task Handle_WhenIncludeMultipleCategoriesWithAndLogic_ShouldReturnRecipesWithAllCategories()
        {
            // Arrange
            var handler = new GetRecipeByCategoryHandler(DbContext);
            var request = new GetRecipeByCategoryRequest
            {
                Include = new List<Guid> { DessertsCategoryId, BreakfastsCategoryId },
                Exclude = null,
                IncludeLogicAnd = true
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value == default, Is.False);
            Assert.That(result.Value.Recipes, Is.Not.Null);
            
            // Verify all returned recipes have both categories
            var allHaveBothCategories = result.Value.Recipes.All(r =>
                r.Categories.Any(c => c.Id == DessertsCategoryId) &&
                r.Categories.Any(c => c.Id == BreakfastsCategoryId));
            Assert.That(allHaveBothCategories, Is.True);
        }

        [Test]
        public async Task Handle_WhenExcludeCategory_ShouldNotReturnRecipesWithThatCategory()
        {
            // Arrange
            var handler = new GetRecipeByCategoryHandler(DbContext);
            var request = new GetRecipeByCategoryRequest
            {
                Include = null,
                Exclude = new List<Guid> { DessertsCategoryId },
                IncludeLogicAnd = false
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value == default, Is.False);
            Assert.That(result.Value.Recipes, Is.Not.Null);
            
            // Verify no returned recipes have the excluded category
            var noneHaveExcludedCategory = result.Value.Recipes.All(r =>
                !r.Categories.Any(c => c.Id == DessertsCategoryId));
            Assert.That(noneHaveExcludedCategory, Is.True);
        }

        [Test]
        public async Task Handle_WhenIncludeAndExclude_ShouldReturnRecipesMatchingBothConditions()
        {
            // Arrange
            var handler = new GetRecipeByCategoryHandler(DbContext);
            var request = new GetRecipeByCategoryRequest
            {
                Include = new List<Guid> { BreakfastsCategoryId },
                Exclude = new List<Guid> { DessertsCategoryId },
                IncludeLogicAnd = false
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value == default, Is.False);
            Assert.That(result.Value.Recipes, Is.Not.Null);
            
            // Verify all returned recipes have breakfast category but not desserts
            var allMatchConditions = result.Value.Recipes.All(r =>
                r.Categories.Any(c => c.Id == BreakfastsCategoryId) &&
                !r.Categories.Any(c => c.Id == DessertsCategoryId));
            Assert.That(allMatchConditions, Is.True);
        }

        [Test]
        public async Task Handle_WhenNoFilters_ShouldReturnAllRecipes()
        {
            // Arrange
            var handler = new GetRecipeByCategoryHandler(DbContext);
            var request = new GetRecipeByCategoryRequest
            {
                Include = null,
                Exclude = null,
                IncludeLogicAnd = false
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value == default, Is.False);
            Assert.That(result.Value.Recipes, Is.Not.Null);
            
            // Should return all recipes from test data (4 recipes)
            Assert.That(result.Value.Recipes.Count, Is.EqualTo(4));
        }

        [Test]
        public async Task Handle_WhenIncludeNonExistentCategory_ShouldReturnEmptyList()
        {
            // Arrange
            var handler = new GetRecipeByCategoryHandler(DbContext);
            var nonExistentCategoryId = Guid.NewGuid();
            var request = new GetRecipeByCategoryRequest
            {
                Include = new List<Guid> { nonExistentCategoryId },
                Exclude = null,
                IncludeLogicAnd = false
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value == default, Is.False);
            Assert.That(result.Value.Recipes, Is.Not.Null);
            Assert.That(result.Value.Recipes.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task Handle_ShouldReturnRecipesWithAllRelatedData()
        {
            // Arrange
            var handler = new GetRecipeByCategoryHandler(DbContext);
            var request = new GetRecipeByCategoryRequest
            {
                Include = new List<Guid> { DessertsCategoryId },
                Exclude = null,
                IncludeLogicAnd = false
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value == default, Is.False);
            Assert.That(result.Value.Recipes, Is.Not.Null);
            
            // Verify recipes have all required data populated
            foreach (var recipe in result.Value.Recipes)
            {
                Assert.That(recipe.Id, Is.Not.EqualTo(Guid.Empty));
                Assert.That(recipe.Name, Is.Not.Null.And.Not.Empty);
                Assert.That(recipe.Ingredients, Is.Not.Null);
                Assert.That(recipe.Categories, Is.Not.Null);
                Assert.That(recipe.Steps, Is.Not.Null);
            }
        }
    }
}
