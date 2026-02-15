using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Recipes.Application.RecipeUsecases.CreateRecipe;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Tests.Application
{
    [TestFixture]
    public class CreateRecipeHandlerTests : BaseApplicationTests
    {
        [Test]
        public async Task Handle_WithValidCommand_ShouldCreateRecipeAndReturnSuccess()
        {
            var handler = new CreateRecipeHandler(DbContext);
            var command = new CreateRecipeCommand
            {
                Name = "Test Recipe",
                Description = "Test description for recipe.",
                CookingTime = TimeSpan.FromMinutes(30),
                Comment = "Optional comment."
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.RecipeId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(result.Value.Name, Is.EqualTo("Test Recipe"));

            var created = await DbContext.Recipes.FindAsync(result.Value.RecipeId);
            Assert.That(created, Is.Not.Null);
            Assert.That(created!.Name, Is.EqualTo("Test Recipe"));
            Assert.That(created.Description, Is.EqualTo("Test description for recipe."));
        }

        [Test]
        public async Task Handle_WithInvalidName_ShouldReturnValidationFailure()
        {
            var handler = new CreateRecipeHandler(DbContext);
            var command = new CreateRecipeCommand
            {
                Name = "",
                Description = "Valid description.",
                CookingTime = TimeSpan.FromMinutes(30)
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public async Task Handle_WithInvalidCookingTime_ShouldReturnValidationFailure()
        {
            var handler = new CreateRecipeHandler(DbContext);
            var command = new CreateRecipeCommand
            {
                Name = "Valid Name",
                Description = "Valid description.",
                CookingTime = TimeSpan.Zero
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public async Task Handle_WithInvalidDescription_ShouldReturnValidationFailure()
        {
            var handler = new CreateRecipeHandler(DbContext);
            var command = new CreateRecipeCommand
            {
                Name = "Valid Name",
                Description = "",
                CookingTime = TimeSpan.FromMinutes(30)
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public async Task Handle_WithAllOptionalFields_ShouldPersistCorrectly()
        {
            var handler = new CreateRecipeHandler(DbContext);
            var baseRecipeId = Guid.Parse("33333333-3333-3333-3333-333333333331");
            var command = new CreateRecipeCommand
            {
                Name = "Derived Recipe",
                Description = "Description with optional fields.",
                CookingTime = TimeSpan.FromHours(1),
                BaseRecipe = baseRecipeId,
                Comment = "Comment text.",
                StorageComment = "Store in fridge.",
                UsageComment = "Use within 2 days."
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            var created = await DbContext.Recipes.FindAsync(result.Value.RecipeId);
            Assert.That(created, Is.Not.Null);
            Assert.That(created!.BaseRecipe, Is.EqualTo(baseRecipeId));
            Assert.That(created.Comment, Is.EqualTo("Comment text."));
            Assert.That(created.StorageComment, Is.EqualTo("Store in fridge."));
            Assert.That(created.UsageComment, Is.EqualTo("Use within 2 days."));
        }

        [Test]
        public async Task Handle_WithMinimalValidCommand_ShouldCreateRecipe()
        {
            var handler = new CreateRecipeHandler(DbContext);
            var command = new CreateRecipeCommand
            {
                Name = "Minimal Recipe",
                Description = "Minimal description.",
                CookingTime = TimeSpan.FromMinutes(1)
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.RecipeId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(result.Value.Name, Is.EqualTo("Minimal Recipe"));
        }
    }
}
