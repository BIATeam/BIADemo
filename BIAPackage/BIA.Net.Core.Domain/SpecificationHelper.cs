// <copyright file="SpecificationHelper.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Specification;

    /// <summary>
    /// The helpers for specifications.
    /// </summary>
    public static class SpecificationHelper
    {

        /// <summary>
        /// Add the specifications of the lazy load.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to filter with.</typeparam>
        /// <param name="specification">The specification to update.</param>
        /// <param name="whereClauses">
        /// The dictionary containing the expressions to access the property to filter on.
        /// </param>
        /// <param name="dto">The lazy DTO.</param>
        /// <returns>The specification updated.</returns>
        public static Specification<TEntity> GetLazyLoad<TEntity, TMatcher>(Specification<TEntity> specification, TMatcher matcher, LazyLoadDto dto)
        where TEntity : class, IEntity, new()
        where TMatcher : BaseEntityMapper<TEntity>, new()
        {
            ExpressionCollection<TEntity> whereClauses = matcher.ExpressionCollection;

            if (dto?.Filters == null)
            {
                return specification;
            }

            Specification<TEntity> globalFilterSpecification = null;

            foreach (var (key, value) in dto.Filters)
            {
                var isGlobal = key.Contains("global|", StringComparison.InvariantCultureIgnoreCase);
                var matchKey = key.Replace("global|", string.Empty, StringComparison.InvariantCultureIgnoreCase);

                try
                {
                    Expression<Func<TEntity, bool>> matchingCriteria = null;


                    if (!whereClauses.ContainsKey(matchKey))
                    {
                        continue;
                    }
                    LambdaExpression expression = whereClauses[matchKey];
                    if (expression == null)
                    {
                        continue;
                    }


                    Type type = expression.ReturnType;


                    matchingCriteria = LazyDynamicFilterExpression<TEntity>(
                        expression,
                        value["matchMode"].ToString(),
                        value["value"].ToString(),
                        type);



                    if (isGlobal)
                    {
                        if (globalFilterSpecification == null)
                        {
                            globalFilterSpecification = new DirectSpecification<TEntity>(matchingCriteria);
                        }
                        else
                        {
                            globalFilterSpecification |= new DirectSpecification<TEntity>(matchingCriteria);
                        }
                    }
                    else
                    {
                        specification &= new DirectSpecification<TEntity>(matchingCriteria);
                    }
                }
                catch (FormatException)
                {
#pragma warning disable S3626 // Jump statements should not be redundant
                    continue;
#pragma warning restore S3626 // Jump statements should not be redundant
                }


            }

            if (globalFilterSpecification != null)
            {
                specification &= globalFilterSpecification;
            }

            return specification;
        }

        /// <summary>
        /// Create the matching criteria dynamically for the given parameter and value.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="expression">The expression used to get the property.</param>
        /// <param name="criteria">The matching criteria.</param>
        /// <param name="value">The value to filter with.</param>
        /// <param name="valueType">The property type.</param>
        /// <returns>The expression created dynamically.</returns>
        private static Expression<Func<TEntity, bool>>
            LazyDynamicFilterExpression<TEntity>(
                LambdaExpression expression, string criteria, string value, Type valueType)
        {
            ConstantExpression valueExpression;
            ParameterExpression parameterExpression = expression.Parameters.FirstOrDefault();
            var expressionBody = expression.Body;

            MethodInfo method;
            Expression binaryExpression;
            var methodToString = expressionBody.Type.GetMethod("ToString", Type.EmptyTypes);

            object valueFormated = TypeDescriptor.GetConverter(expressionBody.Type).ConvertFromString(value);

            switch (criteria.ToLower())
            {
                case "gt":
                    valueExpression = Expression.Constant(valueFormated, expressionBody.Type);
                    binaryExpression = Expression.GreaterThan(expressionBody, valueExpression);
                    break;

                case "lt":
                    valueExpression = Expression.Constant(valueFormated, expressionBody.Type);
                    binaryExpression = Expression.LessThan(expressionBody, valueExpression);
                    break;

                case "equals":
                    valueExpression = Expression.Constant(valueFormated, expressionBody.Type);
                    binaryExpression = Expression.Equal(expressionBody, valueExpression);
                    break;

                case "lte":
                    valueExpression = Expression.Constant(valueFormated, expressionBody.Type);
                    binaryExpression = Expression.LessThanOrEqual(expressionBody, valueExpression);
                    break;

                case "gte":
                    valueExpression = Expression.Constant(valueFormated, expressionBody.Type);
                    binaryExpression = Expression.GreaterThanOrEqual(expressionBody, valueExpression);
                    break;

                case "notequals":
                    valueExpression = Expression.Constant(valueFormated, expressionBody.Type);
                    binaryExpression = Expression.NotEqual(expressionBody, valueExpression);
                    break;

                case "contains":
                    if (IsCollectionType(valueType))
                    {
                        valueExpression = Expression.Constant(valueFormated);
                        method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        ParameterExpression pe = Expression.Parameter(typeof(string), "a");
                        var predicate = Expression.Call(pe, method ?? throw new InvalidOperationException(), valueExpression);
                        var predicateExpr = Expression.Lambda<Func<string, bool>>(predicate, pe);

                        binaryExpression = Expression.Call(typeof(Enumerable), "Any", new[] { typeof(string) }, expressionBody, predicateExpr);
                    }
                    else
                    {
                        valueExpression = Expression.Constant(valueFormated);
                        method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        if (expressionBody.Type != typeof(string))
                        {
                            expressionBody = Expression.Call(expressionBody, methodToString ?? throw new InvalidOperationException());
                        }

                        binaryExpression = Expression.Call(expressionBody, method ?? throw new InvalidOperationException(), valueExpression);
                    }

                    break;

                case "startswith":
                    valueExpression = Expression.Constant(valueFormated);
                    method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                    if (expressionBody.Type != typeof(string))
                    {
                        expressionBody = Expression.Call(expressionBody, methodToString ?? throw new InvalidOperationException());
                    }

                    binaryExpression = Expression.Call(expressionBody, method ?? throw new InvalidOperationException(), valueExpression);
                    break;

                case "endswith":
                    valueExpression = Expression.Constant(valueFormated);
                    method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                    if (expressionBody.Type != typeof(string))
                    {
                        expressionBody = Expression.Call(expressionBody, methodToString ?? throw new InvalidOperationException());
                    }

                    binaryExpression = Expression.Call(expressionBody, method ?? throw new InvalidOperationException(), valueExpression);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(criteria), criteria, null);
            }

            return Expression.Lambda<Func<TEntity, bool>>(binaryExpression, parameterExpression);
        }

        public static bool IsCollectionType(Type valueType)
        {
            return (valueType.Name.Length > 11 && valueType.Name.Substring(0, 11) == "IEnumerable")
                || (valueType.Name.Length > 18 && valueType.Name.Substring(0, 18) == "IOrderedEnumerable");
        }

        /// <summary>
        /// Convert the filter string to the type of the property to filter on. This method needs to
        /// be expanded to include all appropriate use cases when needed.
        /// </summary>
        /// <param name="value">The filter string.</param>
        /// <param name="type">The property type.</param>
        /// <returns>The filter in the good type.</returns>
        private static object AsType(string value, Type type)
        {
            var filter = value;
            if (value.StartsWith("'") && value.EndsWith("'"))
            {
                filter = value.Substring(1, value.Length - 2);
            }

            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                filter = value.Substring(1, value.Length - 2);
            }

            if (type == typeof(string))
            {
                return filter;
            }

            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return DateTime.Parse(filter);
            }

            if (type == typeof(int) || type == typeof(int?))
            {
                return int.Parse(filter);
            }

            if (type == typeof(long) || type == typeof(long?))
            {
                return long.Parse(filter);
            }

            if (type == typeof(short) || type == typeof(short?))
            {
                return short.Parse(filter);
            }

            if (type == typeof(byte) || type == typeof(byte?))
            {
                return byte.Parse(filter);
            }

            if (type == typeof(bool) || type == typeof(bool?))
            {
                return bool.Parse(filter);
            }

            throw new ArgumentException("NSG.PrimeNG.LazyLoading.Helpers.AsType: " +
                                        "A filter was attempted for a field with value '" + value + "' and type '" +
                                        type + "' however this type is not currently supported");
        }
    }
}