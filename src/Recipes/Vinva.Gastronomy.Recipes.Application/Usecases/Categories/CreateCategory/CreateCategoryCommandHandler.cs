using MediatR;


using Vinva.Gastronomy.Recipes.Application.Common.Map;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Categories.CreateCategory
{
    public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryResponse>
    {                
        private readonly RecipeDbContext _dbContext;

        public CreateCategoryCommandHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));                        
        }

        public async Task<CreateCategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {                        
            var categoryType = Enum.Parse<CategoryType>(Enum.GetName(request.Type)!);
            var newCategory = new Category(request.Name, request.Description, categoryType)
            {
                Comment = request.Comment
            };
            var result = await _dbContext.Categories.AddAsync(newCategory, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);


            return new CreateCategoryResponse
            {
                CategoryId = result.Entity.Id
            };
        }
    }
}