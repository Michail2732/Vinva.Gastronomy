using Vinva.Gastronomy.Common.Infrastructure.Exceptions;

using Vinva.Gastronomy.Recipes.Application.Common;
using Vinva.Gastronomy.Recipes.Application.Usecases.Categories.CreateCategory;

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
            
            Assert.That(result.CategoryId, Is.Not.EqualTo(Guid.Empty));

            var created = await DbContext.Categories.FindAsync(result.CategoryId);
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
            
            Assert.That(result.CategoryId, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public async Task Handle_WithEmptyName_ShouldReturnValidationFailure()
        {            
            var command = new CreateCategoryCommand
            {
                Name = "",
                Description = "Valid description.",
                Type = CategoryDtoType.Recipe
            };

            Assert.ThrowsAsync<BadRequestException>(async () =>
                await Mediator.Send(command, CancellationToken.None));
        }

        [Test]
        public async Task Handle_WithEmptyDescription_ShouldReturnValidationFailure()
        {            
            var command = new CreateCategoryCommand
            {
                Name = "Valid Name",
                Description = "",
                Type = CategoryDtoType.Recipe
            };

            Assert.ThrowsAsync<BadRequestException>(async () =>
                await Mediator.Send(command, CancellationToken.None));            
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
            
            var created = await DbContext.Categories.FindAsync(result.CategoryId);
            Assert.That(created, Is.Not.Null);
            Assert.That(created!.Comment, Is.Null);
        }
    }
}
