using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Application.IngredientUsecases.GetIngredientById;

namespace Vinva.Gastronomy.Recipes.Tests.Application
{
    [TestFixture]
    public class GetIngredientByIdHandlerTests : BaseApplicationTests
    {
        private static readonly Guid ExistingIngredientId = Guid.Parse("44444444-4444-4444-4444-444444444441");

        [Test]
        public async Task Handle_WithExistingIngredientId_ShouldReturnIngredient()
        {
            var handler = new GetIngredientByIdHandler(DbContext);
            var request = new GetIngredientByIdRequest { IngredientId = ExistingIngredientId };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Ingredient, Is.Not.Null);
            Assert.That(result.Value.Ingredient.Id, Is.EqualTo(ExistingIngredientId));
            Assert.That(result.Value.Ingredient.Name, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task Handle_WithNonExistentIngredientId_ShouldReturnFailure()
        {
            var handler = new GetIngredientByIdHandler(DbContext);
            var request = new GetIngredientByIdRequest { IngredientId = Guid.NewGuid() };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error.Code, Is.EqualTo(RecipesApplicationErrors.IngredientNotFound.Code));
        }

        [Test]
        public async Task Handle_ShouldReturnIngredientWithCategoriesWhenPresent()
        {
            var handler = new GetIngredientByIdHandler(DbContext);
            var request = new GetIngredientByIdRequest { IngredientId = ExistingIngredientId };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Ingredient.Categories, Is.Not.Null);
        }

        [Test]
        public async Task Handle_WithAnotherExistingId_ShouldReturnCorrectIngredient()
        {
            var cheeseId = Guid.Parse("44444444-4444-4444-4444-444444444445");
            var handler = new GetIngredientByIdHandler(DbContext);
            var request = new GetIngredientByIdRequest { IngredientId = cheeseId };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Ingredient.Id, Is.EqualTo(cheeseId));
            Assert.That(result.Value.Ingredient.Name, Is.Not.Null.And.Not.Empty);
        }
    }
}
