using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Recipes.Application.Common;
using Vinva.Gastronomy.Recipes.Application.RecipeUsecases.AddIngredients;

namespace Vinva.Gastronomy.Recipes.Tests.Application
{
    [TestFixture]
    public class AddIngredientCommandHandlerTests : BaseApplicationTests
    {
        private static readonly Guid ExistingRecipeId = Guid.Parse("33333333-3333-3333-3333-333333333331");
        private static readonly Guid ExistingIngredientId = Guid.Parse("44444444-4444-4444-4444-444444444449"); // Vanillin - not in this recipe

        [Test]
        public async Task Handle_WithValidCommand_ShouldAddIngredientToRecipe()
        {
            var handler = new AddIngredientCommandHandler(DbContext);
            var command = new AddIngredientCommand
            {
                RecipeId = ExistingRecipeId,
                Ingredients =
                [
                    new RecipeIngredientDto
                    {
                        IngredientId = ExistingIngredientId,
                        IngredientName = "Vanillin",
                        IsRequired = false,
                        Quantities = [new IngredientQuantityDto { Measure = "гр.", Quantity = 1 }]
                    }
                ]
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);

            var recipe = await DbContext.Recipes.Include(r => r.Ingredients).FirstAsync(r => r.Id == ExistingRecipeId);
            Assert.That(recipe.Ingredients.Any(i => i.IngredientId == ExistingIngredientId), Is.True);
        }

        [Test]
        public void Handle_WithNonExistentRecipeId_ShouldThrowBadRequestException()
        {
            var handler = new AddIngredientCommandHandler(DbContext);
            var command = new AddIngredientCommand
            {
                RecipeId = Guid.NewGuid(),
                Ingredients =
                [
                    new RecipeIngredientDto
                    {
                        IngredientId = ExistingIngredientId,
                        IngredientName = "Vanillin",
                        IsRequired = false,
                        Quantities = [new IngredientQuantityDto { Measure = "гр.", Quantity = 1 }]
                    }
                ]
            };

            Assert.ThrowsAsync<BadRequestException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void Handle_WithNonExistentIngredientId_ShouldThrowBadRequestException()
        {
            var handler = new AddIngredientCommandHandler(DbContext);
            var command = new AddIngredientCommand
            {
                RecipeId = ExistingRecipeId,
                Ingredients =
                [
                    new RecipeIngredientDto
                    {
                        IngredientId = Guid.NewGuid(),
                        IngredientName = "Fake",
                        IsRequired = false,
                        Quantities = [new IngredientQuantityDto { Measure = "гр.", Quantity = 1 }]
                    }
                ]
            };

            Assert.ThrowsAsync<BadRequestException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task Handle_WithMultipleIngredients_ShouldAddAllToRecipe()
        {
            var handler = new AddIngredientCommandHandler(DbContext);
            var flourId = Guid.Parse("44444444-4444-4444-4444-444444444441");
            var sugarId = Guid.Parse("44444444-4444-4444-4444-444444444442");
            var omeletteRecipeId = Guid.Parse("33333333-3333-3333-3333-333333333332");
            var command = new AddIngredientCommand
            {
                RecipeId = omeletteRecipeId,
                Ingredients =
                [
                    new RecipeIngredientDto
                    {
                        IngredientId = flourId,
                        IngredientName = "Flour",
                        IsRequired = false,
                        Quantities = [new IngredientQuantityDto { Measure = "гр.", Quantity = 1 }]
                    },
                    new RecipeIngredientDto
                    {
                        IngredientId = sugarId,
                        IngredientName = "Sugar",
                        IsRequired = false,
                        Quantities = [new IngredientQuantityDto { Measure = "гр.", Quantity = 1 }]
                    }
                ]
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            var recipe = await DbContext.Recipes.Include(r => r.Ingredients).FirstAsync(r => r.Id == omeletteRecipeId);
            Assert.That(recipe.Ingredients.Any(i => i.IngredientId == flourId), Is.True);
            Assert.That(recipe.Ingredients.Any(i => i.IngredientId == sugarId), Is.True);
        }

        [Test]
        public async Task Handle_WithEmptyIngredients_ShouldReturnValidationFailure()
        {
            var handler = new AddIngredientCommandHandler(DbContext);
            var command = new AddIngredientCommand
            {
                RecipeId = ExistingRecipeId,
                Ingredients = []
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public async Task Handle_WithIsRequiredTrue_ShouldPersistCorrectly()
        {
            var handler = new AddIngredientCommandHandler(DbContext);
            var command = new AddIngredientCommand
            {
                RecipeId = ExistingRecipeId,
                Ingredients =
                [
                    new RecipeIngredientDto
                    {
                        IngredientId = ExistingIngredientId,
                        IngredientName = "Vanillin",
                        IsRequired = true,
                        Quantities = [new IngredientQuantityDto { Measure = "гр.", Quantity = 1 }]
                    }
                ]
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            var recipe = await DbContext.Recipes.Include(r => r.Ingredients).FirstAsync(r => r.Id == ExistingRecipeId);
            var added = recipe.Ingredients.First(i => i.IngredientId == ExistingIngredientId);
            Assert.That(added.IsRequired, Is.True);
        }
    }
}
