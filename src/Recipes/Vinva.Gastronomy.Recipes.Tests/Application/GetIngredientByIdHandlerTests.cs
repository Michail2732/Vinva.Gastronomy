using Vinva.Gastronomy.Common.Exceptions;

using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Application.Usecases.Ingredients.GetById;

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

            
            Assert.That(result.Ingredient, Is.Not.Null);
            Assert.That(result.Ingredient.Id, Is.EqualTo(ExistingIngredientId));
            Assert.That(result.Ingredient.Name, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task Handle_WithNonExistentIngredientId_ShouldReturnFailure()
        {
            var handler = new GetIngredientByIdHandler(DbContext);
            var request = new GetIngredientByIdRequest { IngredientId = Guid.NewGuid() };

            Assert.ThrowsAsync<NotFoundException>(async () =>
               await handler.Handle(request, CancellationToken.None));
        }

        [Test]
        public async Task Handle_ShouldReturnIngredientWithCategoriesWhenPresent()
        {
            var handler = new GetIngredientByIdHandler(DbContext);
            var request = new GetIngredientByIdRequest { IngredientId = ExistingIngredientId };

            var result = await handler.Handle(request, CancellationToken.None);

           
            Assert.That(result.Ingredient.Categories, Is.Not.Null);
        }

        [Test]
        public async Task Handle_WithAnotherExistingId_ShouldReturnCorrectIngredient()
        {
            var cheeseId = Guid.Parse("44444444-4444-4444-4444-444444444445");
            var handler = new GetIngredientByIdHandler(DbContext);
            var request = new GetIngredientByIdRequest { IngredientId = cheeseId };

            var result = await handler.Handle(request, CancellationToken.None);

            
            Assert.That(result.Ingredient.Id, Is.EqualTo(cheeseId));
            Assert.That(result.Ingredient.Name, Is.Not.Null.And.Not.Empty);
        }
    }
}
