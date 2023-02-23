// <copyright file="ConfigurationExtensions.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{

    using Microsoft.Extensions.Configuration;

    public static class ConfigurationExtensions
    {
        public static string GetDBEngine(this IConfiguration configuration, string name)
        {
            return configuration["DBEngine:" + name];
        }
    }
}
