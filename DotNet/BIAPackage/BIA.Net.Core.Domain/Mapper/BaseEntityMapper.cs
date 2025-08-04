// <copyright file="BaseEntityMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The class used to define the base mapper.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public abstract class BaseEntityMapper<TEntity>
    {
        /// <summary>
        /// Gets the collection used for expressions to access fields.
        /// </summary>
        public virtual ExpressionCollection<TEntity> ExpressionCollection
        {
            get
            {
#pragma warning disable CA1065 // Ne pas lever d'exceptions dans les emplacements inattendus
#pragma warning disable S2372 // Exceptions should not be thrown from property getters
                throw new BadBiaFrameworkUsageException("This mapper is not build for list, or the implementation of ExpressionCollection is missing.");
#pragma warning restore S2372 // Exceptions should not be thrown from property getters
#pragma warning restore CA1065 // Ne pas lever d'exceptions dans les emplacements inattendus
            }
        }

        /// <summary>
        /// Gets the collection used for expressions to order fields.
        /// By default, same value as ExpressionCollection.
        /// </summary>
        public virtual ExpressionCollection<TEntity> ExpressionCollectionOrder
        {
            get
            {
                return this.ExpressionCollection;
            }
        }

        /// <summary>
        /// Gets the collection used for expressions to filter fields.
        /// By default, same value as ExpressionCollection.
        /// </summary>
        public virtual ExpressionCollection<TEntity> ExpressionCollectionFilter
        {
            get
            {
                return this.ExpressionCollection;
            }
        }

        /// <summary>
        /// Gets the collection used for expressions to filter fields in "In" matchMode.
        /// By default, same value as ExpressionCollectionFilter.
        /// </summary>
        public virtual ExpressionCollection<TEntity> ExpressionCollectionFilterIn
        {
            get
            {
                return this.ExpressionCollectionFilter;
            }
        }

        /// <summary>
        /// CSVs the specified x.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="x">The x.</param>
        /// <returns>A string for a string cell.</returns>
        public static string CSVCell<TType>(TType x)
        {
            if (typeof(TType) == typeof(string))
            {
                return CSVString(x as string);
            }

            if (typeof(IFormattable).IsAssignableFrom(typeof(TType)))
            {
                return CSVNumber(x as IFormattable);
            }

            return null;
        }

        /// <summary>
        /// CSVs the string.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>A string for a string cell.</returns>
        public static string CSVString(string x)
        {
            return "\"=\"\"" + x?.Replace("\"", "\"\"\"\"") + "\"\"\"";
        }

        /// <summary>
        /// CSVs the list.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>A string for a list cell.</returns>
        public static string CSVList(ICollection<OptionDto> x)
        {
            return CSVString(string.Join(" - ", x?.Select(ca => ca.Display).ToList()));
        }

        /// <summary>
        /// CSVs the list.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>A string for a list cell.</returns>
        public static string CSVList(IEnumerable<OptionDto> x)
        {
            return CSVString(string.Join(" - ", x?.Select(ca => ca.Display).ToList()));
        }

        /// <summary>
        /// CSVs the date.
        /// </summary>
        /// <param name="x">The DateTime.</param>
        /// <returns>A string for a date cell.</returns>
        public static string CSVDate(DateTime? x)
        {
            return x?.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// CSVs the date.
        /// </summary>
        /// <param name="x">The DateTime.</param>
        /// <returns>A string for a date cell.</returns>
        public static string CSVDateMonth(DateTime? x)
        {
            return x?.ToString("yyyy-MM");
        }

        /// <summary>
        /// CSVs the date.
        /// </summary>
        /// <param name="x">The DateTime.</param>
        /// <returns>A string for a date cell.</returns>
        public static string CSVDateYear(DateTime? x)
        {
            return x?.ToString("yyyy");
        }

        /// <summary>
        /// CSVs the time.
        /// </summary>
        /// <param name="x">The DateTime.</param>
        /// <returns>A string for a time cell.</returns>
        public static string CSVTime(DateTime? x)
        {
            return x?.ToString("HH:mm");
        }

        /// <summary>
        /// CSVs the time.
        /// </summary>
        /// <param name="x">The TimeSpan.</param>
        /// <returns>A string for a time cell.</returns>
        public static string CSVTime(TimeSpan? x)
        {
            return x is null ? string.Empty : $"{x?.Hours}:{x?.Minutes}";
        }

        /// <summary>
        /// CSVs the time.
        /// </summary>
        /// <param name="x">The string.</param>
        /// <returns>A string for a time cell.</returns>
        public static string CSVTime(string x)
        {
            return x;
        }

        /// <summary>
        /// CSVs the date time.
        /// </summary>
        /// <param name="x">The DateTime.</param>
        /// <returns>A string for a date and time cell.</returns>
        public static string CSVDateTime(DateTime? x)
        {
            return x?.ToString("yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// CSVs the date time with seconds.
        /// </summary>
        /// <param name="x">The DateTime.</param>
        /// <returns>A string for a date and time with seconds cell.</returns>
        public static string CSVDateTimeSeconds(DateTime? x)
        {
            return x?.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// CSVs the bool.
        /// </summary>
        /// <param name="x">if set to <c>true</c> [x].</param>
        /// <returns>A string for a bool cell.</returns>
        public static string CSVBool(bool x)
        {
            return x.ToString().ToLower();
        }

        /// <summary>
        /// CSVs the bool.
        /// </summary>
        /// <param name="x">if set to <c>true</c> [x].</param>
        /// <returns>A string for a bool cell.</returns>
        public static string CSVBool(bool? x)
        {
            return x.HasValue ? x.Value.ToString().ToLower() : string.Empty;
        }

        /// <summary>
        /// CSVs the number.
        /// </summary>
        /// <typeparam name="T">The type of number.</typeparam>
        /// <param name="x">The number.</param>
        /// <returns>A string for a number cell.</returns>
        public static string CSVNumber<T>(T? x)
            where T : struct, IFormattable
        {
            return x.HasValue ? x.Value.ToString(null, CultureInfo.InvariantCulture) : string.Empty;
        }

        /// <summary>
        /// CSVs the number.
        /// </summary>
        /// <typeparam name="T">The type of number.</typeparam>
        /// <param name="x">The number.</param>
        /// <returns>A string for a number cell.</returns>
        public static string CSVNumber<T>(T x)
            where T : IFormattable
        {
            return x.ToString(null, CultureInfo.InvariantCulture);
        }
    }
}