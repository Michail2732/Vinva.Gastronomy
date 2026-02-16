using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Recipes.Application.RecipeUsecases.RemoveSteps;

namespace Vinva.Gastronomy.Recipes.Tests.Application
{
    [TestFixture]
    public class RemoveStepsCommandHandlerTests : BaseApplicationTests
    {
        private static readonly Guid RecipeWithStepsId = Guid.Parse("33333333-3333-3333-3333-333333333331");

        [Test]
        public async Task Handle_WithValidSeqNumbers_ShouldRemoveSteps()
        {
            var handler = new RemoveStepsCommandHandler(DbContext);
            var recipeBefore = await DbContext.Recipes.Include(r => r.Steps).FirstAsync(r => r.Id == RecipeWithStepsId);
            var stepCountBefore = recipeBefore.Steps.Count;
            Assert.That(stepCountBefore, Is.GreaterThanOrEqualTo(2));

            var command = new RemoveStepsCommand
            {
                RecipeId = RecipeWithStepsId,
                SeqNumbers = [2]
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);

            await DbContext.Entry(DbContext.Recipes.Find(RecipeWithStepsId)!)
                .Collection(r => r.Steps)
                .LoadAsync();
            var recipeAfter = await DbContext.Recipes.Include(r => r.Steps).FirstAsync(r => r.Id == RecipeWithStepsId);
            Assert.That(recipeAfter.Steps.Count, Is.EqualTo(stepCountBefore - 1));
            Assert.That(recipeAfter.Steps.Any(s => s.SeqNumber == 2), Is.False);
        }

        [Test]
        public void Handle_WithNonExistentRecipeId_ShouldThrowBadRequestException()
        {
            var handler = new RemoveStepsCommandHandler(DbContext);
            var command = new RemoveStepsCommand
            {
                RecipeId = Guid.NewGuid(),
                SeqNumbers = [1]
            };

            Assert.ThrowsAsync<BadRequestException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task Handle_WithMultipleSeqNumbers_ShouldRemoveAllSpecifiedSteps()
        {
            var handler = new RemoveStepsCommandHandler(DbContext);
            var recipeBefore = await DbContext.Recipes.Include(r => r.Steps).FirstAsync(r => r.Id == RecipeWithStepsId);
            var countBefore = recipeBefore.Steps.Count;
            Assert.That(countBefore, Is.GreaterThanOrEqualTo(3));

            var command = new RemoveStepsCommand
            {
                RecipeId = RecipeWithStepsId,
                SeqNumbers = [1, 3]
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            var recipeAfter = await DbContext.Recipes.Include(r => r.Steps).FirstAsync(r => r.Id == RecipeWithStepsId);
            Assert.That(recipeAfter.Steps.Count, Is.EqualTo(countBefore - 2));
            Assert.That(recipeAfter.Steps.Any(s => s.SeqNumber == 1), Is.False);
            Assert.That(recipeAfter.Steps.Any(s => s.SeqNumber == 3), Is.False);
        }

        [Test]
        public async Task Handle_WithEmptySeqNumbers_ShouldReturnValidationFailure()
        {
            var handler = new RemoveStepsCommandHandler(DbContext);
            var command = new RemoveStepsCommand
            {
                RecipeId = RecipeWithStepsId,
                SeqNumbers = []
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
        }
    }
}
