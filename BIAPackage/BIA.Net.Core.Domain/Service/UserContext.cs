// <copyright file="UserContext.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Domain.Service
{
    using Microsoft.Extensions.Primitives;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    /// <summary>
    /// A <see cref="ClaimsPrincipal"/> implementation with additional utility methods.
    /// </summary>
    /// <seealso cref="ClaimsPrincipal" />
    public class UserContext
    {
        public string Culture { get; private set; }
        public string Language { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BIAClaimsPrincipal"/> class from the given <see cref="ClaimsPrincipal"/>.
        /// </summary>
        /// <param name="principal">The base principal.</param>
        public UserContext(string acceptedLanguage)
        {
            Culture = acceptedLanguage;
            Language = Culture.Split("-")[0];
        }
    }
}
