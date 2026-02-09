using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Common.Infrastructure.Validations;
using Vinva.Gastronomy.Recipes.Application.Common.Map;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.CreateCategory
{
    public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<CreateCategoryResponse>>
    {
        private readonly RecipeApplicationMapper _mapper;
        private readonly CreateCategoryCommandValidator _validator;
        private readonly RecipeDbContext _dbContext;

        public CreateCategoryCommandHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = new RecipeApplicationMapper();
            _validator = new CreateCategoryCommandValidator();
        }

        public async Task<Result<CreateCategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {            
            var validResult = _validator.Validate(request);
            if (!validResult.IsValid)
                return validResult.HandleValidationErrors<CreateCategoryResponse>();

            var categoryType = Enum.Parse<CategoryType>(Enum.GetName(request.Type)!);
            var newCategory = new Category(request.Name, request.Description, categoryType)
            {
                Comment = request.Comment
            };
            var result = await _dbContext.Categories.AddAsync(newCategory, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);


            return Result.Success(new CreateCategoryResponse
            {
                CategoryId = result.Entity.Id
            });
        }
    }
}