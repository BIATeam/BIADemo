// <copyright file="BiaWebApi.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration.BiaWebApi
{
    /// <summary>
    /// Bia WebApi Configuration.
    /// </summary>
    public class BiaWebApi
    {
        /// <summary>
        /// Gets or sets the base address.
        /// </summary>
        public string BaseAddress { get; set; }

        /// <summary>
        /// Gets or sets the credential source.
        /// </summary>
        public CredentialSource CredentialSource { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use login fine grained].
        /// </summary>
        public bool UseLoginFineGrained { get; set; }
    }
}
