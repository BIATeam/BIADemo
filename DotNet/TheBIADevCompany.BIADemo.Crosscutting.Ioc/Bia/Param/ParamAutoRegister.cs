// <copyright file="ParamAutoRegister.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Crosscutting.Ioc.Bia.Param
{
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Param Auto Register.
    /// </summary>
    internal partial class ParamAutoRegister
    {
        /// <summary>
        /// Gets the collection.
        /// </summary>
        internal IServiceCollection Collection { get; init; }

        /// <summary>
        /// Gets the service lifetime.
        /// </summary>
        internal ServiceLifetime ServiceLifetime { get; init; } = ServiceLifetime.Transient;

        /// <summary>
        /// Gets the excluded service names.
        /// </summary>
        internal IEnumerable<string> ExcludedServiceNames { get; init; }

        /// <summary>
        /// Gets the included service names.
        /// </summary>
        internal IEnumerable<string> IncludedServiceNames { get; init; }
    }
}
