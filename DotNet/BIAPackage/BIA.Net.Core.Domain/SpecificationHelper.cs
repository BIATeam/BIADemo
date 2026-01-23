// <copyright file="SpecificationHelper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text.Json;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Base.Interface;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.Specification;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The helpers for specifications.
    /// </summary>
    public static class SpecificationHelper
    {
        /// <summary>
        /// Add the specifications of the lazy load.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to filter with.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TMapper">The type of the mapper.</typeparam>
        /// <param name="specification">The specification to update.</param>
        /// <param name="matcher">The matcher.</param>
        /// <param name="dto">The lazy DTO.</param>
        /// <param name="clientTimeZoneContext">Client time zone context.</param>
        /// <returns>
        /// The specification updated.
        /// </returns>
        public static Specification<TEntity> GetLazyLoad<TEntity, TKey, TMapper>(Specification<TEntity> specification, TMapper matcher, IPagingFilterFormatDto dto, IClientTimeZoneContext clientTimeZoneContext)
        where TEntity : class, IEntity<TKey>, new()
        where TMapper : BaseEntityMapper<TEntity>
        {
            ExpressionCollection<TEntity> whereClausesFilter = matcher.ExpressionCollectionFilter;
            ExpressionCollection<TEntity> whereClausesFilterIn = matcher.ExpressionCollectionFilterIn;

            if (dto?.Filters == null)
            {
                return specification;
            }

            Specification<TEntity> globalFilterSpecification = null;

            foreach (var (key, json) in dto.Filters)
            {
                if (json.ValueKind == JsonValueKind.Array)
                {
                    var values = JsonSerializer.Deserialize<Dictionary<string, object>[]>(json);
                    Specification<TEntity> ruleSpecification = null;
                    foreach (var value in values)
                    {
                        if (value["matchMode"]?.ToString() == "in")
                        {
                            AddSpecByValue<TEntity, TKey>(ref ruleSpecification, whereClausesFilterIn, ref globalFilterSpecification, key, value, clientTimeZoneContext);
                        }
                        else
                        {
                            AddSpecByValue<TEntity, TKey>(ref ruleSpecification, whereClausesFilter, ref globalFilterSpecification, key, value, clientTimeZoneContext);
                        }
                    }

                    specification &= ruleSpecification;
                }
                else if (json.ValueKind == JsonValueKind.Object)
                {
                    var value = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                    if (value["matchMode"]?.ToString() == "in")
                    {
                        AddSpecByValue<TEntity, TKey>(ref specification, whereClausesFilterIn, ref globalFilterSpecification, key, value, clientTimeZoneContext);
                    }
                    else
                    {
                        AddSpecByValue<TEntity, TKey>(ref specification, whereClausesFilter, ref globalFilterSpecification, key, value, clientTimeZoneContext);
                    }
                }
            }

            if (globalFilterSpecification != null)
            {
                specification &= globalFilterSpecification;
            }

            return specification;
        }

        /// <summary>
        /// Determines whether [is collection type] [the specified value type].
        /// </summary>
        /// <param name="valueType">Type of the value.</param>
        /// <returns>
        ///   <c>true</c> if [is collection type] [the specified value type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCollectionType(Type valueType)
        {
            return (valueType.Name.Length > 11 && valueType.Name.Substring(0, 11) == "IEnumerable")
                || (valueType.Name.Length > 18 && valueType.Name.Substring(0, 18) == "IOrderedEnumerable");
        }

        private static void AddSpecByValue<TEntity, TKey>(ref Specification<TEntity> specification, ExpressionCollection<TEntity> whereClauses, ref Specification<TEntity> globalFilterSpecification, string key, Dictionary<string, object> value, IClientTimeZoneContext clientTimeZoneContext)
            where TEntity : class, IEntity<TKey>, new()
        {
            if (value["value"] == null && value["matchMode"]?.ToString() != "empty" && value["matchMode"]?.ToString() != "notEmpty" && value["matchMode"]?.ToString() != "today" && value["matchMode"]?.ToString() != "beforeToday" && value["matchMode"]?.ToString() != "afterToday")
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

                var isLocalTimeCriteria = IsDateTimeSelector(expression) &&
                    value.TryGetValue("isLocal", out var isLocalRawValue) &&
                    bool.Parse(isLocalRawValue.ToString());

                matchingCriteria = LazyDynamicFilterExpression<TEntity>(
                    expression,
                    value["matchMode"].ToString(),
                    value["value"]?.ToString(),
                    isLocalTimeCriteria,
                    clientTimeZoneContext);

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
        /// Checks if the selector is a DateTime or Nullable DateTime.
        /// </summary>
        /// <param name="expression">Expression to check.</param>
        /// <returns>Boolean indicating if the selector is a DateTime or Nullable DateTime.</returns>
        private static bool IsDateTimeSelector(LambdaExpression expression)
        {
            PropertyInfo propertyInfo = null;

            if (expression.Body is MemberExpression memberExpression)
            {
                propertyInfo = memberExpression.Member as PropertyInfo;
            }
            else if (expression.Body is ConditionalExpression conditionalExpression && conditionalExpression.IfTrue is MemberExpression ifTrueMember)
            {
                propertyInfo = ifTrueMember.Member as PropertyInfo;
            }
            else if (expression.Body is MethodCallExpression methodCallExpression && methodCallExpression.Object is MemberExpression objectMember)
            {
                propertyInfo = objectMember.Member as PropertyInfo;
            }

            if (propertyInfo == null)
            {
                return false;
            }

            var propertyType = propertyInfo.PropertyType;
            return propertyType == typeof(DateTime) ||
                   (propertyType.IsGenericType &&
                    propertyType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                    propertyType.GenericTypeArguments[0] == typeof(DateTime));
        }

        /// <summary>
        /// Create the matching criteria dynamically for the given parameter and value.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="expression">The expression used to get the property.</param>
        /// <param name="criteria">The matching criteria.</param>
        /// <param name="value">The value to filter with.</param>
        /// <param name="isLocalTimeCriteria">Indicates if the criteria value is expressed as local time.</param>
        /// <param name="clientTimeZoneContext">Client time zone context.</param>
        /// <returns>The expression created dynamically.</returns>
        private static Expression<Func<TEntity, bool>>
            LazyDynamicFilterExpression<TEntity>(
                LambdaExpression expression, string criteria, string value, bool isLocalTimeCriteria, IClientTimeZoneContext clientTimeZoneContext)
                    where TEntity : class
        {
            ConstantExpression valueExpression;
            ParameterExpression parameterExpression = expression.Parameters.FirstOrDefault();
            var expressionBody = expression.Body;
            var expressionBodyGenericArgumentType = expressionBody.Type.GenericTypeArguments.FirstOrDefault() ?? expressionBody.Type;

            Expression binaryExpression;

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
                        var culture = new CultureInfo("en-US");
                        object valueFormated = TypeDescriptor.GetConverter(expressionBody.Type).ConvertFromString(null, culture, value);
                        if (isLocalTimeCriteria && valueFormated is DateTime valueDateTime)
                        {
                            valueFormated = valueDateTime.ToUniversalTime();
                        }

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
                    catch (Exception)
                    {
                        return new FalseSpecification<TEntity>().SatisfiedBy();
                    }

                    break;

                case "today":
                case "beforetoday":
                case "aftertoday":
                    try
                    {
                        var culture = new CultureInfo("en-US");
                        DateTime now = isLocalTimeCriteria ? clientTimeZoneContext.GetClientNow().Date : DateTime.Now.Date;
                        DateTime tomorrow = now.AddDays(1);

                        object todayFormatted = TypeDescriptor.GetConverter(expressionBody.Type)
                            .ConvertFromString(null, culture, now.ToString("o"));
                        object tomorrowFormatted = TypeDescriptor.GetConverter(expressionBody.Type)
                            .ConvertFromString(null, culture, tomorrow.ToString("o"));

                        Expression todayExpression = Expression.Constant(todayFormatted, expressionBody.Type);
                        Expression tomorrowExpression = Expression.Constant(tomorrowFormatted, expressionBody.Type);

                        binaryExpression = criteria.ToLower() switch
                        {
                            "today" => Expression.AndAlso(
                                Expression.GreaterThanOrEqual(expressionBody, todayExpression),
                                Expression.LessThan(expressionBody, tomorrowExpression)),
                            "beforetoday" => Expression.LessThan(expressionBody, todayExpression),
                            "aftertoday" => Expression.GreaterThanOrEqual(expressionBody, tomorrowExpression),
                            _ => throw new ArgumentOutOfRangeException(nameof(criteria), criteria, null),
                        };
                    }
                    catch (Exception)
                    {
                        return new FalseSpecification<TEntity>().SatisfiedBy();
                    }

                    break;

                case "empty":
                    var isNull = Expression.Equal(expressionBody, Expression.Constant(null, expressionBody.Type));

                    if (IsCollectionType(expressionBody.Type))
                    {
                        binaryExpression = Expression.Not(Expression.Call(typeof(Enumerable), "Any", new[] { expressionBodyGenericArgumentType }, expressionBody));
                    }
                    else if (expressionBody.Type == typeof(string))
                    {
                        var isEmpty = Expression.Equal(expressionBody, Expression.Constant(string.Empty, typeof(string)));
                        binaryExpression = Expression.OrElse(isNull, isEmpty);
                    }
                    else
                    {
                        binaryExpression = isNull;
                    }

                    break;

                case "notempty":
                    var isNotNull = Expression.Not(Expression.Equal(expressionBody, Expression.Constant(null, expressionBody.Type)));

                    if (IsCollectionType(expressionBody.Type))
                    {
                        binaryExpression = Expression.Call(typeof(Enumerable), "Any", new[] { expressionBodyGenericArgumentType }, expressionBody);
                    }
                    else if (expressionBody.Type == typeof(string))
                    {
                        var isNotEmpty = Expression.Not(Expression.Equal(expressionBody, Expression.Constant(string.Empty, typeof(string))));
                        binaryExpression = Expression.AndAlso(isNotNull, isNotEmpty);
                    }
                    else
                    {
                        binaryExpression = isNotNull;
                    }

                    break;

                case "contains":
                    if (isLocalTimeCriteria)
                    {
                        // Convert SQL date value to local client time zone before string comparison contains in provider
                    }
                    binaryExpression = ComputeExpression(expressionBody, "Contains", value);
                    break;

                case "notcontains":
                    binaryExpression = Expression.Not(ComputeExpression(expressionBody, "Contains", value));
                    break;

                case "startswith":
                    binaryExpression = ComputeExpression(expressionBody, "StartsWith", value);
                    break;

                case "notstartswith":
                    binaryExpression = Expression.Not(ComputeExpression(expressionBody, "StartsWith", value));
                    break;

                case "endswith":
                    binaryExpression = ComputeExpression(expressionBody, "EndsWith", value);
                    break;

                case "notendswith":
                    binaryExpression = Expression.Not(ComputeExpression(expressionBody, "EndsWith", value));
                    break;

                case "in":
                    JObject o = JObject.Parse(@"{ array : " + value + " }");
                    JArray a = (JArray)o["array"];
                    Type genericListType = typeof(IList<>).MakeGenericType(expressionBodyGenericArgumentType);
                    var values = a.ToObject(genericListType);

                    if (IsCollectionType(expressionBody.Type))
                    {
                        valueExpression = Expression.Constant(values);
                        var method = typeof(Enumerable)
                            .GetMethods()
                            .Single(m => m.Name == "Intersect" && m.GetParameters().Length == 2)
                            .MakeGenericMethod(expressionBodyGenericArgumentType);
                        var intersect = Expression.Call(method ?? throw new InvalidOperationException(), expressionBody, valueExpression);
                        dynamic dynamicList = values;
                        int valuesCount = dynamicList.Count;
                        var lengthValueExpression = Expression.Constant(valuesCount);
                        var count = Expression.Call(typeof(Enumerable), "Count", new[] { expressionBodyGenericArgumentType }, intersect);
                        var equals = Expression.Equal(count, lengthValueExpression);

                        binaryExpression = equals;
                    }
                    else
                    {
                        var filterExpression = Expression.Constant(values);
                        var method = typeof(Enumerable)
                            .GetMethods()
                            .Single(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                            .MakeGenericMethod(expressionBodyGenericArgumentType);
                        var contains = Expression.Call(method, filterExpression, expressionBody);

                        binaryExpression = contains;
                    }

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
                    var methodToString = expressionBody.Type.GetMethod("ToString", Type.EmptyTypes);
                    expressionBody = Expression.Call(expressionBody, methodToString ?? throw new InvalidOperationException());
                }

                binaryExpression = Expression.Call(expressionBody, method ?? throw new InvalidOperationException(), valueExpression);
            }

            return binaryExpression;
#pragma warning disable S125 // Sections of code should not be commented out

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
            //
            //    binaryExpression = Expression.Call(typeof(NpgsqlDbFunctionsExtensions), nameof(NpgsqlDbFunctionsExtensions.ILike),
            //        Type.EmptyTypes, Expression.Property(null, typeof(EF), nameof(EF.Functions)),
            //        expressionBody, Expression.Constant($"%{valueFormated}%"));
            // }
#pragma warning restore S125 // Sections of code should not be commented out
        }
    }
}