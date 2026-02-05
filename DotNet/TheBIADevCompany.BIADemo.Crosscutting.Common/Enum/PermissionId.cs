// <copyright file="PermissionId.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Crosscutting.Common.Enum
{
    /// <summary>
    /// Permission identifiers for BIADemo project CRUD operations (ordinales for JWT compaction).
    /// IMPORTANT: Keep enum member order in sync with Permission enum in Angular/src/app/shared/permission.ts
    /// When adding new permissions:
    /// 1. Add to both Permission.ts (Angular) and this enum (C#) in the SAME ORDER
    /// 2. DO NOT specify explicit values for regular permissions - order is implicit (0, 1, 2, ...)
    /// 3. These permissions start at ID 2000 (Project CRUD range: 2000+)
    /// 4. Options permissions are in a separate enum: OptionPermissionId (IDs 1000-1999)
    /// 5. If out of sync, fallback to string claims ensures zero auth breakage, but watch logs for warnings
    /// </summary>
    public enum PermissionId
    {
        /// <summary>
        /// Home Access.
        /// </summary>
        Home_Access = 2000,

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
        /// Site Member Read.
        /// </summary>
        Site_Member_Read,

        /// <summary>
        /// Site Member Update.
        /// </summary>
        Site_Member_Update,

        /// <summary>
        /// Site Member Save.
        /// </summary>
        Site_Member_Save,

        /// <summary>
        /// Site View Add Team View.
        /// </summary>
        Site_View_Add_TeamView,

        /// <summary>
        /// Site View Update Team View.
        /// </summary>
        Site_View_Update_TeamView,

        /// <summary>
        /// Site View Set Default Team View.
        /// </summary>
        Site_View_Set_Default_TeamView,

        /// <summary>
        /// Site View Assign To Team.
        /// </summary>
        Site_View_Assign_To_Team,

        /// <summary>
        /// Site Access All.
        /// </summary>
        Site_Access_All,

        /// <summary>
        /// Hangfire Access.
        /// </summary>
        Hangfire_Access,

        /// <summary>
        /// Hangfire Run Worker.
        /// </summary>
        Hangfire_Run_Worker,

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
        /// Aircraft Maintenance Company Member Save.
        /// </summary>
        AircraftMaintenanceCompany_Member_Save,

        /// <summary>
        /// Aircraft Maintenance Company Member Create.
        /// </summary>
        AircraftMaintenanceCompany_Member_Create,

        /// <summary>
        /// Aircraft Maintenance Company Member Read.
        /// </summary>
        AircraftMaintenanceCompany_Member_Read,

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
        /// Maintenance Team Member Save.
        /// </summary>
        MaintenanceTeam_Member_Save,

        /// <summary>
        /// Maintenance Team Member Create.
        /// </summary>
        MaintenanceTeam_Member_Create,

        /// <summary>
        /// Maintenance Team Member Read.
        /// </summary>
        MaintenanceTeam_Member_Read,

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
    }
}
