// <copyright file="UserContext.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Domain.User
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using BIA.Net.Core.Common.Configuration;

    /// <summary>
    /// A <see cref="UserContext"/> implementation with additional utility methods.
    /// </summary>
    public class UserContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserContext"/> class.
        /// </summary>
        /// <param name="culture">The wanted culture.</param>
        /// <param name="acceptedCultures">The accepted cultures in config.</param>
        public UserContext(string culture, IEnumerable<Culture> acceptedCultures)
        {
            var acceptedCulture = acceptedCultures.FirstOrDefault(w => Array.Exists(w.AcceptedCodes, cc => cc == culture));

            // Select the default culture
            acceptedCulture ??= acceptedCultures.FirstOrDefault(w => Array.Exists(w.AcceptedCodes, cc => cc == "default"));

            if (acceptedCulture == null)
            {
                throw new ConfigurationErrorsException("You forgot to specify a default culture in bianetconfig.json.");
            }

            this.Culture = acceptedCulture.Code;
            this.Language = acceptedCulture.LanguageCode;
            this.LanguageId = acceptedCulture.LanguageId;
        }

        /// <summary>
        /// Culture.
        /// </summary>
        public string Culture { get; private set; }

        /// <summary>
        /// Language.
        /// </summary>
        public string Language { get; private set; }

        /// <summary>
        /// Language Id.
        /// </summary>
        public int LanguageId { get; private set; }
    }
}
