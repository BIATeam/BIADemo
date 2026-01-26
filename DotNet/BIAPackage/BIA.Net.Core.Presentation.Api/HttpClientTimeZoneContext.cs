// <copyright file="HttpClientTimeZoneContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Api
{
    using System;
    using BIA.Net.Core.Common;
    using Microsoft.AspNetCore.Http;
    using NodaTime;

    /// <summary>
    /// Context to get client time zone information from request headers.
    /// </summary>
    public sealed class HttpClientTimeZoneContext : ClientTimeZoneContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientTimeZoneContext"/> class.
        /// </summary>
        /// <param name="http">Http context accessor.</param>
        public HttpClientTimeZoneContext(IHttpContextAccessor http)
        {
            var raw = http.HttpContext?.Request.Headers["X-Client-TimeZone"].ToString();
            this.IanaTimeZoneId = string.IsNullOrWhiteSpace(raw) ? "UTC" : raw;
            if (TimeZoneInfo.TryConvertIanaIdToWindowsId(this.IanaTimeZoneId, out var windowsId))
            {
                this.WindowsTimeZoneId = windowsId;
            }

            this.WindowsTimeZone = TimeZoneInfo.FindSystemTimeZoneById(this.IanaTimeZoneId);
            this.Zone = DateTimeZoneProviders.Tzdb[this.IanaTimeZoneId];
        }
    }
}
