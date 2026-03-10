using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Infrastructure.Filters
{
    /// <summary>
    /// Построитель запросов поиска для IQueryable
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public sealed class QueryExpressionBuilder<TEntity>
        where TEntity : Entity
    {

        public IQueryable<TEntity> BuildQuery(IQueryable<TEntity> query,  SearchQuery searchQuery)
        {
            if (searchQuery == null)
                return query;

            // 1. Применяем фильтрацию (Conditions)
            if (searchQuery.Conditions != null && searchQuery.Conditions.Any())
            {
                query = ApplyConditions(query, searchQuery.Conditions);
            }

            // 2. Применяем сортировку
            if (searchQuery.Sort != null)
            {
                query = ApplySorting(query, searchQuery.Sort);
            }

            // 3. Применяем пагинацию
            if (searchQuery.Skip > 0)
            {
                query = query.Skip((int)searchQuery.Skip);
            }

            if (searchQuery.Take > 0)
            {
                query = query.Take(searchQuery.Take);
            }

            return query;
        }

        private IQueryable<TEntity> ApplyConditions(
        IQueryable<TEntity> query,
        List<Condition> conditions)
        {
            if (!conditions.Any())
                return query;

            // Начинаем с первого условия
            var parameter = Expression.Parameter(typeof(TEntity), "entity");
            var firstCondition = conditions[0];
            var expression = BuildConditionExpression(parameter, firstCondition);

            // Комбинируем остальные условия
            for (int i = 1; i < conditions.Count; i++)
            {
                var condition = conditions[i];
                var conditionExpression = BuildConditionExpression(parameter, condition);

                expression = condition.Logic == Logic.And
                    ? Expression.AndAlso(expression, conditionExpression)
                    : Expression.OrElse(expression, conditionExpression);
            }

            var lambda = Expression.Lambda<Func<TEntity, bool>>(expression, parameter);
            return query.Where(lambda);
        }

        private Expression BuildConditionExpression(
        ParameterExpression parameter,
        Condition condition)
        {
            // Получаем свойство по имени (поддерживает вложенные свойства через ".")
            var propertyExpression = GetPropertyExpression(parameter, condition.Field);

            if (propertyExpression == null)
                throw new ArgumentException($"Property '{condition.Field}' not found on type {typeof(TEntity).Name}");

            var propertyType = propertyExpression.Type;
            var value = ConvertValue(condition.Value, propertyType);

            // Создаем константу со значением
            var constant = Expression.Constant(value, propertyType);

            // Приводим значение, если нужно (для nullable типов)
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var nonNullableType = Nullable.GetUnderlyingType(propertyType);
                if (nonNullableType != null && value != null)
                {
                    // Создаем выражение для доступа к Value у Nullable<T>
                    var valueProperty = propertyType.GetProperty("Value");
                    var hasValueProperty = propertyType.GetProperty("HasValue");

                    if (condition.Operator == Operator.Equals && value == null)
                    {
                        // Проверка на null: entity.Property == null
                        return Expression.Equal(
                            propertyExpression,
                            Expression.Constant(null, propertyType));
                    }

                    // Для сравнений с nullable типами
                    var convertedConstant = Expression.Constant(value, nonNullableType);
                    var propertyValue = Expression.Property(propertyExpression, valueProperty!);

                    return BuildComparisonExpression(propertyValue, convertedConstant, condition.Operator);
                }
            }

            return BuildComparisonExpression(propertyExpression, constant, condition.Operator);
        }

        private Expression BuildStringMethodExpression( Expression left,
                Expression right, Operator op)
        {
            if (left.Type != typeof(string))
                throw new ArgumentException("String operators can only be applied to string properties");

            var methodName = op switch
            {
                Operator.Contains => "Contains",
                Operator.StartWith => "StartsWith",
                Operator.EndWith => "EndsWith",
                _ => throw new NotSupportedException($"Operator {op} is not supported for strings")
            };

            var method = typeof(string).GetMethod(methodName, new[] { typeof(string) });
            if (method == null)
                throw new InvalidOperationException($"Method {methodName} not found");

            return Expression.Call(left, method, right);
        }

        private Expression? GetPropertyExpression(ParameterExpression parameter, string propertyPath)
        {
            if (string.IsNullOrWhiteSpace(propertyPath))
                return null;

            var properties = propertyPath.Split('.');
            Expression expression = parameter;

            foreach (var property in properties)
            {
                var propertyInfo = expression.Type.GetProperty(property);
                if (propertyInfo == null)
                    return null;

                expression = Expression.Property(expression, propertyInfo);
            }

            return expression;
        }

        private object? ConvertValue(object value, Type targetType)
        {
            if (value == null)
                return null;

            // Если целевой тип - nullable, получаем базовый тип
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                targetType = Nullable.GetUnderlyingType(targetType)!;
            }

            // Если типы совпадают
            if (targetType.IsInstanceOfType(value))
                return value;

            // Конвертируем значение
            try
            {
                // Для enum
                if (targetType.IsEnum)
                {
                    if (value is string stringValue)
                        return Enum.Parse(targetType, stringValue);

                    return Enum.ToObject(targetType, value);
                }

                // Для примитивных типов
                return Convert.ChangeType(value, targetType);
            }
            catch
            {
                return value;
            }
        }

        private Expression BuildComparisonExpression(Expression left,
            Expression right, Operator op)
        {
            switch (op)
            {
                case Operator.Equals:
                    return Expression.Equal(left, right);

                case Operator.NotEquals:
                    return Expression.NotEqual(left, right);

                case Operator.Less:
                    return Expression.LessThan(left, right);

                case Operator.LessOrEqual:
                    return Expression.LessThanOrEqual(left, right);

                case Operator.Larger:
                    return Expression.GreaterThan(left, right);

                case Operator.LargerOrEqual:
                    return Expression.GreaterThanOrEqual(left, right);

                case Operator.Contains:
                case Operator.StartWith:
                case Operator.EndWith:
                    return BuildStringMethodExpression(left, right, op);

                default:
                    throw new NotSupportedException($"Operator {op} is not supported");
            }
        }


        private IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query,
                Sorting sorting)
        {
            if (sorting == null || string.IsNullOrWhiteSpace(sorting.Property))
                return query;

            var parameter = Expression.Parameter(typeof(TEntity), "entity");
            var propertyExpression = GetPropertyExpression(parameter, sorting.Property);

            if (propertyExpression == null)
                throw new ArgumentException($"Property '{sorting.Property}' not found on type {typeof(TEntity).Name}");

            var lambda = Expression.Lambda(propertyExpression, parameter);

            // Используем Expression.Call для создания OrderBy/OrderByDescending
            var methodName = sorting.Direction == SortDirection.Ascending
                ? "OrderBy"
                : "OrderByDescending";

            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(TEntity), propertyExpression.Type);

            return (IQueryable<TEntity>)method.Invoke(null, new object[] { query, lambda })!;
        }


    }
}
