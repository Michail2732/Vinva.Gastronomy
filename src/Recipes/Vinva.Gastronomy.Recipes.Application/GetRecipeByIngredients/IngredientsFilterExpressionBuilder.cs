using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Application.GetRecipeByIngredients
{
    internal sealed class IngredientsFilterExpressionBuilder
    {
        private readonly MethodInfo _anyMethod;
        private readonly MethodInfo _whereMethod;
        private readonly MethodInfo _countMethod;
        private readonly MethodInfo _containsMethod;

        public IngredientsFilterExpressionBuilder()
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
                .First(a => a.Name == "Count" && a.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(Category));
        }

        public Expression<Func<Recipe, bool>> CreateExpression(GetRecipeByIngredientsRequest request)
        {
            var exprRecipeParam = Expression.Parameter(typeof(Recipe), "re");

            Expression resultExpr = default!;

            if (request.Include?.Any() == true)
            {
                resultExpr = BuildCondition(exprRecipeParam, request.Include.Distinct().ToList(), false, request.IncludeLogicAnd);
            }       
            
            if (request.Exclude?.Any() == true)
            {
                var excludeExpr = BuildCondition(exprRecipeParam, request.Exclude.Distinct().ToList(), true, false);
                resultExpr = resultExpr switch
                {                    
                    not null => Expression.AndAlso(resultExpr, excludeExpr),
                    _ => excludeExpr,
                };
            }

            return Expression.Lambda<Func<Recipe, bool>>(resultExpr, exprRecipeParam);
        }

        private Expression BuildCondition(ParameterExpression exprRecipeParam, List<Guid> ingredientIds, bool isExclude, bool isAndLogic)
        {            
            var exprIngredientsProp = Expression.Property(exprRecipeParam, nameof(Recipe.Ingredients));

            var exprRecipeIngredientParam = Expression.Parameter(typeof(RecipeIngredient), "b");

            var exprIngredientIdProp = Expression.Property(exprRecipeIngredientParam, nameof(RecipeIngredient.IngredientId));

            var exprContainsCall = Expression.Call(
                            Expression.Constant(ingredientIds),
                            _containsMethod,
                            exprIngredientIdProp);

            var exprInnerLambda = Expression.Lambda(exprContainsCall, exprRecipeIngredientParam);
            if (isExclude)
            {
                return Expression.Not(Expression.Call(null, _anyMethod, exprIngredientsProp, exprInnerLambda));
            }
            else if (isAndLogic)
            {
                var exprWhereCall = Expression.Call(null, _whereMethod, exprIngredientsProp, exprInnerLambda);
                var exprCountCall = Expression.Call(null, _countMethod, exprWhereCall);
                return Expression.Equal(exprCountCall, Expression.Constant(ingredientIds.Count));
            }
            else
            {                                
                return Expression.Call(null, _anyMethod, exprIngredientsProp, exprInnerLambda);
            }                
        }        

    }
}
