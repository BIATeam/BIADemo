/**
 * Permission enum (values = permission strings for auth checks)
 * IMPORTANT: Keep enum member order in sync with PermissionId enum in DotNet/TheBIADevCompany.BIADemo.Crosscutting.Common/Enum/PermissionId.cs
 * When adding new permissions:
 * 1. Add to both this enum (Angular) and PermissionId.cs (C#) in the SAME ORDER
 * 2. Options permissions are in a separate enum: PermissionOptions
 * 3. If out of sync, fallback to string claims ensures zero auth breakage
 */
/* eslint-disable @typescript-eslint/naming-convention */
export enum Permission {
  // Home
  Home_Access,

  // Site
  Site_Create,
  Site_Delete,
  Site_List_Access,
  Site_Read,
  Site_Save,
  Site_Update,
  Site_Member_Create,
  Site_Member_Delete,
  Site_Member_List_Access,
  Site_Member_Read,
  Site_Member_Update,
  Site_Member_Save,
  Site_View_Add_TeamView,
  Site_View_Update_TeamView,
  Site_View_Set_Default_TeamView,
  Site_View_Assign_To_Team,
  Site_Access_All,

  // Begin BIADemo
  Hangfire_Access,
  Hangfire_Run_Worker,
  // End BIADemo

  // BIAToolKit - Begin Permission
  // BIAToolKit - End Permission

  // Begin BIAToolKit Generation Ignore
  // BIAToolKit - Begin Partial Permission Plane
  Plane_Create,
  Plane_Delete,
  Plane_List_Access,
  Plane_Read,
  Plane_Save,
  Plane_Update,
  Plane_Fix,
  // BIAToolKit - End Partial Permission Plane

  // BIAToolKit - Begin Partial Permission Engine
  Engine_Create,
  Engine_Delete,
  Engine_List_Access,
  Engine_Read,
  Engine_Save,
  Engine_Update,
  Engine_Fix,
  // BIAToolKit - End Partial Permission Engine

  // BIAToolKit - Begin Partial Permission AircraftMaintenanceCompany
  AircraftMaintenanceCompany_Create,
  AircraftMaintenanceCompany_Delete,
  AircraftMaintenanceCompany_List_Access,
  AircraftMaintenanceCompany_Read,
  AircraftMaintenanceCompany_Save,
  AircraftMaintenanceCompany_Update,
  AircraftMaintenanceCompany_Member_Update,
  AircraftMaintenanceCompany_Member_Delete,
  AircraftMaintenanceCompany_Member_Save,
  AircraftMaintenanceCompany_Member_Create,
  AircraftMaintenanceCompany_Member_Read,
  AircraftMaintenanceCompany_Member_List_Access,
  AircraftMaintenanceCompany_View_AddTeamView,
  AircraftMaintenanceCompany_View_UpdateTeamView,
  AircraftMaintenanceCompany_View_SetDefaultTeamView,
  AircraftMaintenanceCompany_View_AssignToTeam,
  // BIAToolKit - End Partial Permission AircraftMaintenanceCompany

  // BIAToolKit - Begin Partial Permission MaintenanceTeam
  MaintenanceTeam_Create,
  MaintenanceTeam_Delete,
  MaintenanceTeam_List_Access,
  MaintenanceTeam_Read,
  MaintenanceTeam_Save,
  MaintenanceTeam_Update,
  MaintenanceTeam_Member_Update,
  MaintenanceTeam_Member_Delete,
  MaintenanceTeam_Member_Save,
  MaintenanceTeam_Member_Create,
  MaintenanceTeam_Member_Read,
  MaintenanceTeam_Member_List_Access,
  MaintenanceTeam_View_AddTeamView,
  MaintenanceTeam_View_UpdateTeamView,
  MaintenanceTeam_View_SetDefaultTeamView,
  MaintenanceTeam_View_AssignToTeam,
  MaintenanceTeam_Fix,
  // BIAToolKit - End Partial Permission MaintenanceTeam

  // BIAToolKit - Begin Partial Permission Pilot
  Pilot_Create,
  Pilot_Delete,
  Pilot_List_Access,
  Pilot_Read,
  Pilot_Save,
  Pilot_Update,
  Pilot_Fix,
  // BIAToolKit - End Partial Permission Pilot

  // BIAToolKit - Begin Partial Permission Flight
  Flight_Create,
  Flight_Delete,
  Flight_List_Access,
  Flight_Read,
  Flight_Save,
  Flight_Update,
  Flight_Fix,
  // BIAToolKit - End Partial Permission Flight

  // End BIAToolKit Generation Ignore

  // Begin BIADemo
  Airport_Create,
  Airport_Delete,
  Airport_List_Access,
  Airport_Read,
  Airport_Save,
  Airport_Update,

  PlaneType_Create,
  PlaneType_Delete,
  PlaneType_List_Access,
  PlaneType_Read,
  PlaneType_Save,
  PlaneType_Update,

  MaintenanceContract_Create,
  MaintenanceContract_Delete,
  MaintenanceContract_List_Access,
  MaintenanceContract_Read,
  MaintenanceContract_Save,
  MaintenanceContract_Update,
  // End BIADemo
}
