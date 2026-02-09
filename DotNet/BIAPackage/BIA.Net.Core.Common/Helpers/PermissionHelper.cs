namespace BIA.Net.Core.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class PermissionHelper
    {
        public static int GetPermissionId(string permissionName, IEnumerable<IPermissionConverter> permissionConverters)
        {
            var permissionIds = permissionConverters.SelectMany(c => c.ConvertToIds([permissionName]));
            if (!permissionConverters.Any())
            {
                throw new InvalidOperationException($"Unable to find permission converters that returned an ID for permission name '{permissionName}'.");
            }

            if (permissionIds.Count() > 1)
            {
                throw new InvalidOperationException($"Multiple permission converters returned an ID for permission name '{permissionName}'.");
            }

            return permissionIds.First();
        }

        public static string GetPermissionName(int permissionId, IEnumerable<IPermissionConverter> permissionConverters)
        {
            var permissionNames = permissionConverters.SelectMany(c => c.ConvertToNames([permissionId]));
            if (!permissionConverters.Any())
            {
                throw new InvalidOperationException($"Unable to find permission converters that returned a name for permission ID '{permissionId}'.");
            }

            if (permissionNames.Count() > 1)
            {
                throw new InvalidOperationException($"Multiple permission converters returned a name for permission ID '{permissionId}'.");
            }

            return permissionNames.First();
        }
    }
}
