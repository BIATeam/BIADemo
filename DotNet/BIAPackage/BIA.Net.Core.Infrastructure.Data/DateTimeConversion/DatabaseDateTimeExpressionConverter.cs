// <copyright file="DatabaseDateTimeExpressionConverter.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.DateTimeConversion
{
    using System;

    /// <summary>
    /// Static class providing marker methods for DateTime conversion in LINQ queries.
    /// These methods are never executed directly - they are intercepted by EF Core's query translator.
    /// </summary>
    public static class DatabaseDateTimeExpressionConverter
    {
        /// <summary>
        /// Converts a UTC DateTime to a localized string representation.
        /// This method is a marker and will be translated to SQL AT TIME ZONE by EF Core.
        /// NEVER called at runtime - only used for LINQ expression translation.
        /// Pass IANA timezone ID (e.g., "Europe/Paris") - the translator will convert to Windows format for SQL Server.
        /// </summary>
        /// <param name="dateTime">The UTC DateTime to convert.</param>
        /// <param name="timeZoneId">The target timezone identifier (IANA format like "Europe/Paris").</param>
        /// <returns>A string representation of the DateTime in the target time zone.</returns>
        public static string ConvertDateTimeToLocalString(DateTime dateTime, string timeZoneId)
        {
            // This method should NEVER be executed
            // It's only a marker for EF Core query translation
            throw new NotSupportedException(
                "ConvertDateTimeToLocalString is a marker method for EF Core query translation. " +
                "It should never be executed directly.");
        }
    }
}
