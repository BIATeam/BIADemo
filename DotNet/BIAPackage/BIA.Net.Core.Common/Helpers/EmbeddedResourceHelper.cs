// <copyright file="EmbeddedResourceHelper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Helper for embedded resources.
    /// </summary>
    public static class EmbeddedResourceHelper
    {
        /// <summary>
        /// Read the content of en ambedded resources.
        /// </summary>
        /// <param name="assembly">Assembly that contains the embedded resource.</param>
        /// <param name="resourcePath">The embedded resource path.</param>
        /// <returns>Content of the embedded resource as <see cref="string"/>.</returns>
        /// <exception cref="FileNotFoundException">.</exception>
        public static async Task<string> ReadEmbeddedResourceAsync(Assembly assembly, string resourcePath)
        {
            await using var stream = assembly.GetManifestResourceStream(resourcePath) ?? throw new FileNotFoundException(resourcePath);
            using var reader = new StreamReader(stream, Encoding.UTF8);
            return await reader.ReadToEndAsync();
        }

        /// <summary>
        /// Retrive all the embedded resources path from an assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="relativeResourcesFolderPath">Optionnal relative folder path to search the embedded resources.</param>
        /// <param name="fileExtensionPattern">Optional file extension pattern of the embedded resources to search.</param>
        /// <returns><see cref="List{T}"/>.</returns>
        public static List<string> GetEmbeddedResourcesPath(Assembly assembly, string relativeResourcesFolderPath = null, string fileExtensionPattern = null)
        {
            var resourcesFolderPath = string.Join('.', assembly.GetName().Name, relativeResourcesFolderPath);
            var resourcesPath = assembly.GetManifestResourceNames().AsEnumerable();

            if (!string.IsNullOrWhiteSpace(relativeResourcesFolderPath))
            {
                resourcesPath = resourcesPath.Where(r => r.StartsWith(resourcesFolderPath, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(fileExtensionPattern))
            {
                var fileExtension = $".{fileExtensionPattern}";
                resourcesPath = resourcesPath.Where(r => r.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase));
            }

            return resourcesPath.ToList();
        }
    }
}
