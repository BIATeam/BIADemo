// <copyright file="ObjectHelper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Helpers
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Reflection;
    using BIA.Net.Core.Common.Attributes;
    using BIA.Net.Core.Common.Enum;

    /// <summary>
    /// Object helper.
    /// </summary>
    public static class ObjectHelper
    {
        /// <summary>
        /// Convert a <paramref name="rawKey"/> to <paramref name="targetType"/>.
        /// </summary>
        /// <param name="rawKey">The object key to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <returns>Converted key as <see cref="object"/>.</returns>
        public static object ConvertKey(object rawKey, Type targetType)
        {
            if (rawKey == null)
            {
                return null;
            }

            var src = rawKey.GetType();

            if (targetType.IsAssignableFrom(src))
            {
                return rawKey;
            }

            if (targetType.IsEnum)
            {
                return Enum.Parse(targetType, rawKey.ToString()!);
            }

            if (targetType == typeof(Guid))
            {
                return rawKey is Guid g ? g : Guid.Parse(rawKey.ToString()!);
            }

            var t = Nullable.GetUnderlyingType(targetType) ?? targetType;
            return Convert.ChangeType(rawKey, t, System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Ensure <paramref name="value"/> is of type <typeparamref name="TTarget"/> and return <paramref name="value"/> as <typeparamref name="TTarget"/> if true.
        /// </summary>
        /// <typeparam name="TTarget">Target type.</typeparam>
        /// <param name="value">Object to convert.</param>
        /// <returns><paramref name="value"/> as <typeparamref name="TTarget"/>.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="value"/> is not of type <typeparamref name="TTarget"/>.</exception>
        public static TTarget EnsureType<TTarget>(object value)
        {
            if (value is not TTarget typed)
            {
                throw new InvalidOperationException($"{value?.GetType()} is not of type {typeof(TTarget)}");
            }

            return typed;
        }

        /// <summary>
        /// Gets the property marked with the BiaRowVersionPropertyAttribute for the specified provider.
        /// </summary>
        /// <param name="type">The type to search for the property.</param>
        /// <param name="provider">The database provider.</param>
        /// <returns>The PropertyInfo of the matching property, or null if no match is found.</returns>
        public static PropertyInfo GetBiaRowVersionProperty(this Type type, DbProvider provider)
        {
            return type.GetProperties().SingleOrDefault(x => x.GetCustomAttribute<BiaRowVersionPropertyAttribute>()?.Provider == provider);
        }
    }
}
