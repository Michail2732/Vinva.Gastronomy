using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Recipes.Application.Common;
using Vinva.Gastronomy.Recipes.Application.RecipeUsecases.AddSteps;

namespace Vinva.Gastronomy.Recipes.Tests.Application
{
    [TestFixture]
    public class AddStepsCommandHandlerTests : BaseApplicationTests
    {
        private static readonly Guid ExistingRecipeId = Guid.Parse("33333333-3333-3333-3333-333333333331");

        [Test]
        public async Task Handle_WithValidCommand_ShouldAddStepsToRecipe()
        {
            var handler = new AddStepsCommandHandler(DbContext);
            var command = new AddStepsCommand
            {
                RecipeId = ExistingRecipeId,
                Steps =
                [
                    new RecipeStepDto { Name = "Step A", Description = "Do step A.", SeqNumber = 4 },
                    new RecipeStepDto { Name = "Step B", Description = "Do step B.", SeqNumber = 5 }
                ]
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);

            await DbContext.Entry(DbContext.Recipes.Find(ExistingRecipeId)!)
                .Collection(r => r.Steps)
                .LoadAsync();
            var recipe = await DbContext.Recipes.Include(r => r.Steps).FirstAsync(r => r.Id == ExistingRecipeId);
            Assert.That(recipe.Steps.Count, Is.GreaterThanOrEqualTo(4));
            Assert.That(recipe.Steps.Any(s => s.Name == "Step A" && s.SeqNumber == 4), Is.True);
            Assert.That(recipe.Steps.Any(s => s.Name == "Step B" && s.SeqNumber == 5), Is.True);
        }

        [Test]
        public void Handle_WithNonExistentRecipeId_ShouldThrowBadRequestException()
        {
            var handler = new AddStepsCommandHandler(DbContext);
            var command = new AddStepsCommand
            {
                RecipeId = Guid.NewGuid(),
                Steps = [new RecipeStepDto { Description = "Valid step.", SeqNumber = 1 }]
            };

            Assert.ThrowsAsync<BadRequestException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task Handle_WithInvalidStepDescription_ShouldReturnValidationFailure()
        {
            var handler = new AddStepsCommandHandler(DbContext);
            var command = new AddStepsCommand
            {
                RecipeId = ExistingRecipeId,
                Steps = [new RecipeStepDto { Description = "", SeqNumber = 1 }]
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public async Task Handle_WithEmptySteps_ShouldReturnValidationFailure()
        {
            var handler = new AddStepsCommandHandler(DbContext);
            var command = new AddStepsCommand
            {
                RecipeId = ExistingRecipeId,
                Steps = []
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public async Task Handle_WithSingleStep_ShouldAddOneStep()
        {
            var handler = new AddStepsCommandHandler(DbContext);
            var command = new AddStepsCommand
            {
                RecipeId = ExistingRecipeId,
                Steps = [new RecipeStepDto { Name = "Solo Step", Description = "Single step description.", Comment = "Optional.", SeqNumber = 10 }]
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            var recipe = await DbContext.Recipes.Include(r => r.Steps).FirstAsync(r => r.Id == ExistingRecipeId);
            Assert.That(recipe.Steps.Any(s => s.SeqNumber == 10 && s.Name == "Solo Step"), Is.True);
        }

        [Test]
        public async Task Handle_WithStepsOutOfOrder_ShouldOrderBySeqNumber()
        {
            var handler = new AddStepsCommandHandler(DbContext);
            var command = new AddStepsCommand
            {
                RecipeId = ExistingRecipeId,
                Steps =
                [
                    new RecipeStepDto { Description = "Third.", SeqNumber = 7 },
                    new RecipeStepDto { Description = "First.", SeqNumber = 6 },
                    new RecipeStepDto { Description = "Second.", SeqNumber = 8 }
                ]
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            var recipe = await DbContext.Recipes.Include(r => r.Steps).FirstAsync(r => r.Id == ExistingRecipeId);
            Assert.That(recipe.Steps.Any(s => s.SeqNumber == 6 && s.Description == "First."), Is.True);
            Assert.That(recipe.Steps.Any(s => s.SeqNumber == 7 && s.Description == "Third."), Is.True);
            Assert.That(recipe.Steps.Any(s => s.SeqNumber == 8 && s.Description == "Second."), Is.True);
        }
    }
}
