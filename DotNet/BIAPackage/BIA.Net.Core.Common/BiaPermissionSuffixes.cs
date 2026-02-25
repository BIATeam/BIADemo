// <copyright file="BiaPermissionSuffixes.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common
{
    /// <summary>
    /// BIA permission suffixes.
    public static class BiaPermissionSuffixes
    {
        /// <summary>
        /// The members permission suffixes.
        /// </summary>
        public static class Members
        {
            /// <summary>
            /// The permission suffix to access to the list of members.
            /// </summary>
            public const string ListAccessSuffix = "_Member_List_Access";

            /// <summary>
            /// The permission suffix to create members.
            /// </summary>
            public const string CreateSuffix = "_Member_Create";

            /// <summary>
            /// The permission suffix to read members.
            /// </summary>
            public const string ReadSuffix = "_Member_Read";

            /// <summary>
            /// The permission suffix to update members.
            /// </summary>
            public const string UpdateSuffix = "_Member_Update";

            /// <summary>
            /// The permission suffix to delete members.
            /// </summary>
            public const string DeleteSuffix = "_Member_Delete";

            /// <summary>
            /// The permission suffix to save members.
            /// </summary>
            public const string SaveSuffix = "_Member_Save";
        }

        /// <summary>
        /// The team views permission suffixes.
        /// </summary>
        public static class TeamViews
        {
            /// <summary>
            /// The permission suffix to add a team view.
            /// </summary>
            public const string AddTeamViewSuffix = "_View_Add_TeamView";

            /// <summary>
            /// The permission suffix to update a team view.
            /// </summary>
            public const string UpdateTeamViewSuffix = "_View_Update_TeamView";

            /// <summary>
            /// The permission suffix to set default team view.
            /// </summary>
            public const string SetDefaultTeamViewSuffix = "_View_Set_Default_TeamView";

            /// <summary>
            /// The permission suffix to assign view to a team.
            /// </summary>
            public const string AssignToTeamSuffix = "_View_Assign_To_Team";
        }
    }
}
