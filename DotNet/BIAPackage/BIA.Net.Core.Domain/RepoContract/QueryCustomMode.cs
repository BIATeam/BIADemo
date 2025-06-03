// <copyright file="QueryCustomMode.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    /// <summary>
    /// Custom mode for the query.
    /// </summary>
    public static class QueryCustomMode
    {
        /// <summary>
        /// Mode Update the view of type user.
        /// </summary>
        public const string ModeUpdateViewUsers = "UpdateViewUsers";

        /// <summary>
        /// Mode Update the view of type site.
        /// </summary>
        public const string ModeUpdateViewTeams = "UpdateViewTeams";

        /// <summary>
        /// Mode Update the view of type site.
        /// </summary>
        public const string ModeUpdateViewTeamsAndUsers = "UpdateViewTeamsAndUsers";
    }
}
