/* eslint-disable @typescript-eslint/naming-convention */
export enum BiaPermission {
  Background_Task_Admin = 'Background_Task_Admin',
  Background_Task_Read_Only = 'Background_Task_Read_Only',
  Notification_Create = 'Notification_Create',
  Notification_List_Access = 'Notification_List_Access',
  Notification_Delete = 'Notification_Delete',
  Notification_Read = 'Notification_Read',
  Notification_Update = 'Notification_Update',

  Roles_List = 'Roles_List',

  User_Add = 'User_Add',
  User_Delete = 'User_Delete',
  User_Save = 'User_Save',
  User_List = 'User_List',
  User_ListAD = 'User_ListAD',
  User_List_Access = 'User_List_Access',
  User_Sync = 'User_Sync',
  User_UpdateRoles = 'User_UpdateRoles',

  LdapDomains_List = 'LdapDomains_List',
  View_List = 'View_List',
  View_AddUserView = 'View_Add_UserView',
  View_AddTeamViewSuffix = '_View_Add_TeamView',
  View_UpdateUserView = 'View_Update_UserView',
  View_UpdateTeamViewSuffix = '_View_Update_TeamView',
  View_DeleteUserView = 'View_Delete_UserView',
  View_DeleteTeamView = 'View_Delete_TeamView',
  View_SetDefaultUserView = 'View_Set_Default_UserView',
  View_SetDefaultTeamViewSuffix = '_View_Set_Default_TeamView',
  View_AssignToTeamSuffix = '_View_Assign_To_Team',

  Impersonation_Connection_Rights = 'Impersonation_Connection_Rights',

  Announcement_Create = 'Announcement_Create',
  Announcement_Delete = 'Announcement_Delete',
  Announcement_List_Access = 'Announcement_List_Access',
  Announcement_Read = 'Announcement_Read',
  Announcement_Update = 'Announcement_Update',
}

export function IsAnnouncementPermission(permission: string): boolean {
  return (
    permission === BiaPermission.Announcement_Create ||
    permission === BiaPermission.Announcement_Delete ||
    permission === BiaPermission.Announcement_List_Access ||
    permission === BiaPermission.Announcement_Read ||
    permission === BiaPermission.Announcement_Update
  );
}
