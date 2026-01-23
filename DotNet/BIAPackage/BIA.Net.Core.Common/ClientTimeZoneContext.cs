// <copyright file="ClientTimeZoneContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common
{
    using System;
    using NodaTime;

    /// <summary>
    /// Represents the client's time zone context, providing access to IANA and Windows time zone identifiers and the
    /// corresponding NodaTime zone.
    /// </summary>
    public class ClientTimeZoneContext : IClientTimeZoneContext
    {
        /// <inheritdoc/>
        public string IanaTimeZoneId { get; set; }

        /// <inheritdoc/>
        public TimeZoneInfo WindowsTimeZone { get; set; }

        /// <inheritdoc/>
        public DateTimeZone Zone { get; set; }

        /// <inheritdoc/>
        public DateTime GetClientNow()
        => SystemClock.Instance.GetCurrentInstant()
            .InZone(this.Zone)
            .LocalDateTime
            .ToDateTimeUnspecified();
    }
}
