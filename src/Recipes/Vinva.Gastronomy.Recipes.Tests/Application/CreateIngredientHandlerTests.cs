using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Recipes.Application.IngredientUsecases.CreateIngredient;

namespace Vinva.Gastronomy.Recipes.Tests.Application
{
    [TestFixture]
    public class CreateIngredientHandlerTests : BaseApplicationTests
    {
        [Test]
        public async Task Handle_WithValidCommand_ShouldCreateIngredientAndReturnSuccess()
        {
            var handler = new CreateIngredientHandler(DbContext);
            var command = new CreateIngredientCommand
            {
                Name = "New Ingredient",
                Description = "Test ingredient description.",
                UsageComment = "Store in dry place."
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.IngredientId, Is.Not.EqualTo(Guid.Empty));

            var created = await DbContext.Ingredients.FindAsync(result.Value.IngredientId);
            Assert.That(created, Is.Not.Null);
            Assert.That(created!.Name, Is.EqualTo("New Ingredient"));
        }

        [Test]
        public async Task Handle_WithInvalidName_ShouldReturnValidationFailure()
        {
            var handler = new CreateIngredientHandler(DbContext);
            var command = new CreateIngredientCommand
            {
                Name = "",
                Description = "Valid description."
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public async Task Handle_WithInvalidDescription_ShouldReturnValidationFailure()
        {
            var handler = new CreateIngredientHandler(DbContext);
            var command = new CreateIngredientCommand
            {
                Name = "Valid Name",
                Description = ""
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public async Task Handle_WithoutUsageComment_ShouldCreateIngredient()
        {
            var handler = new CreateIngredientHandler(DbContext);
            var command = new CreateIngredientCommand
            {
                Name = "Simple Ingredient",
                Description = "No usage comment.",
                UsageComment = null
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            var created = await DbContext.Ingredients.FindAsync(result.Value.IngredientId);
            Assert.That(created, Is.Not.Null);
            Assert.That(created!.UsageComment, Is.Null);
        }
    }
}
