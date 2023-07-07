// <copyright file="UserContext.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.Extensions.Primitives;
    using Newtonsoft.Json;

    /// <summary>
    /// A <see cref="UserContext"/> implementation with additional utility methods.
    /// </summary>
    public class UserContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserContext"/> class.
        /// </summary>
        /// <param name="culture">The default culture.</param>
        public UserContext(string culture)
        {
            this.Culture = culture;
            this.Language = this.Culture.Split(",")[0].Split("-")[0];
            if (string.IsNullOrEmpty(this.Language) || this.Language.Count() != 2)
            {
                this.Language = "en";
            }
        }

        /// <summary>
        /// Culture.
        /// </summary>
        public string Culture { get; private set; }

        /// <summary>
        /// Language.
        /// </summary>
        public string Language { get; private set; }
    }
}
