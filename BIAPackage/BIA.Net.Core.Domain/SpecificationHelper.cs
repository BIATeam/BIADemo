// <copyright file="SpecificationHelper.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>


namespace BIA.Net.Core.Domain
{
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Specification;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text.Json;

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
        public static Specification<TEntity> GetLazyLoad<TEntity, TKey, TMapper>(Specification<TEntity> specification, TMapper matcher, LazyLoadDto dto)
        where TEntity : class, IEntity<TKey>, new()
        where TMapper : BaseEntityMapper<TEntity>, new()
        {
            ExpressionCollection<TEntity> whereClauses = matcher.ExpressionCollection;

            if (dto?.Filters == null)
            {
                return specification;
            }

            Specification<TEntity> globalFilterSpecification = null;

            foreach (var (key, json) in dto.Filters)
            {
                if (json.ValueKind == System.Text.Json.JsonValueKind.Array)
                {
                    var values = JsonSerializer.Deserialize<Dictionary<string, object>[]>(json);
                    Specification<TEntity> ruleSpecification = null;
                    foreach (var value in values)
                    {
                        AddSpecByValue<TEntity, TKey>(ref ruleSpecification, whereClauses, ref globalFilterSpecification, key, value);
                    }
                    specification &= ruleSpecification;
                }
                else
                {
                    var value = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                    AddSpecByValue<TEntity, TKey>(ref specification, whereClauses, ref globalFilterSpecification, key, value);
                }

            }

            if (globalFilterSpecification != null)
            {
                specification &= globalFilterSpecification;
            }

            return specification;
        }

        private static void AddSpecByValue<TEntity, TKey>(ref Specification<TEntity> specification, ExpressionCollection<TEntity> whereClauses, ref Specification<TEntity> globalFilterSpecification, string key, Dictionary<string, object> value) where TEntity : class, IEntity<TKey>, new()
        {
            if (value["value"] == null)
            {
                return;
            }

            var isGlobal = key.Contains("global|", StringComparison.InvariantCultureIgnoreCase);
            var matchKey = key.Replace("global|", string.Empty, StringComparison.InvariantCultureIgnoreCase);

            try
            {
                Expression<Func<TEntity, bool>> matchingCriteria = null;
                if (!whereClauses.ContainsKey(matchKey))
                {
                    return;
                }
                LambdaExpression expression = whereClauses[matchKey];
                if (expression == null)
                {
                    return;
                }

                matchingCriteria = LazyDynamicFilterExpression<TEntity>(
                    expression,
                    value["matchMode"].ToString(),
                    value["value"].ToString());

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
                    if (specification == null)
                    {
                        specification = new DirectSpecification<TEntity>(matchingCriteria);
                    }
                    else
                    {
                        if (value.ContainsKey("operator") && value["operator"]?.ToString() == "or")
                        {
                            specification |= new DirectSpecification<TEntity>(matchingCriteria);
                        }
                        else
                        {
                            specification &= new DirectSpecification<TEntity>(matchingCriteria);
                        }
                    }
                }
            }
            catch (FormatException)
            {
#pragma warning disable S3626 // Jump statements should not be redundant
                return;
#pragma warning restore S3626 // Jump statements should not be redundant
            }
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
                LambdaExpression expression, string criteria, string value)
                    where TEntity : class
        {
            ConstantExpression valueExpression;
            ParameterExpression parameterExpression = expression.Parameters.FirstOrDefault();
            var expressionBody = expression.Body;

            MethodInfo method;
            Expression binaryExpression;
            var methodToString = expressionBody.Type.GetMethod("ToString", Type.EmptyTypes);

            switch (criteria.ToLower())
            {
                case "gt":
                case "lt":
                case "equals":
                case "lte":
                case "gte":
                case "notequals":
                case "dateis":
                case "dateisnot":
                case "datebefore":
                case "dateafter":
                    try 
                    {
                        //object valueFormated = AsType(value, expressionBody.Type);
                        object valueFormated = TypeDescriptor.GetConverter(expressionBody.Type).ConvertFromString(value);
                        switch (criteria.ToLower())
                        {
                            case "gt":
                            case "dateafter":
                                valueExpression = Expression.Constant(valueFormated, expressionBody.Type);
                                binaryExpression = Expression.GreaterThan(expressionBody, valueExpression);
                                break;

                            case "lt":
                            case "datebefore":
                                valueExpression = Expression.Constant(valueFormated, expressionBody.Type);
                                binaryExpression = Expression.LessThan(expressionBody, valueExpression);
                                break;

                            case "equals":
                            case "dateis":
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
                            case "dateisnot":
                                valueExpression = Expression.Constant(valueFormated, expressionBody.Type);
                                binaryExpression = Expression.NotEqual(expressionBody, valueExpression);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(criteria), criteria, null);
                        }
                    }
                    catch(Exception)
                    {
                        return new FalseSpecification<TEntity>().SatisfiedBy();
                    }
                    break;

                case "contains":
                    binaryExpression = ComputeExpression(expressionBody, "Contains", value);
                    break;

                case "notcontains":
                    binaryExpression = Expression.Not(ComputeExpression(expressionBody, "Contains", value));
                    break;

                case "startswith":
                    binaryExpression = ComputeExpression(expressionBody, "StartsWith", value);
                    break;

                case "endswith":
                    binaryExpression = ComputeExpression(expressionBody, "EndsWith", value);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(criteria), criteria, null);
            }

            return Expression.Lambda<Func<TEntity, bool>>(binaryExpression, parameterExpression);
        }

        private static Expression ComputeExpression(Expression expressionBody, string filterFonction, string value)
        {
            ConstantExpression valueExpression;
            MethodInfo method;
            Expression binaryExpression;
            var methodToString = expressionBody.Type.GetMethod("ToString", Type.EmptyTypes);
            if (IsCollectionType(expressionBody.Type))
            {
                valueExpression = Expression.Constant(value);
                method = typeof(string).GetMethod(filterFonction, new[] { typeof(string) });
                ParameterExpression pe = Expression.Parameter(typeof(string), "a");
                var predicate = Expression.Call(pe, method ?? throw new InvalidOperationException(), valueExpression);
                var predicateExpr = Expression.Lambda<Func<string, bool>>(predicate, pe);

                binaryExpression = Expression.Call(typeof(Enumerable), "Any", new[] { typeof(string) }, expressionBody, predicateExpr);
            }
            else
            {
                valueExpression = Expression.Constant(value);
                method = typeof(string).GetMethod(filterFonction, new[] { typeof(string) });
                if (expressionBody.Type != typeof(string))
                {
                    expressionBody = Expression.Call(expressionBody, methodToString ?? throw new InvalidOperationException());
                }

                binaryExpression = Expression.Call(expressionBody, method ?? throw new InvalidOperationException(), valueExpression);
            }

            return binaryExpression;

            // PostgreSQL : use like function only to do search action with no case sensitive => Contains is case sensitive with database without database CI
            // if (IsCollectionType(valueType))
            // {
            //    valueExpression = Expression.Constant(valueFormated);
            //    method = typeof(string).GetMethod("ILike", new[] { typeof(string) });
            //    ParameterExpression pe = Expression.Parameter(typeof(string), "a");
            //    var predicate = Expression.Call(pe, method ?? throw new InvalidOperationException(), valueExpression);
            //    var predicateExpr = Expression.Lambda<Func<string, bool>>(predicate, pe);

            //    binaryExpression = Expression.Call(typeof(Enumerable), "Any", new[] { typeof(string) }, expressionBody, predicateExpr);
            // }
            // else
            // {
            //    if (expressionBody.Type != typeof(string))
            //    {
            //        expressionBody = Expression.Call(expressionBody, methodToString ?? throw new InvalidOperationException());
            //    }

            //    binaryExpression = Expression.Call(typeof(NpgsqlDbFunctionsExtensions), nameof(NpgsqlDbFunctionsExtensions.ILike),
            //        Type.EmptyTypes, Expression.Property(null, typeof(EF), nameof(EF.Functions)),
            //        expressionBody, Expression.Constant($"%{valueFormated}%"));
            // }
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