// <copyright file="ObjectHelper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Helpers
{
    using System;

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
    }
}
