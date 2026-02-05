/* eslint-disable @typescript-eslint/naming-convention */
export enum BiaPermission {
  Background_Task_Admin,
  Background_Task_Read_Only,

  Notification_Create,
  Notification_List_Access,
  Notification_Delete,
  Notification_Read,
  Notification_Update,

  Roles_List,
  Roles_Options,
  Roles_List_For_Current_User,

  User_Add,
  User_Delete,
  User_Save,
  User_Read,
  User_List,
  User_ListAD,
  User_List_Access,
  User_Sync,
  User_UpdateRoles,
  User_Options,

  LdapDomains_List,
  Languages_Options,
  ProfileImage_Get,
  Logs_Create,

  Team_Access_All,
  Team_List_Access,
  Team_Options,
  Team_Set_Default_Team,
  Team_Set_Default_Roles,

  View_List,
  View_Read,
  View_AddUserView,
  View_AddTeamViewSuffix,
  View_UpdateUserView,
  View_UpdateTeamViewSuffix,
  View_DeleteUserView,
  View_DeleteTeamView,
  View_SetDefaultUserView,
  View_SetDefaultTeamViewSuffix,
  View_AssignToTeamSuffix,

  Impersonation_Connection_Rights,

  Announcement_Create,
  Announcement_Delete,
  Announcement_List_Access,
  Announcement_Read,
  Announcement_Update,

  NotificationType_Options,
  AnnouncementType_Options,
}

export function IsAnnouncementPermission(permission: string): boolean {
  const announcementPermissions = [
    BiaPermission.Announcement_Create,
    BiaPermission.Announcement_Delete,
    BiaPermission.Announcement_List_Access,
    BiaPermission.Announcement_Read,
    BiaPermission.Announcement_Update
  ];
  
  return announcementPermissions.some(p => BiaPermission[p] === permission);
}
