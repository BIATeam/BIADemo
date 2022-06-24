namespace BIA.Net.Core.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Helper class to map objects.
    /// </summary>
    public static class PropertyMapper
    {
        /// <summary>
        /// Map a source object to a destination object.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDest">The type of the destination objects.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="dest">The destination object.</param>
        /// <param name="excludeProperties">The excluded properties of the mapping.</param>
        public static void Map<TSource, TDest>(TSource source, TDest dest, List<string> excludeProperties = null)
        {
            if (source == null || dest == null)
            {
                return;
            }

            PropertyInfo[] sourceProperties = source.GetType().GetProperties();
            PropertyInfo[] destProperties = dest.GetType().GetProperties();

            if (!(sourceProperties?.Length > 0) || !(destProperties?.Length > 0))
            {
                return;
            }

            destProperties = destProperties.AsEnumerable()
                .Where(x => x.CanWrite && (excludeProperties == null || !excludeProperties.Contains(x.Name))).ToArray();
            sourceProperties = sourceProperties.AsEnumerable().Where(x =>
                x.CanWrite && (excludeProperties == null || !excludeProperties.Contains(x.Name))).ToArray();

            if (!(sourceProperties?.Length > 0) || !(destProperties?.Length > 0))
            {
                return;
            }

            foreach (PropertyInfo destProperty in destProperties)
            {
                foreach (PropertyInfo sourceProperty in sourceProperties)
                {
                    if (string.Equals(sourceProperty.Name, destProperty.Name, StringComparison.OrdinalIgnoreCase) &&
                        sourceProperty.PropertyType == destProperty.PropertyType)
                    {
                        destProperty.SetValue(dest, sourceProperty.GetValue(source));
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Map a source object list to a destination object list.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDest">The type of the destination objects.</typeparam>
        /// <param name="sources">The source object list.</param>
        /// <param name="excludeProperties">The excluded properties of the mapping.</param>
        /// <returns>The list of objects.</returns>
        public static IList<TDest> Map<TSource, TDest>(IEnumerable<TSource> sources, List<string> excludeProperties = null)
            where TSource : class, new()
            where TDest : class, new()
        {
            List<TDest> list = new List<TDest>();
            if (sources != null)
            {
                foreach (TSource source in sources)
                {
                    TDest dest = new TDest();
                    Map(source, dest, excludeProperties);
                    list.Add(dest);
                }
            }

            return list;
        }
    }
}
