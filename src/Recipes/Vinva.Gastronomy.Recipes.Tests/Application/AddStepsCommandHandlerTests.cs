using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Recipes.Application.Common;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.AddSteps;

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
                    new RecipeStepDto {  Description = "Do step A.", SeqNumber = 4 },
                    new RecipeStepDto { Description = "Do step B.", SeqNumber = 5 }
                ]
            };

            await handler.Handle(command, CancellationToken.None);            

            await DbContext.Entry(DbContext.Recipes.Find(ExistingRecipeId)!)
                .Collection(r => r.Steps)
                .LoadAsync();
            var recipe = await DbContext.Recipes.Include(r => r.Steps).FirstAsync(r => r.Id == ExistingRecipeId);
            Assert.That(recipe.Steps.Count, Is.GreaterThanOrEqualTo(4));
            Assert.That(recipe.Steps.Any(s => s.Description == "Do step A." && s.SeqNumber == 4), Is.True);
            Assert.That(recipe.Steps.Any(s => s.Description == "Do step B." && s.SeqNumber == 5), Is.True);
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

            var ex = Assert.Catch<BadRequestException>(async () => await handler.Handle(command, CancellationToken.None));

            Assert.That(ex, Is.Not.Null);            
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

            var ex = Assert.Catch<BadRequestException>(async () => await handler.Handle(command, CancellationToken.None));

            Assert.That(ex, Is.Not.Null);            
        }

        [Test]
        public async Task Handle_WithSingleStep_ShouldAddOneStep()
        {
            var handler = new AddStepsCommandHandler(DbContext);
            var command = new AddStepsCommand
            {
                RecipeId = ExistingRecipeId,
                Steps = [new RecipeStepDto { Description = "Single step description.", Comment = "Optional.", SeqNumber = 999}]
            };

            await handler.Handle(command, CancellationToken.None);
            
            var recipe = await DbContext.Recipes.Include(r => r.Steps).FirstAsync(r => r.Id == ExistingRecipeId);
            var lastStep = recipe.Steps.LastOrDefault();
            Assert.That(lastStep, Is.Not.Null);
            Assert.That(lastStep.Description == "Single step description.", Is.True);
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

            await handler.Handle(command, CancellationToken.None);
            
            var recipe = await DbContext.Recipes.Include(r => r.Steps).FirstAsync(r => r.Id == ExistingRecipeId);
            var lastStepIndex = recipe.Steps.Count - 1;
            Assert.That(recipe.Steps[lastStepIndex-2].Description == "First.", Is.True);
            Assert.That(recipe.Steps[lastStepIndex-1].Description == "Third.", Is.True);
            Assert.That(recipe.Steps[lastStepIndex-0].Description == "Second.", Is.True);
        }
    }
}
