// <copyright file="PermissionId.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Crosscutting.Common.Enum
{
    /// <summary>
    /// Permission identifiers for BIADemo project (ordinales for JWT compaction).
    /// IMPORTANT: Keep enum member order in sync with Permission enum in Angular/src/app/shared/permission.ts
    /// When adding new permissions:
    /// 1. Add to both Permission.ts (Angular) and this enum (C#) in the SAME ORDER
    /// 2. DO NOT specify explicit values for regular permissions - order is implicit (0, 1, 2, ...)
    /// 3. Options permissions start at 1000 to separate from CRUD permissions
    /// 4. If out of sync, fallback to string claims ensures zero auth breakage, but watch logs for warnings
    /// </summary>
    public enum PermissionId
    {
        /// <summary>
        /// Hangfire Access.
        /// </summary>
        Hangfire_Access,

        /// <summary>
        /// Home Access.
        /// </summary>
        Home_Access,

        /// <summary>
        /// Site Create.
        /// </summary>
        Site_Create,

        /// <summary>
        /// Site Delete.
        /// </summary>
        Site_Delete,

        /// <summary>
        /// Site List Access.
        /// </summary>
        Site_List_Access,

        /// <summary>
        /// Site Read.
        /// </summary>
        Site_Read,

        /// <summary>
        /// Site Save.
        /// </summary>
        Site_Save,

        /// <summary>
        /// Site Update.
        /// </summary>
        Site_Update,

        /// <summary>
        /// Site Member Create.
        /// </summary>
        Site_Member_Create,

        /// <summary>
        /// Site Member Delete.
        /// </summary>
        Site_Member_Delete,

        /// <summary>
        /// Site Member List Access.
        /// </summary>
        Site_Member_List_Access,

        /// <summary>
        /// Site Member Update.
        /// </summary>
        Site_Member_Update,

        /// <summary>
        /// Site Member Save.
        /// </summary>
        Site_Member_Save,

        /// <summary>
        /// Plane Create.
        /// </summary>
        Plane_Create,

        /// <summary>
        /// Plane Delete.
        /// </summary>
        Plane_Delete,

        /// <summary>
        /// Plane List Access.
        /// </summary>
        Plane_List_Access,

        /// <summary>
        /// Plane Read.
        /// </summary>
        Plane_Read,

        /// <summary>
        /// Plane Save.
        /// </summary>
        Plane_Save,

        /// <summary>
        /// Plane Update.
        /// </summary>
        Plane_Update,

        /// <summary>
        /// Plane Fix.
        /// </summary>
        Plane_Fix,

        /// <summary>
        /// Engine Create.
        /// </summary>
        Engine_Create,

        /// <summary>
        /// Engine Delete.
        /// </summary>
        Engine_Delete,

        /// <summary>
        /// Engine List Access.
        /// </summary>
        Engine_List_Access,

        /// <summary>
        /// Engine Read.
        /// </summary>
        Engine_Read,

        /// <summary>
        /// Engine Save.
        /// </summary>
        Engine_Save,

        /// <summary>
        /// Engine Update.
        /// </summary>
        Engine_Update,

        /// <summary>
        /// Engine Fix.
        /// </summary>
        Engine_Fix,

        /// <summary>
        /// Aircraft Maintenance Company Create.
        /// </summary>
        AircraftMaintenanceCompany_Create,

        /// <summary>
        /// Aircraft Maintenance Company Delete.
        /// </summary>
        AircraftMaintenanceCompany_Delete,

        /// <summary>
        /// Aircraft Maintenance Company List Access.
        /// </summary>
        AircraftMaintenanceCompany_List_Access,

        /// <summary>
        /// Aircraft Maintenance Company Read.
        /// </summary>
        AircraftMaintenanceCompany_Read,

        /// <summary>
        /// Aircraft Maintenance Company Save.
        /// </summary>
        AircraftMaintenanceCompany_Save,

        /// <summary>
        /// Aircraft Maintenance Company Update.
        /// </summary>
        AircraftMaintenanceCompany_Update,

        /// <summary>
        /// Aircraft Maintenance Company Member Update.
        /// </summary>
        AircraftMaintenanceCompany_Member_Update,

        /// <summary>
        /// Aircraft Maintenance Company Member Delete.
        /// </summary>
        AircraftMaintenanceCompany_Member_Delete,

        /// <summary>
        /// Aircraft Maintenance Company Member Create.
        /// </summary>
        AircraftMaintenanceCompany_Member_Create,

        /// <summary>
        /// Aircraft Maintenance Company Member List Access.
        /// </summary>
        AircraftMaintenanceCompany_Member_List_Access,

        /// <summary>
        /// Aircraft Maintenance Company View Add Team View.
        /// </summary>
        AircraftMaintenanceCompany_View_Add_TeamView,

        /// <summary>
        /// Aircraft Maintenance Company View Update Team View.
        /// </summary>
        AircraftMaintenanceCompany_View_Update_TeamView,

        /// <summary>
        /// Aircraft Maintenance Company View Set Default Team View.
        /// </summary>
        AircraftMaintenanceCompany_View_Set_Default_TeamView,

        /// <summary>
        /// Aircraft Maintenance Company View Assign To Team.
        /// </summary>
        AircraftMaintenanceCompany_View_Assign_To_Team,

        /// <summary>
        /// Maintenance Team Create.
        /// </summary>
        MaintenanceTeam_Create,

        /// <summary>
        /// Maintenance Team Delete.
        /// </summary>
        MaintenanceTeam_Delete,

        /// <summary>
        /// Maintenance Team List Access.
        /// </summary>
        MaintenanceTeam_List_Access,

        /// <summary>
        /// Maintenance Team Read.
        /// </summary>
        MaintenanceTeam_Read,

        /// <summary>
        /// Maintenance Team Save.
        /// </summary>
        MaintenanceTeam_Save,

        /// <summary>
        /// Maintenance Team Update.
        /// </summary>
        MaintenanceTeam_Update,

        /// <summary>
        /// Maintenance Team Member Update.
        /// </summary>
        MaintenanceTeam_Member_Update,

        /// <summary>
        /// Maintenance Team Member Delete.
        /// </summary>
        MaintenanceTeam_Member_Delete,

        /// <summary>
        /// Maintenance Team Member Create.
        /// </summary>
        MaintenanceTeam_Member_Create,

        /// <summary>
        /// Maintenance Team Member List Access.
        /// </summary>
        MaintenanceTeam_Member_List_Access,

        /// <summary>
        /// Maintenance Team View Add Team View.
        /// </summary>
        MaintenanceTeam_View_Add_TeamView,

        /// <summary>
        /// Maintenance Team View Update Team View.
        /// </summary>
        MaintenanceTeam_View_Update_TeamView,

        /// <summary>
        /// Maintenance Team View Set Default Team View.
        /// </summary>
        MaintenanceTeam_View_Set_Default_TeamView,

        /// <summary>
        /// Maintenance Team View Assign To Team.
        /// </summary>
        MaintenanceTeam_View_Assign_To_Team,

        /// <summary>
        /// Maintenance Team Fix.
        /// </summary>
        MaintenanceTeam_Fix,

        /// <summary>
        /// Pilot Create.
        /// </summary>
        Pilot_Create,

        /// <summary>
        /// Pilot Delete.
        /// </summary>
        Pilot_Delete,

        /// <summary>
        /// Pilot List Access.
        /// </summary>
        Pilot_List_Access,

        /// <summary>
        /// Pilot Read.
        /// </summary>
        Pilot_Read,

        /// <summary>
        /// Pilot Save.
        /// </summary>
        Pilot_Save,

        /// <summary>
        /// Pilot Update.
        /// </summary>
        Pilot_Update,

        /// <summary>
        /// Pilot Fix.
        /// </summary>
        Pilot_Fix,

        /// <summary>
        /// Flight Create.
        /// </summary>
        Flight_Create,

        /// <summary>
        /// Flight Delete.
        /// </summary>
        Flight_Delete,

        /// <summary>
        /// Flight List Access.
        /// </summary>
        Flight_List_Access,

        /// <summary>
        /// Flight Read.
        /// </summary>
        Flight_Read,

        /// <summary>
        /// Flight Save.
        /// </summary>
        Flight_Save,

        /// <summary>
        /// Flight Update.
        /// </summary>
        Flight_Update,

        /// <summary>
        /// Flight Fix.
        /// </summary>
        Flight_Fix,

        /// <summary>
        /// Airport Create.
        /// </summary>
        Airport_Create,

        /// <summary>
        /// Airport Delete.
        /// </summary>
        Airport_Delete,

        /// <summary>
        /// Airport List Access.
        /// </summary>
        Airport_List_Access,

        /// <summary>
        /// Airport Read.
        /// </summary>
        Airport_Read,

        /// <summary>
        /// Airport Save.
        /// </summary>
        Airport_Save,

        /// <summary>
        /// Airport Update.
        /// </summary>
        Airport_Update,

        /// <summary>
        /// Plane Type Create.
        /// </summary>
        PlaneType_Create,

        /// <summary>
        /// Plane Type Delete.
        /// </summary>
        PlaneType_Delete,

        /// <summary>
        /// Plane Type List Access.
        /// </summary>
        PlaneType_List_Access,

        /// <summary>
        /// Plane Type Read.
        /// </summary>
        PlaneType_Read,

        /// <summary>
        /// Plane Type Save.
        /// </summary>
        PlaneType_Save,

        /// <summary>
        /// Plane Type Update.
        /// </summary>
        PlaneType_Update,

        /// <summary>
        /// Maintenance Contract Create.
        /// </summary>
        MaintenanceContract_Create,

        /// <summary>
        /// Maintenance Contract Delete.
        /// </summary>
        MaintenanceContract_Delete,

        /// <summary>
        /// Maintenance Contract List Access.
        /// </summary>
        MaintenanceContract_List_Access,

        /// <summary>
        /// Maintenance Contract Read.
        /// </summary>
        MaintenanceContract_Read,

        /// <summary>
        /// Maintenance Contract Save.
        /// </summary>
        MaintenanceContract_Save,

        /// <summary>
        /// Maintenance Contract Update.
        /// </summary>
        MaintenanceContract_Update,

        // ========== OPTIONS PERMISSIONS (1000+) ==========
        // Options permissions for dropdown lists and autocomplete
        // Starting at 1000 to clearly separate from regular CRUD permissions

        /// <summary>
        /// Site Options.
        /// </summary>
        Site_Options = 1000,

        /// <summary>
        /// Country Options.
        /// </summary>
        Country_Options,

        /// <summary>
        /// Airport Options.
        /// </summary>
        Airport_Options,

        /// <summary>
        /// Plane Options.
        /// </summary>
        Plane_Options,

        /// <summary>
        /// Plane Type Options.
        /// </summary>
        PlaneType_Options,

        /// <summary>
        /// Aircraft Maintenance Company Options.
        /// </summary>
        AircraftMaintenanceCompany_Options,

        /// <summary>
        /// Announcement Type Options.
        /// </summary>
        AnnouncementType_Options,

        /// <summary>
        /// Hangfire Run Worker.
        /// </summary>
        Hangfire_Run_Worker,
    }
}
