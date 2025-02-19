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
    }
}
