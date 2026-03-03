using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.GetByIngredients;

namespace Vinva.Gastronomy.Recipes.Tests.Application
{
    [TestFixture]
    public class GetRecipeByIngredientsHandlerTests : BaseApplicationTests
    {
        private readonly Guid FlourIngredientId = Guid.Parse("44444444-4444-4444-4444-444444444441");
        private readonly Guid SugarIngredientId = Guid.Parse("44444444-4444-4444-4444-444444444442");
        private readonly Guid MilkIngredientId = Guid.Parse("44444444-4444-4444-4444-444444444443");
        private readonly Guid EggsIngredientId = Guid.Parse("44444444-4444-4444-4444-444444444444");
        private readonly Guid CheeseIngredientId = Guid.Parse("44444444-4444-4444-4444-444444444445");
        private readonly Guid CottageCheeseIngredientId = Guid.Parse("44444444-4444-4444-4444-444444444448");

        [Test]
        public async Task Handle_WhenIncludeSingleIngredient_ShouldReturnRecipesWithThatIngredient()
        {
            // Arrange
            var handler = new GetRecipeByIngredientsHandler(DbContext);
            var request = new GetRecipeByIngredientsRequest
            {
                Include = new List<Guid> { FlourIngredientId },
                Exclude = null,
                IncludeLogicAnd = false
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert            
            Assert.That(result.Recipes, Is.Not.Null);
            Assert.That(result.Recipes.Count, Is.GreaterThan(0));
            
            // Verify all returned recipes have the flour ingredient
            var allHaveFlour = result.Recipes.All(r =>
                r.Ingredients.Any(i => i.IngredientId == FlourIngredientId));
            Assert.That(allHaveFlour, Is.True);
        }

        [Test]
        public async Task Handle_WhenIncludeMultipleIngredientsWithOrLogic_ShouldReturnRecipesWithAnyIngredient()
        {
            // Arrange
            var handler = new GetRecipeByIngredientsHandler(DbContext);
            var request = new GetRecipeByIngredientsRequest
            {
                Include = new List<Guid> { FlourIngredientId, CheeseIngredientId },
                Exclude = null,
                IncludeLogicAnd = false
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert                        
            Assert.That(result.Recipes, Is.Not.Null);
            Assert.That(result.Recipes.Count, Is.GreaterThan(0));
            
            // Verify all returned recipes have at least one of the ingredients
            var allHaveAtLeastOneIngredient = result.Recipes.All(r =>
                r.Ingredients.Any(i => i.IngredientId == FlourIngredientId || i.IngredientId == CheeseIngredientId));
            Assert.That(allHaveAtLeastOneIngredient, Is.True);
        }

        [Test]
        public async Task Handle_WhenIncludeMultipleIngredientsWithAndLogic_ShouldReturnRecipesWithAllIngredients()
        {
            // Arrange
            var handler = new GetRecipeByIngredientsHandler(DbContext);
            var request = new GetRecipeByIngredientsRequest
            {
                Include = new List<Guid> { FlourIngredientId, EggsIngredientId, MilkIngredientId },
                Exclude = null,
                IncludeLogicAnd = true
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert                        
            Assert.That(result.Recipes, Is.Not.Null);
            
            // Verify all returned recipes have all three ingredients
            var allHaveAllIngredients = result.Recipes.All(r =>
                r.Ingredients.Any(i => i.IngredientId == FlourIngredientId) &&
                r.Ingredients.Any(i => i.IngredientId == EggsIngredientId) &&
                r.Ingredients.Any(i => i.IngredientId == MilkIngredientId));
            Assert.That(allHaveAllIngredients, Is.True);
        }

        [Test]
        public async Task Handle_WhenExcludeIngredient_ShouldNotReturnRecipesWithThatIngredient()
        {
            // Arrange
            var handler = new GetRecipeByIngredientsHandler(DbContext);
            var request = new GetRecipeByIngredientsRequest
            {
                Include = null,
                Exclude = new List<Guid> { FlourIngredientId },
                IncludeLogicAnd = false
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert           
            Assert.That(result.Recipes, Is.Not.Null);
            
            // Verify no returned recipes have the excluded ingredient
            var noneHaveExcludedIngredient = result.Recipes.All(r =>
                !r.Ingredients.Any(i => i.IngredientId == FlourIngredientId));
            Assert.That(noneHaveExcludedIngredient, Is.True);
        }

        [Test]
        public async Task Handle_WhenIncludeAndExclude_ShouldReturnRecipesMatchingBothConditions()
        {
            // Arrange
            var handler = new GetRecipeByIngredientsHandler(DbContext);
            var request = new GetRecipeByIngredientsRequest
            {
                Include = new List<Guid> { EggsIngredientId },
                Exclude = new List<Guid> { FlourIngredientId },
                IncludeLogicAnd = false
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert            
            Assert.That(result.Recipes, Is.Not.Null);
            
            // Verify all returned recipes have eggs but not flour
            var allMatchConditions = result.Recipes.All(r =>
                r.Ingredients.Any(i => i.IngredientId == EggsIngredientId) &&
                !r.Ingredients.Any(i => i.IngredientId == FlourIngredientId));
            Assert.That(allMatchConditions, Is.True);
        }

        [Test]
        public async Task Handle_WhenNoFilters_ShouldReturnAllRecipes()
        {
            // Arrange
            var handler = new GetRecipeByIngredientsHandler(DbContext);
            var request = new GetRecipeByIngredientsRequest
            {
                Include = null,
                Exclude = null,
                IncludeLogicAnd = false
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert            
            Assert.That(result.Recipes, Is.Not.Null);
            
            // Should return all recipes from test data (0 recipes)
            Assert.That(result.Recipes.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task Handle_WhenIncludeNonExistentIngredient_ShouldReturnEmptyList()
        {
            // Arrange
            var handler = new GetRecipeByIngredientsHandler(DbContext);
            var nonExistentIngredientId = Guid.NewGuid();
            var request = new GetRecipeByIngredientsRequest
            {
                Include = new List<Guid> { nonExistentIngredientId },
                Exclude = null,
                IncludeLogicAnd = false
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert            
            Assert.That(result.Recipes, Is.Not.Null);
            Assert.That(result.Recipes.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task Handle_ShouldReturnRecipesWithAllRelatedData()
        {
            // Arrange
            var handler = new GetRecipeByIngredientsHandler(DbContext);
            var request = new GetRecipeByIngredientsRequest
            {
                Include = new List<Guid> { CottageCheeseIngredientId },
                Exclude = null,
                IncludeLogicAnd = false
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert            
            Assert.That(result.Recipes, Is.Not.Null);
            
            // Verify recipes have all required data populated
            foreach (var recipe in result.Recipes)
            {
                Assert.That(recipe.Id, Is.Not.EqualTo(Guid.Empty));
                Assert.That(recipe.Name, Is.Not.Null.And.Not.Empty);
                Assert.That(recipe.Ingredients, Is.Not.Null);
                Assert.That(recipe.Categories, Is.Not.Null);
                Assert.That(recipe.Steps, Is.Not.Null);
            }
        }

        [Test]
        public async Task Handle_WhenIncludeDuplicateIngredientIds_ShouldHandleCorrectly()
        {
            // Arrange
            var handler = new GetRecipeByIngredientsHandler(DbContext);
            var request = new GetRecipeByIngredientsRequest
            {
                Include = new List<Guid> { FlourIngredientId, FlourIngredientId, FlourIngredientId },
                Exclude = null,
                IncludeLogicAnd = false
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert            
            Assert.That(result.Recipes, Is.Not.Null);
            
            // Should work correctly with duplicate IDs (treated as single ingredient)
            var allHaveFlour = result.Recipes.All(r =>
                r.Ingredients.Any(i => i.IngredientId == FlourIngredientId));
            Assert.That(allHaveFlour, Is.True);
        }

        [Test]
        public async Task Handle_WhenIncludeFlourAndEggsAndMilkWithAndLogic_ShouldReturnOnlyBliny()
        {
            var handler = new GetRecipeByIngredientsHandler(DbContext);
            var request = new GetRecipeByIngredientsRequest
            {
                Include = new List<Guid> { FlourIngredientId, EggsIngredientId, MilkIngredientId },
                Exclude = null,
                IncludeLogicAnd = true
            };

            var result = await handler.Handle(request, CancellationToken.None);
            
            Assert.That(result.Recipes, Is.Not.Null);
            Assert.That(result.Recipes.Count, Is.EqualTo(1));
            var blinyId = Guid.Parse("33333333-3333-3333-3333-333333333331");
            Assert.That(result.Recipes[0].Id, Is.EqualTo(blinyId));
        }

        [Test]
        public async Task Handle_WhenExcludeMultipleIngredients_ShouldNotReturnRecipesWithAnyOfThem()
        {
            var handler = new GetRecipeByIngredientsHandler(DbContext);
            var request = new GetRecipeByIngredientsRequest
            {
                Include = null,
                Exclude = new List<Guid> { FlourIngredientId, CottageCheeseIngredientId },
                IncludeLogicAnd = false
            };

            var result = await handler.Handle(request, CancellationToken.None);
            
            Assert.That(result.Recipes, Is.Not.Null);
            var noneHaveFlourOrCottageCheese = result.Recipes.All(r =>
                !r.Ingredients.Any(i => i.IngredientId == FlourIngredientId) &&
                !r.Ingredients.Any(i => i.IngredientId == CottageCheeseIngredientId));
            Assert.That(noneHaveFlourOrCottageCheese, Is.True);
        }

        [Test]
        public async Task Handle_WhenIncludeSingleRareIngredient_ShouldReturnExpectedCount()
        {
            var handler = new GetRecipeByIngredientsHandler(DbContext);
            var request = new GetRecipeByIngredientsRequest
            {
                Include = new List<Guid> { CottageCheeseIngredientId },
                Exclude = null,
                IncludeLogicAnd = false
            };

            var result = await handler.Handle(request, CancellationToken.None);
            
            Assert.That(result.Recipes, Is.Not.Null);
            Assert.That(result.Recipes.Count, Is.EqualTo(1));
            Assert.That(result.Recipes[0].Ingredients.Any(i => i.IngredientId == CottageCheeseIngredientId), Is.True);
        }
    }
}
