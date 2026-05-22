using Vinva.Gastronomy.Common.Exceptions;

using Vinva.Gastronomy.Recipes.Application.Usecases.Ingredients.Create;

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
                Comment = "Store in dry place."
            };

            var result = await handler.Handle(command, CancellationToken.None);
            
            Assert.That(result.IngredientId, Is.Not.EqualTo(Guid.Empty));

            var created = await DbContext.Ingredients.FindAsync(result.IngredientId);
            Assert.That(created, Is.Not.Null);
            Assert.That(created!.Name, Is.EqualTo("New Ingredient"));
        }

        [Test]
        public async Task Handle_WithInvalidName_ShouldReturnValidationFailure()
        {            
            var command = new CreateIngredientCommand
            {
                Name = "",
                Description = "Valid description."
            };            

            Assert.ThrowsAsync<BadRequestException>(async () =>
                await Mediator.Send(command, CancellationToken.None));            
        }

        [Test]
        public async Task Handle_WithInvalidDescription_ShouldReturnValidationFailure()
        {            
            var command = new CreateIngredientCommand
            {
                Name = "Valid Name",
                Description = ""
            };

            Assert.ThrowsAsync<BadRequestException>(async () =>
                await Mediator.Send(command, CancellationToken.None));
        }

        [Test]
        public async Task Handle_WithoutUsageComment_ShouldCreateIngredient()
        {
            var handler = new CreateIngredientHandler(DbContext);
            var command = new CreateIngredientCommand
            {
                Name = "Simple Ingredient",
                Description = "No usage comment.",
                Comment = null
            };

            var result = await handler.Handle(command, CancellationToken.None);
            
            var created = await DbContext.Ingredients.FindAsync(result.IngredientId);
            Assert.That(created, Is.Not.Null);
            Assert.That(created!.Comment, Is.Null);
        }
    }
}
