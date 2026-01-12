// <copyright file="BiaRights.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common
{
    /// <summary>
    /// The list of all rights.
    /// </summary>
    public static class BiaRights
    {
        /// <summary>
        /// The roles rights.
        /// </summary>
        public static class Roles
        {
            /// <summary>
            /// The right to access to the list of roles (options only).
            /// </summary>
            public const string Options = "Roles_Options";

            /// <summary>
            /// The right to get the roles of the current user.
            /// </summary>
            public const string ListForCurrentUser = "Roles_List_For_Current_User";
        }

        /// <summary>
        /// The roles rights.
        /// </summary>
        public static class Permissions
        {
            /// <summary>
            /// The right to access to the list of permissions (options only).
            /// </summary>
            public const string Options = "Permissions_Options";
        }

        /// <summary>
        /// The LDAP domains rights.
        /// </summary>
        public static class LdapDomains
        {
            /// <summary>
            /// The right to get all LDAP domains.
            /// </summary>
            public const string List = "LdapDomains_List";
        }

        /// <summary>
        /// The LDAP domains rights.
        /// </summary>
        public static class Languages
        {
            /// <summary>
            /// The right to get all LDAP domains.
            /// </summary>
            public const string Options = "Languages_Options";
        }

        /// <summary>
        /// The Profile image access rights.
        /// </summary>
        public static class ProfileImage
        {
            /// <summary>
            /// The right to get the profile image.
            /// </summary>
            public const string Get = "ProfileImage_Get";
        }

        /// <summary>
        /// The home rights.
        /// </summary>
        public static class Home
        {
            /// <summary>
            /// The right to access the home.
            /// </summary>
            public const string Access = "Home_Access";
        }

        /// <summary>
        /// The logs rights.
        /// </summary>
        public static class Logs
        {
            /// <summary>
            /// The right to create logs.
            /// </summary>
            public const string Create = "Logs_Create";
        }

        /// <summary>
        /// The members rights.
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
        /// The teams rights.
        /// </summary>
        public static class Teams
        {
            /// <summary>
            /// The right to access to the list of teams (options only).
            /// </summary>
            public const string Options = "Team_Options";

            /// <summary>
            /// The right to access to all sites.
            /// </summary>
            public const string AccessAll = "Team_Access_All";

            /// <summary>
            /// The right to access to the list of sites.
            /// </summary>
            public const string ListAccess = "Team_List_Access";

            /// <summary>
            /// The right to set default site.
            /// </summary>
            public const string SetDefaultTeam = "Team_Set_Default_Team";

            /// <summary>
            /// The right to set default role.
            /// </summary>
            public const string SetDefaultRoles = "Team_Set_Default_Roles";
        }

        /// <summary>
        /// The users rights.
        /// </summary>
        public static class Users
        {
            /// <summary>
            /// The right to access to the list of user (options only).
            /// </summary>
            public const string Options = "User_Options";

            /// <summary>
            /// The right to access to the list of users.
            /// </summary>
            public const string ListAccess = "User_List_Access";

            /// <summary>
            /// The right to get the list of users.
            /// </summary>
            public const string List = "User_List";

            /// <summary>
            /// The right to get the list of AD users.
            /// </summary>
            public const string ListAD = "User_ListAD";

            /// <summary>
            /// The right to get the list of AD users.
            /// </summary>
            public const string Read = "User_Read";

            /// <summary>
            /// The right to add users.
            /// </summary>
            public const string Add = "User_Add";

            /// <summary>
            /// The right to delete users.
            /// </summary>
            public const string Delete = "User_Delete";

            /// <summary>
            /// The right to delete users.
            /// </summary>
            public const string Save = "User_Save";

            /// <summary>
            /// The right to synchronize users.
            /// </summary>
            public const string Sync = "User_Sync";

            /// <summary>
            /// The right to add users.
            /// </summary>
            public const string UpdateRoles = "User_UpdateRoles";
        }

        /// <summary>
        /// The views rights.
        /// </summary>
        public static class Views
        {
            /// <summary>
            /// The right to get a view of the current user.
            /// </summary>
            public const string Read = "View_Read";

            /// <summary>
            /// The right to get all views of the current user.
            /// </summary>
            public const string List = "View_List";

            /// <summary>
            /// The right to add an user view.
            /// </summary>
            public const string AddUserView = "View_Add_UserView";

            /// <summary>
            /// The right to add an site view.
            /// </summary>
            public const string AddTeamViewSuffix = "_View_Add_TeamView";

            /// <summary>
            /// The right to update an user view.
            /// </summary>
            public const string UpdateUserView = "View_Update_UserView";

            /// <summary>
            /// The right to update an site view.
            /// </summary>
            public const string UpdateTeamViewSuffix = "_View_Update_TeamView";

            /// <summary>
            /// The right to delete an user view.
            /// </summary>
            public const string DeleteUserView = "View_Delete_UserView";

            /// <summary>
            /// The right to delete an site view.
            /// </summary>
            public const string DeleteTeamView = "View_Delete_TeamView";

            /// <summary>
            /// The right to set default user view.
            /// </summary>
            public const string SetDefaultUserView = "View_Set_Default_UserView";

            /// <summary>
            /// The right to set default site view.
            /// </summary>
            public const string SetDefaultTeamViewSuffix = "_View_Set_Default_TeamView";

            /// <summary>
            /// The right to assign view to a site.
            /// </summary>
            public const string AssignToTeamSuffix = "_View_Assign_To_Team";
        }

        /// <summary>
        /// The notifications rights.
        /// </summary>
        public static class Notifications
        {
            /// <summary>
            /// The right to access to the list of notifications.
            /// </summary>
            public const string ListAccess = "Notification_List_Access";

            /// <summary>
            /// The right to read notifications.
            /// </summary>
            public const string Read = "Notification_Read";

            /// <summary>
            /// The right to create notifications.
            /// </summary>
            public const string Create = "Notification_Create";

            /// <summary>
            /// The right to update notifications.
            /// </summary>
            public const string Update = "Notification_Update";

            /// <summary>
            /// The right to delete notifications.
            /// </summary>
            public const string Delete = "Notification_Delete";
        }

        /// <summary>
        /// The planes rights.
        /// </summary>
        public static class NotificationTypes
        {
            /// <summary>
            /// The right to access to the list of notifications types (options only).
            /// </summary>
            public const string Options = "NotificationType_Options";
        }

        /// <summary>
        /// The Impersonation rights.
        /// </summary>
        public static class Impersonation
        {
            /// <summary>
            /// The right to connect with same rights of another user.
            /// </summary>
            public const string ConnectionRights = "Impersonation_Connection_Rights";
        }
    }
}
