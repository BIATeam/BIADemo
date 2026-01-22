// <copyright file="IClientTimeZoneContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common
{
    using System;
    using NodaTime;

    /// <summary>
    /// Represents a context for client time zone information, providing access to IANA and Windows time zone
    /// identifiers and the associated DateTimeZone.
    /// </summary>
    public interface IClientTimeZoneContext
    {
        /// <summary>
        /// IANA time zone identifier.
        /// </summary>
        public string IanaTimeZoneId { get; }

        /// <summary>
        /// Windows time zone.
        /// </summary>
        public TimeZoneInfo WindowsTimeZone { get; }

        /// <summary>
        /// Zone information.
        /// </summary>
        public DateTimeZone Zone { get; }
    }
}