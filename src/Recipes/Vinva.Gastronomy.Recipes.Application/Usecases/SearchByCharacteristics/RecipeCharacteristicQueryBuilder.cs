using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Vinva.Gastronomy.Common.Models.Search;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Persistence.Models;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.SearchByCharacteristics
{
    internal static class RecipeCharacteristicQueryBuilder
    {
        public static IQueryable<Recipe> Apply(
            IQueryable<Recipe> recipes,
            IQueryable<CharacteristicRecord> records,
            IReadOnlyList<CharacteristicFilterDto> filters)
        {
            if (filters.Count == 0)
                return recipes;

            var recipe = Expression.Parameter(typeof(Recipe), "recipe");
            Expression? combined = null;

            foreach (var filter in filters)
            {
                var current = BuildExistsExpression(recipe, records, filter);
                combined = combined is null
                    ? current
                    : filter.Logic == Logic.And
                        ? Expression.AndAlso(combined, current)
                        : Expression.OrElse(combined, current);
            }

            return recipes.Where(Expression.Lambda<Func<Recipe, bool>>(combined!, recipe));
        }

        private static Expression BuildExistsExpression(
            ParameterExpression recipe,
            IQueryable<CharacteristicRecord> records,
            CharacteristicFilterDto filter)
        {
            var record = Expression.Parameter(typeof(CharacteristicRecord), "record");
            var match = Expression.AndAlso(
                Expression.Equal(
                    Expression.Property(record, nameof(CharacteristicRecord.RecipeId)),
                    Expression.Property(recipe, nameof(Recipe.Id))),
                Expression.AndAlso(
                    Expression.Equal(
                        Expression.Property(record, nameof(CharacteristicRecord.Name)),
                        Expression.Constant(filter.Name)),
                    BuildValueExpression(record, filter)));

            var any = typeof(Queryable).GetMethods()
                .Single(method => method.Name == nameof(Queryable.Any) && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(CharacteristicRecord));

            return Expression.Call(
                any,
                records.Expression,
                Expression.Lambda<Func<CharacteristicRecord, bool>>(match, record));
        }

        private static Expression BuildValueExpression(
            ParameterExpression record,
            CharacteristicFilterDto filter)
        {
            return filter.Type switch
            {
                CharacteristicType.String => BuildStringExpression(record, filter),
                CharacteristicType.Number => BuildNumberExpression(record, filter),
                CharacteristicType.Range => BuildRangeExpression(record, filter),
                CharacteristicType.Array => BuildArrayExpression(record, filter),
                _ => throw new ArgumentOutOfRangeException(nameof(filter.Type), filter.Type, "Unsupported characteristic type.")
            };
        }

        private static Expression BuildStringExpression(
            ParameterExpression record,
            CharacteristicFilterDto filter)
        {
            EnsureValueKind(filter, JsonValueKind.String);
            EnsureOperator(filter.Operator, Operator.Equals, Operator.NotEquals, Operator.Contains, Operator.StartWith, Operator.EndWith);

            var property = Expression.Property(record, nameof(CharacteristicRecord.StringValue));
            var value = Expression.Constant(filter.Value.GetString()!);
            Expression comparison = filter.Operator switch
            {
                Operator.Equals => Expression.Equal(property, value),
                Operator.NotEquals => Expression.NotEqual(property, value),
                Operator.Contains => Expression.Call(property, nameof(string.Contains), Type.EmptyTypes, value),
                Operator.StartWith => Expression.Call(property, nameof(string.StartsWith), Type.EmptyTypes, value),
                Operator.EndWith => Expression.Call(property, nameof(string.EndsWith), Type.EmptyTypes, value),
                _ => throw new ArgumentException($"Operator {filter.Operator} is not supported for strings.")
            };

            return WithType(record, CharacteristicType.String, property, comparison);
        }

        private static Expression BuildNumberExpression(
            ParameterExpression record,
            CharacteristicFilterDto filter)
        {
            EnsureValueKind(filter, JsonValueKind.Number);
            EnsureOperator(filter.Operator, Operator.Equals, Operator.NotEquals, Operator.Less, Operator.LessOrEqual, Operator.Larger, Operator.LargerOrEqual);

            var property = Expression.Property(record, nameof(CharacteristicRecord.NumberValue));
            var comparison = BuildNullableComparison(property, filter.Value.GetDouble(), filter.Operator);
            return WithType(record, CharacteristicType.Number, property, comparison);
        }

        private static Expression BuildRangeExpression(
            ParameterExpression record,
            CharacteristicFilterDto filter)
        {
            var value = filter.Value;
            var rangeMin = Expression.Property(record, nameof(CharacteristicRecord.RangeMin));
            var rangeMax = Expression.Property(record, nameof(CharacteristicRecord.RangeMax));

            if (value.ValueKind == JsonValueKind.Number)
            {
                var number = value.GetDouble();
                var comparison = filter.Operator switch
                {
                    Operator.Equals => Expression.AndAlso(
                        BuildNullableComparison(rangeMin, number, Operator.LessOrEqual),
                        BuildNullableComparison(rangeMax, number, Operator.LargerOrEqual)),
                    Operator.NotEquals => Expression.OrElse(
                        BuildNullableComparison(rangeMin, number, Operator.Larger),
                        BuildNullableComparison(rangeMax, number, Operator.Less)),
                    Operator.Less => BuildNullableComparison(rangeMax, number, Operator.Less),
                    Operator.LessOrEqual => BuildNullableComparison(rangeMax, number, Operator.LessOrEqual),
                    Operator.Larger => BuildNullableComparison(rangeMin, number, Operator.Larger),
                    Operator.LargerOrEqual => BuildNullableComparison(rangeMin, number, Operator.LargerOrEqual),
                    _ => throw new ArgumentException($"Operator {filter.Operator} is not supported for ranges.")
                };
                return WithType(record, CharacteristicType.Range, rangeMin, comparison);
            }

            if (value.ValueKind != JsonValueKind.Object
                || !value.TryGetProperty("Min", out var minValue)
                || !value.TryGetProperty("Max", out var maxValue)
                || !minValue.TryGetDouble(out var minNumber)
                || !maxValue.TryGetDouble(out var maxNumber))
                throw new ArgumentException("Range value must be a number or an object with numeric Min and Max properties.");

            EnsureOperator(filter.Operator, Operator.Equals, Operator.NotEquals);
            var exact = Expression.AndAlso(
                BuildNullableComparison(rangeMin, minNumber, Operator.Equals),
                BuildNullableComparison(rangeMax, maxNumber, Operator.Equals));
            return WithType(record, CharacteristicType.Range, rangeMin,
                filter.Operator == Operator.Equals ? exact : Expression.Not(exact));
        }

        private static Expression BuildArrayExpression(
            ParameterExpression record,
            CharacteristicFilterDto filter)
        {
            var property = Expression.Property(record, nameof(CharacteristicRecord.ArrayItems));
            Expression comparison;

            if (filter.Operator == Operator.Contains)
            {
                EnsureValueKind(filter, JsonValueKind.String);
                comparison = Expression.Call(
                    typeof(Enumerable),
                    nameof(Enumerable.Contains),
                    new[] { typeof(string) },
                    property,
                    Expression.Constant(filter.Value.GetString()!));
            }
            else
            {
                EnsureValueKind(filter, JsonValueKind.Array);
                EnsureOperator(filter.Operator, Operator.Equals, Operator.NotEquals);
                var values = filter.Value.EnumerateArray()
                    .Select(item => item.GetString() ?? throw new ArgumentException("Array values must be strings."))
                    .ToArray();
                var exact = Expression.Call(
                    typeof(Enumerable),
                    nameof(Enumerable.SequenceEqual),
                    new[] { typeof(string) },
                    property,
                    Expression.Constant(values));
                comparison = filter.Operator == Operator.Equals ? exact : Expression.Not(exact);
            }

            return WithType(record, CharacteristicType.Array, property,
                Expression.AndAlso(Expression.NotEqual(property, Expression.Constant(null, typeof(string[]))), comparison));
        }

        private static Expression WithType(
            ParameterExpression record,
            CharacteristicType type,
            MemberExpression value,
            Expression comparison)
        {
            return Expression.AndAlso(
                Expression.Equal(Expression.Property(record, nameof(CharacteristicRecord.Type)), Expression.Constant(type)),
                Expression.AndAlso(Expression.NotEqual(value, Expression.Constant(null, value.Type)), comparison));
        }

        private static Expression BuildNullableComparison(
            MemberExpression property,
            double value,
            Operator op)
        {
            var actual = Expression.Property(property, nameof(Nullable<double>.Value));
            var comparison = op switch
            {
                Operator.Equals => Expression.Equal(actual, Expression.Constant(value)),
                Operator.NotEquals => Expression.NotEqual(actual, Expression.Constant(value)),
                Operator.Less => Expression.LessThan(actual, Expression.Constant(value)),
                Operator.LessOrEqual => Expression.LessThanOrEqual(actual, Expression.Constant(value)),
                Operator.Larger => Expression.GreaterThan(actual, Expression.Constant(value)),
                Operator.LargerOrEqual => Expression.GreaterThanOrEqual(actual, Expression.Constant(value)),
                _ => throw new ArgumentException($"Operator {op} is not supported for numbers.")
            };
            return Expression.AndAlso(Expression.Property(property, nameof(Nullable<double>.HasValue)), comparison);
        }

        private static void EnsureValueKind(CharacteristicFilterDto filter, JsonValueKind kind)
        {
            if (filter.Value.ValueKind != kind)
                throw new ArgumentException($"Value for {filter.Type} characteristic must be {kind}.");
        }

        private static void EnsureOperator(Operator actual, params Operator[] allowed)
        {
            if (!allowed.Contains(actual))
                throw new ArgumentException($"Operator {actual} is not supported for this characteristic type.");
        }
    }
}
