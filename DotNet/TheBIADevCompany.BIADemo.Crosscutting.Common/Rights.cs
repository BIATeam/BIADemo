// <copyright file="Rights.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Crosscutting.Common
{
    /// <summary>
    /// The list of all rights.
    /// </summary>
    public static class Rights
    {
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

        // Begin BIADemo

        /// <summary>
        /// The aircraft maintenance companies rights.
        /// </summary>
        public static class AircraftMaintenanceCompanies
        {
            /// <summary>
            /// The right to access to the list of aircraft maintenance companies.
            /// </summary>
            public const string ListAccess = "AircraftMaintenanceCompany_List_Access";

            /// <summary>
            /// The right to create aircraft maintenance companies.
            /// </summary>
            public const string Create = "AircraftMaintenanceCompany_Create";

            /// <summary>
            /// The right to read aircraft maintenance companies.
            /// </summary>
            public const string Read = "AircraftMaintenanceCompany_Read";

            /// <summary>
            /// The right to update aircraft maintenance companies.
            /// </summary>
            public const string Update = "AircraftMaintenanceCompany_Update";

            /// <summary>
            /// The right to delete aircraft maintenance companies.
            /// </summary>
            public const string Delete = "AircraftMaintenanceCompany_Delete";

            /// <summary>
            /// The right to save aircraft maintenance companies.
            /// </summary>
            public const string Save = "AircraftMaintenanceCompany_Save";
        }

        /// <summary>
        /// The planes rights.
        /// </summary>
        public static class Engines
        {
            /// <summary>
            /// The right to access to the list of planes.
            /// </summary>
            public const string ListAccess = "Engine_List_Access";

            /// <summary>
            /// The right to create planes.
            /// </summary>
            public const string Create = "Engine_Create";

            /// <summary>
            /// The right to read planes.
            /// </summary>
            public const string Read = "Engine_Read";

            /// <summary>
            /// The right to update planes.
            /// </summary>
            public const string Update = "Engine_Update";

            /// <summary>
            /// The right to delete planes.
            /// </summary>
            public const string Delete = "Engine_Delete";

            /// <summary>
            /// The right to save planes.
            /// </summary>
            public const string Save = "Engine_Save";
        }

        /// <summary>
        /// The maintenance team rights.
        /// </summary>
        public static class MaintenanceTeams
        {
            /// <summary>
            /// The right to access to the list of maintenance team.
            /// </summary>
            public const string ListAccess = "MaintenanceTeam_List_Access";

            /// <summary>
            /// The right to create maintenance team.
            /// </summary>
            public const string Create = "MaintenanceTeam_Create";

            /// <summary>
            /// The right to read maintenance team.
            /// </summary>
            public const string Read = "MaintenanceTeam_Read";

            /// <summary>
            /// The right to update maintenance team.
            /// </summary>
            public const string Update = "MaintenanceTeam_Update";

            /// <summary>
            /// The right to delete maintenance team.
            /// </summary>
            public const string Delete = "MaintenanceTeam_Delete";

            /// <summary>
            /// The right to save maintenance team.
            /// </summary>
            public const string Save = "MaintenanceTeam_Save";
        }

        /// BIAToolKit - Begin Partial Rights Plane
        /// <summary>
        /// The planes rights.
        /// </summary>
        public static class Planes
        {
            /// <summary>
            /// The right to access to the list of planes.
            /// </summary>
            public const string ListAccess = "Plane_List_Access";

            /// <summary>
            /// The right to create planes.
            /// </summary>
            public const string Create = "Plane_Create";

            /// <summary>
            /// The right to read planes.
            /// </summary>
            public const string Read = "Plane_Read";

            /// <summary>
            /// The right to update planes.
            /// </summary>
            public const string Update = "Plane_Update";

            /// <summary>
            /// The right to delete planes.
            /// </summary>
            public const string Delete = "Plane_Delete";

            /// <summary>
            /// The right to save planes.
            /// </summary>
            public const string Save = "Plane_Save";
        }
        /// BIAToolKit - End Partial Rights Plane

        /// <summary>
        /// The planes rights.
        /// </summary>
        public static class PlanesTypes
        {
            /// <summary>
            /// The right to access to the list of planes types (options only).
            /// </summary>
            public const string Options = "PlaneType_Options";

            /// <summary>
            /// The right to access to the list of planes types.
            /// </summary>
            public const string ListAccess = "PlaneType_List_Access";

            /// <summary>
            /// The right to create planes types.
            /// </summary>
            public const string Create = "PlaneType_Create";

            /// <summary>
            /// The right to read planes types.
            /// </summary>
            public const string Read = "PlaneType_Read";

            /// <summary>
            /// The right to update planes types.
            /// </summary>
            public const string Update = "PlaneType_Update";

            /// <summary>
            /// The right to delete planes types.
            /// </summary>
            public const string Delete = "PlaneType_Delete";

            /// <summary>
            /// The right to save planes types.
            /// </summary>
            public const string Save = "PlaneType_Save";
        }

        /// BIAToolKit - Begin Partial Rights Airport
        /// <summary>
        /// The planes rights.
        /// </summary>
        public static class Airports
        {
            /// <summary>
            /// The right to access to the list of airports (options only).
            /// </summary>
            public const string Options = "Airport_Options";

            /// <summary>
            /// The right to access to the list of airports.
            /// </summary>
            public const string ListAccess = "Airport_List_Access";

            /// <summary>
            /// The right to create airports.
            /// </summary>
            public const string Create = "Airport_Create";

            /// <summary>
            /// The right to read airports.
            /// </summary>
            public const string Read = "Airport_Read";

            /// <summary>
            /// The right to update airports.
            /// </summary>
            public const string Update = "Airport_Update";

            /// <summary>
            /// The right to delete airports.
            /// </summary>
            public const string Delete = "Airport_Delete";

            /// <summary>
            /// The right to save planes.
            /// </summary>
            public const string Save = "Airport_Save";
        }
        /// BIAToolKit - End Partial Rights Airport

        /// <summary>
        /// The Hangfire rights.
        /// </summary>
        public static class Hangfires
        {
            /// <summary>
            /// The right to run the worker example.
            /// </summary>
            public const string RunWorker = "Hangfire_Run_Worker";
        }

        // End BIADemo

        /// <summary>
        /// The roles rights.
        /// </summary>
        public static class Roles
        {
            /// <summary>
            /// The right to access to the list of airports (options only).
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
            /// The right to access to the list of airports (options only).
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
        /// The sites rights.
        /// </summary>
        public static class Sites
        {
            /// <summary>
            /// The right to access to the list of sites.
            /// </summary>
            public const string ListAccess = "Site_List_Access";

            /// <summary>
            /// The right to create sites.
            /// </summary>
            public const string Create = "Site_Create";

            /// <summary>
            /// The right to read sites.
            /// </summary>
            public const string Read = "Site_Read";

            /// <summary>
            /// The right to update sites.
            /// </summary>
            public const string Update = "Site_Update";

            /// <summary>
            /// The right to delete sites.
            /// </summary>
            public const string Delete = "Site_Delete";

            /// <summary>
            /// The right to save sites.
            /// </summary>
            public const string Save = "Site_Save";
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

        /// BIAToolKit - Begin Rights
        /// BIAToolKit - End Rights
    }
}