// <copyright file="BiaRights.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common
{
    /// <summary>
    /// Legacy BiaRights class kept only for team-based permission suffixes.
    /// All other permissions should now use nameof() with the corresponding enum (BiaPermissionId, PermissionId, OptionPermissionId).
    /// </summary>
    public static class BiaRights
    {
        /// <summary>
        /// The members rights - kept for team-based permission suffixes.
        /// These suffixes are concatenated with team type prefixes at runtime.
        /// </summary>
        public static class Members
        {
            /// <summary>
            /// The right to access to the list of members.
            /// </summary>
            public const string ListAccessSuffix = "_Member_List_Access";

            /// <summary>
            /// The right to create members.
            /// </summary>
            public const string CreateSuffix = "_Member_Create";

            /// <summary>
            /// The right to read members.
            /// </summary>
            public const string ReadSuffix = "_Member_Read";

            /// <summary>
            /// The right to update members.
            /// </summary>
            public const string UpdateSuffix = "_Member_Update";

            /// <summary>
            /// The right to delete members.
            /// </summary>
            public const string DeleteSuffix = "_Member_Delete";

            /// <summary>
            /// The right to save members.
            /// </summary>
            public const string SaveSuffix = "_Member_Save";
        }

        /// <summary>
        /// The views rights - kept for team-based permission suffixes.
        /// These suffixes are concatenated with team type prefixes at runtime.
        /// </summary>
        public static class Views
        {
            /// <summary>
            /// The right to add an site view.
            /// </summary>
            public const string AddTeamViewSuffix = "_View_Add_TeamView";

            /// <summary>
            /// The right to update an site view.
            /// </summary>
            public const string UpdateTeamViewSuffix = "_View_Update_TeamView";

            /// <summary>
            /// The right to set default site view.
            /// </summary>
            public const string SetDefaultTeamViewSuffix = "_View_Set_Default_TeamView";

            /// <summary>
            /// The right to assign view to a site.
            /// </summary>
            public const string AssignToTeamSuffix = "_View_Assign_To_Team";
        }
    }
}
