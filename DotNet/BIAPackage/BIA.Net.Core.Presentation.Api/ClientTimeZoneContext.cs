// <copyright file="ClientTimeZoneContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Api
{
    using System;
    using BIA.Net.Core.Common;
    using Microsoft.AspNetCore.Http;
    using NodaTime;
    using TimeZoneConverter;

    /// <summary>
    /// Context to get client time zone information from request headers.
    /// </summary>
    public sealed class ClientTimeZoneContext : IClientTimeZoneContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientTimeZoneContext"/> class.
        /// </summary>
        /// <param name="http">Http context accessor.</param>
        public ClientTimeZoneContext(IHttpContextAccessor http)
        {
            var raw = http.HttpContext?.Request.Headers["X-Client-TimeZone"].ToString();
            this.IanaTimeZoneId = string.IsNullOrWhiteSpace(raw) ? "UTC" : raw;
            this.WindowsTimeZone = TimeZoneInfo.FindSystemTimeZoneById(this.IanaTimeZoneId);
            this.Zone = DateTimeZoneProviders.Tzdb[this.IanaTimeZoneId];
        }

        /// <inheritdoc/>
        public string IanaTimeZoneId { get; }

        /// <inheritdoc/>
        public TimeZoneInfo WindowsTimeZone { get; }

        /// <inheritdoc/>
        public DateTimeZone Zone { get; }

        /// <inheritdoc/>
        public DateTime GetClientNow()
        => SystemClock.Instance.GetCurrentInstant()
            .InZone(this.Zone)
            .LocalDateTime
            .ToDateTimeUnspecified();
    }
}
