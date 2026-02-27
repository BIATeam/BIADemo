// <copyright file="ParamIocContainer.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Ioc.Param
{
    using BIA.Net.Core.Common.Configuration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Param Ioc Container.
    /// </summary>
    public class ParamIocContainer
    {
        /// <summary>
        /// Gets the collection.
        /// </summary>
        public IServiceCollection Collection { get; init; }

        /// <summary>
        /// Gets the bia net section.
        /// </summary>
        public BiaNetSection BiaNetSection { get; set; }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; init; }

        /// <summary>
        /// Gets a value indicating whether this instance is API.
        /// </summary>
        public bool IsApi { get; init; }

        /// <summary>
        /// Gets a value indicating whether this instance is unit test.
        /// </summary>
        public bool IsUnitTest { get; init; }
    }
}
