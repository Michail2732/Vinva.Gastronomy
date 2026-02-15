using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Recipes.Application.CategoryUsecases.CreateCategory;
using Vinva.Gastronomy.Recipes.Application.Common;

namespace Vinva.Gastronomy.Recipes.Tests.Application
{
    [TestFixture]
    public class CreateCategoryCommandHandlerTests : BaseApplicationTests
    {
        [Test]
        public async Task Handle_WithValidCommand_ShouldCreateCategoryAndReturnSuccess()
        {
            var handler = new CreateCategoryCommandHandler(DbContext);
            var command = new CreateCategoryCommand
            {
                Name = "New Category",
                Description = "Category description.",
                Type = CategoryDtoType.Recipe,
                Comment = "Optional comment."
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.CategoryId, Is.Not.EqualTo(Guid.Empty));

            var created = await DbContext.Categories.FindAsync(result.Value.CategoryId);
            Assert.That(created, Is.Not.Null);
            Assert.That(created!.Name, Is.EqualTo("New Category"));
        }

        [Test]
        public async Task Handle_WithIngredientType_ShouldCreateIngredientCategory()
        {
            var handler = new CreateCategoryCommandHandler(DbContext);
            var command = new CreateCategoryCommand
            {
                Name = "New Ingredient Category",
                Description = "For ingredients.",
                Type = CategoryDtoType.Ingredient
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.CategoryId, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public async Task Handle_WithEmptyName_ShouldReturnValidationFailure()
        {
            var handler = new CreateCategoryCommandHandler(DbContext);
            var command = new CreateCategoryCommand
            {
                Name = "",
                Description = "Valid description.",
                Type = CategoryDtoType.Recipe
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public async Task Handle_WithEmptyDescription_ShouldReturnValidationFailure()
        {
            var handler = new CreateCategoryCommandHandler(DbContext);
            var command = new CreateCategoryCommand
            {
                Name = "Valid Name",
                Description = "",
                Type = CategoryDtoType.Recipe
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public async Task Handle_WithNullComment_ShouldCreateCategory()
        {
            var handler = new CreateCategoryCommandHandler(DbContext);
            var command = new CreateCategoryCommand
            {
                Name = "Category No Comment",
                Description = "Description.",
                Type = CategoryDtoType.Recipe,
                Comment = null
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            var created = await DbContext.Categories.FindAsync(result.Value.CategoryId);
            Assert.That(created, Is.Not.Null);
            Assert.That(created!.Comment, Is.Null);
        }
    }
}
