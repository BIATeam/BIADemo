// <copyright file="DateHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Helpers
{
    using System;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// Date helper.
    /// </summary>
    public static class DateHelper
    {
        /// <summary>
        /// Check if given <paramref name="type"/> is date type.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to evaluate.</param>
        /// <returns><see cref="bool"/>.</returns>
        public static bool IsDateType(this Type type)
        {
            type = UnwrapNullable(type);
            return type == typeof(DateTime)
                || type == typeof(DateTimeOffset)
                || type == typeof(DateOnly)
                || type == typeof(TimeOnly)
                ;
        }

        /// <summary>
        /// Format the <paramref name="value"/> with given <paramref name="culture"/> as date or time format according to the <paramref name="property"/> type.
        /// </summary>
        /// <param name="property">The <paramref name="value"/> <see cref="Type"/>.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="culture">Culture to use.</param>
        /// <returns>Formatted date or time value.</returns>
        public static string FormatWithCulture(PropertyInfo property, object value, CultureInfo culture)
        {
            if (value is null)
            {
                return null;
            }

            if (property is null)
            {
                return (value as IFormattable)?.ToString(null, culture) ?? value.ToString();
            }

            var propType = UnwrapNullable(property.PropertyType);
            if (propType == typeof(DateTime) && value is DateTime dt)
            {
                return dt.ToString(culture);
            }

            if (propType == typeof(DateTimeOffset) && value is DateTimeOffset dto)
            {
                return dto.ToString(culture);
            }

            if (propType == typeof(DateOnly) && value is DateOnly dOnly)
            {
                return dOnly.ToString(culture);
            }

            if (propType == typeof(TimeOnly) && value is TimeOnly tOnly)
            {
                return tOnly.ToString(culture);
            }

            if (value is IFormattable formattable)
            {
                return formattable.ToString(null, culture);
            }

            return value.ToString();
        }

        private static Type UnwrapNullable(Type t) => Nullable.GetUnderlyingType(t) ?? t;
    }
}
