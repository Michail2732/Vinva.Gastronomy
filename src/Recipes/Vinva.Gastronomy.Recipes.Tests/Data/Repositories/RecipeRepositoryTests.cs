using Ardalis.Specification;
using Vinva.Gastronomy.Common.Infrastructure.EntityFramework;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Infrastructure.Data.EntityFramework;
using Vinva.Gastronomy.Recipes.Infrastructure.Data.Specifications;

namespace Vinva.Gastronomy.Recipes.Tests.Data.Repositories
{
    [TestFixture(TestName="Тест репозитория Recipes")]
    public class RecipeRepositoryTests : BaseRepositoryTests
    {                
        [Test]
        public async Task FirstOrDefaultAsync_SpecWithIncludeAll_OneRecipeAsync()
        {
            var repository = BuildRepository();
            var recipeIdStr = _configuration["FirstRecipeId"];

            Assert.IsNotEmpty(recipeIdStr);

            var recipeId = Guid.Parse(recipeIdStr!);
            var spec = new Specification<Recipe>();
            spec.Query.Where(a => a.Id == recipeId);
            var recipe = await repository.FirstOrDefaultAsync(spec.IncludeAllDependencies());
            
            Assert.IsNotNull(recipe);
            Assert.That(recipe?.Ingredients.Count, Is.EqualTo(5));
            Assert.That(recipe?.Categories.Count, Is.EqualTo(2));
            Assert.That(recipe?.Steps.Count, Is.EqualTo(3));           
        }

        [Test]
        public async Task FirstOrDefaultAsync_SpecWithoutIncludeAll_OneRecipeAsync()
        {
            var repository = BuildRepository();
            var recipeIdStr = _configuration["FirstRecipeId"];

            Assert.IsNotEmpty(recipeIdStr);

            var recipeId = Guid.Parse(recipeIdStr!);
            var spec = new Specification<Recipe>();
            spec.Query.Where(a => a.Id == recipeId);
            var recipe = await repository.FirstOrDefaultAsync(spec);

            Assert.IsNotNull(recipe);
            Assert.That(recipe?.Ingredients.Count, Is.EqualTo(0));
            Assert.That(recipe?.Categories.Count, Is.EqualTo(0));
            Assert.That(recipe?.Steps.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task AddAsync_RecipeWithoutIncludes_OneRecipeAsync()
        {
            var repository = BuildRepository();

            var comment = "test comment";
            var cookingComment = "test CookingComment";
            var ingredientComment = "test IngredientComment";
            var storageComment = "test StorageComment";
            var usageComment = "test UsageComment";
            var cookingTime = TimeSpan.FromMinutes(5);
            var name = "test name";
            var description = "test description";

            var recipe = new Recipe(name, description)
            {
                Comment = comment,
                CookingComment = cookingComment,
                IngredientComment = ingredientComment,
                StorageComment = storageComment,
                UsageComment = usageComment,
                CookingTime = cookingTime
            };
                        
            var result = await repository.AddAsync(recipe);
            var spec = new Specification<Recipe>();
            spec.Query.Where(a => a.Id == recipe.Id);
            recipe = await repository.FirstOrDefaultAsync(spec.IncludeAllDependencies());

            Assert.IsNotNull(recipe);
            Assert.That(recipe.Name, Is.EqualTo(name));
            Assert.That(recipe.Description, Is.EqualTo(description));
            Assert.That(recipe.Comment, Is.EqualTo(comment));
            Assert.That(recipe.CookingComment, Is.EqualTo(cookingComment));
            Assert.That(recipe.IngredientComment, Is.EqualTo(ingredientComment));
            Assert.That(recipe.StorageComment, Is.EqualTo(storageComment));
            Assert.That(recipe.UsageComment, Is.EqualTo(usageComment));
            Assert.That(recipe.CookingTime, Is.EqualTo(cookingTime));            
            Assert.That(recipe?.Ingredients.Count, Is.EqualTo(0));
            Assert.That(recipe?.Categories.Count, Is.EqualTo(0));
            Assert.That(recipe?.Steps.Count, Is.EqualTo(0));
        }


        private Repository<RecipeDbContext, Recipe> BuildRepository() => new Repository<RecipeDbContext, Recipe>(_recipeDbContext);
    }
}