// <copyright file="AuthorizationConfiguration.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Configuration
{
    using System;
    using Microsoft.AspNetCore.Authorization;
    using TheBIADevCompany.BIADemo.Presentation.Api.Controllers.User;

    /// <summary>
    /// Authorization Helper.
    /// </summary>
    public static class AuthorizationConfiguration
    {
        /// <summary>
        /// Determines whether [is negotiate authentication].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is negotiate authentication]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNegotiate()
        {
            AuthorizeAttribute authorizeAttribute = (AuthorizeAttribute)Attribute.GetCustomAttribute(typeof(AuthController), typeof(AuthorizeAttribute));
            return authorizeAttribute?.AuthenticationSchemes == Microsoft.AspNetCore.Authentication.Negotiate.NegotiateDefaults.AuthenticationScheme;
        }
    }
}
