// <copyright file="BiaPermissionId.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common
{
    /// <summary>
    /// Permission identifiers for BIA Framework (ordinales for JWT compaction).
    /// IMPORTANT: Keep enum member order in sync with BiaPermission enum in Angular/packages/bia-ng/core/bia-permission.ts
    /// When adding new permissions:
    /// 1. Add to both BiaPermission.ts (Angular) and this enum (C#) in the SAME ORDER
    /// 2. DO NOT specify explicit values - order is implicit (0, 1, 2, ...)
    /// 3. If out of sync, fallback to string claims ensures zero auth breakage, but watch logs for warnings
    /// </summary>
    public enum BiaPermissionId
    {
        /// <summary>
        /// Background Task Admin.
        /// </summary>
        Background_Task_Admin = 10000,

        /// <summary>
        /// Background Task Read Only.
        /// </summary>
        Background_Task_Read_Only,

        /// <summary>
        /// Notification Create.
        /// </summary>
        Notification_Create,

        /// <summary>
        /// Notification List Access.
        /// </summary>
        Notification_List_Access,

        /// <summary>
        /// Notification Delete.
        /// </summary>
        Notification_Delete,

        /// <summary>
        /// Notification Read.
        /// </summary>
        Notification_Read,

        /// <summary>
        /// Notification Update.
        /// </summary>
        Notification_Update,

        /// <summary>
        /// Roles List.
        /// </summary>
        Roles_List,

        /// <summary>
        /// Roles Options.
        /// </summary>
        Roles_Options,

        /// <summary>
        /// Roles List For Current User.
        /// </summary>
        Roles_List_For_Current_User,

        /// <summary>
        /// User Add.
        /// </summary>
        User_Add,

        /// <summary>
        /// User Delete.
        /// </summary>
        User_Delete,

        /// <summary>
        /// User Save.
        /// </summary>
        User_Save,

        /// <summary>
        /// User Save.
        /// </summary>
        User_Read,

        /// <summary>
        /// User List.
        /// </summary>
        User_List,

        /// <summary>
        /// User ListAD.
        /// </summary>
        User_ListAD,

        /// <summary>
        /// User List Access.
        /// </summary>
        User_List_Access,

        /// <summary>
        /// User Sync.
        /// </summary>
        User_Sync,

        /// <summary>
        /// User Update Roles.
        /// </summary>
        User_UpdateRoles,

        /// <summary>
        /// User Options.
        /// </summary>
        User_Options,

        /// <summary>
        /// LDAP Domains List.
        /// </summary>
        LdapDomains_List,

        /// <summary>
        /// Languages Options.
        /// </summary>
        Languages_Options,

        /// <summary>
        /// Profile Image Get.
        /// </summary>
        ProfileImage_Get,

        /// <summary>
        /// Logs Create.
        /// </summary>
        Logs_Create,

        /// <summary>
        /// Team Access All.
        /// </summary>
        Team_Access_All,

        /// <summary>
        /// Team List Access.
        /// </summary>
        Team_List_Access,

        /// <summary>
        /// Team Options.
        /// </summary>
        Team_Options,

        /// <summary>
        /// Team Set Default Team.
        /// </summary>
        Team_Set_Default_Team,

        /// <summary>
        /// Team Set Default Roles.
        /// </summary>
        Team_Set_Default_Roles,

        /// <summary>
        /// View List.
        /// </summary>
        View_List,

        /// <summary>
        /// View Read.
        /// </summary>
        View_Read,

        /// <summary>
        /// View Add User View.
        /// </summary>
        View_Add_UserView,

        /// <summary>
        /// View Add Team View Suffix.
        /// </summary>
        View_Add_TeamViewSuffix,

        /// <summary>
        /// View Update User View.
        /// </summary>
        View_Update_UserView,

        /// <summary>
        /// View Update Team View Suffix.
        /// </summary>
        View_Update_TeamViewSuffix,

        /// <summary>
        /// View Delete User View.
        /// </summary>
        View_Delete_UserView,

        /// <summary>
        /// View Delete Team View.
        /// </summary>
        View_Delete_TeamView,

        /// <summary>
        /// View Set Default User View.
        /// </summary>
        View_Set_Default_UserView,

        /// <summary>
        /// View Set Default Team View Suffix.
        /// </summary>
        View_Set_Default_TeamViewSuffix,

        /// <summary>
        /// View Assign To Team Suffix.
        /// </summary>
        View_Assign_To_TeamSuffix,

        /// <summary>
        /// Impersonation Connection Rights.
        /// </summary>
        Impersonation_Connection_Rights,

        /// <summary>
        /// Announcement Create.
        /// </summary>
        Announcement_Create,

        /// <summary>
        /// Announcement Delete.
        /// </summary>
        Announcement_Delete,

        /// <summary>
        /// Announcement List Access.
        /// </summary>
        Announcement_List_Access,

        /// <summary>
        /// Announcement Read.
        /// </summary>
        Announcement_Read,

        /// <summary>
        /// Announcement Update.
        /// </summary>
        Announcement_Update,

        /// <summary>
        /// Notification Type Options.
        /// </summary>
        NotificationType_Options,

        /// <summary>
        /// Announcement Type Options.
        /// </summary>
        AnnouncementType_Options,
    }
}
