using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Application.GetRecipeByCategory
{
    internal sealed class CategoryFilterExpressionBuilder
    {
        private readonly MethodInfo _anyMethod;
        private readonly MethodInfo _whereMethod;
        private readonly MethodInfo _countMethod;
        private readonly MethodInfo _containsMethod;

        public CategoryFilterExpressionBuilder()
        {            
            _anyMethod = typeof(Enumerable).GetMethods()
                .First(a => a.Name == "Any" && a.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(Category));

            _containsMethod = typeof(List<Guid>).GetMethod("Contains", new[] { typeof(Guid) })
                ?? throw new Exception("Not found Contains method info in List<Guid>");

            _whereMethod = typeof(Enumerable).GetMethods()
                .First(a => a.Name == "Where" && a.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(Category));

            _countMethod = typeof(Enumerable).GetMethods()
                .First(a => a.Name == "Count" && a.GetParameters().Length == 1)
                .MakeGenericMethod(typeof(Category));
        }

        public Expression<Func<Recipe, bool>> CreateExpression(GetRecipeByCategoryRequest request)
        {
            var exprRecipeParam = Expression.Parameter(typeof(Recipe), "re");

            Expression resultExpr = default!;

            if (request.Include?.Any() == true)
            {
                var listOfUniqueIncludeIds = request.Include.Distinct().ToList();
                resultExpr = BuildCondition(exprRecipeParam, listOfUniqueIncludeIds, false, request.IncludeLogicAnd);
            }       
            
            if (request.Exclude?.Any() == true)
            {
                var listOfUniqueExcludeIds = request.Exclude.Distinct().ToList();
                var excludeExpr = BuildCondition(exprRecipeParam, listOfUniqueExcludeIds, true, false);
                resultExpr = resultExpr switch
                {                    
                    not null => Expression.AndAlso(resultExpr, excludeExpr),
                    _ => excludeExpr,
                };
            }

            if (resultExpr == null)
                return a => true;
            return Expression.Lambda<Func<Recipe, bool>>(resultExpr, exprRecipeParam);
        }

        private Expression BuildCondition(ParameterExpression exprRecipeParam, List<Guid> categoryIds, bool isExclude, bool isAndLogic)
        {            
            var exprCategoryProp = Expression.Property(exprRecipeParam, nameof(Recipe.Categories));

            var exprCategoryParam = Expression.Parameter(typeof(Category), "b");

            var exprCategoryIdProp = Expression.Property(exprCategoryParam, nameof(Category.Id));

            var exprContainsCall = Expression.Call(
                            Expression.Constant(categoryIds),
                            _containsMethod,
                            exprCategoryIdProp);

            var exprInnerLambda = Expression.Lambda(exprContainsCall, exprCategoryParam);
            if (isExclude)
            {
                return Expression.Not(Expression.Call(null, _anyMethod, exprCategoryProp, exprInnerLambda));
            }
            else if (isAndLogic)
            {
                var exprWhereCall = Expression.Call(null, _whereMethod, exprCategoryProp, exprInnerLambda);
                var exprCountCall = Expression.Call(null, _countMethod, exprWhereCall);
                return Expression.Equal(exprCountCall, Expression.Constant(categoryIds.Count));
            }
            else
            {                                
                return Expression.Call(null, _anyMethod, exprCategoryProp, exprInnerLambda);
            }                
        }        

    }
}
