// <copyright file="PermissionId.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common
{
    /// <summary>
    /// Permission identifiers (ordinales for JWT compaction).
    /// IMPORTANT: Keep enum member order in sync with Permission enum in Angular/src/app/shared/permission.ts
    /// When adding new permissions:
    /// 1. Add to both Permission.ts (Angular) and this enum (C#) in the SAME ORDER
    /// 2. Add value and increment ordinale
    /// 3. If out of sync, fallback to string claims ensures zero auth breakage, but watch logs for "sync issue detected"
    /// </summary>
    public enum PermissionId
    {
        /// <summary>
        /// Hangfire Access.
        /// </summary>
        Hangfire_Access = 0,

        /// <summary>
        /// Home Access.
        /// </summary>
        Home_Access = 1,

        /// <summary>
        /// Plane Create.
        /// </summary>
        Plane_Create = 2,

        /// <summary>
        /// Plane Delete.
        /// </summary>
        Plane_Delete = 3,

        /// <summary>
        /// Plane List Access.
        /// </summary>
        Plane_List_Access = 4,

        /// <summary>
        /// Plane Read.
        /// </summary>
        Plane_Read = 5,

        /// <summary>
        /// Plane Save.
        /// </summary>
        Plane_Save = 6,

        /// <summary>
        /// Plane Update.
        /// </summary>
        Plane_Update = 7,

        /// <summary>
        /// Plane Fix.
        /// </summary>
        Plane_Fix = 8,

        /// <summary>
        /// Engine Create.
        /// </summary>
        Engine_Create = 9,

        /// <summary>
        /// Engine Delete.
        /// </summary>
        Engine_Delete = 10,

        /// <summary>
        /// Engine List Access.
        /// </summary>
        Engine_List_Access = 11,

        /// <summary>
        /// Engine Read.
        /// </summary>
        Engine_Read = 12,

        /// <summary>
        /// Engine Save.
        /// </summary>
        Engine_Save = 13,

        /// <summary>
        /// Engine Update.
        /// </summary>
        Engine_Update = 14,

        /// <summary>
        /// Engine Fix.
        /// </summary>
        Engine_Fix = 15,

        /// <summary>
        /// Aircraft Maintenance Company Create.
        /// </summary>
        AircraftMaintenanceCompany_Create = 16,

        /// <summary>
        /// Aircraft Maintenance Company Delete.
        /// </summary>
        AircraftMaintenanceCompany_Delete = 17,

        /// <summary>
        /// Aircraft Maintenance Company List Access.
        /// </summary>
        AircraftMaintenanceCompany_List_Access = 18,

        /// <summary>
        /// Aircraft Maintenance Company Read.
        /// </summary>
        AircraftMaintenanceCompany_Read = 19,

        /// <summary>
        /// Aircraft Maintenance Company Save.
        /// </summary>
        AircraftMaintenanceCompany_Save = 20,

        /// <summary>
        /// Aircraft Maintenance Company Update.
        /// </summary>
        AircraftMaintenanceCompany_Update = 21,

        /// <summary>
        /// Aircraft Maintenance Company Member Update.
        /// </summary>
        AircraftMaintenanceCompany_Member_Update = 22,

        /// <summary>
        /// Aircraft Maintenance Company Member Delete.
        /// </summary>
        AircraftMaintenanceCompany_Member_Delete = 23,

        /// <summary>
        /// Aircraft Maintenance Company Member Create.
        /// </summary>
        AircraftMaintenanceCompany_Member_Create = 24,

        /// <summary>
        /// Aircraft Maintenance Company Member List Access.
        /// </summary>
        AircraftMaintenanceCompany_Member_List_Access = 25,

        /// <summary>
        /// Aircraft Maintenance Company View Add Team View.
        /// </summary>
        AircraftMaintenanceCompany_View_AddTeamView = 26,

        /// <summary>
        /// Aircraft Maintenance Company View Update Team View.
        /// </summary>
        AircraftMaintenanceCompany_View_UpdateTeamView = 27,

        /// <summary>
        /// Aircraft Maintenance Company View Set Default Team View.
        /// </summary>
        AircraftMaintenanceCompany_View_SetDefaultTeamView = 28,

        /// <summary>
        /// Aircraft Maintenance Company View Assign To Team.
        /// </summary>
        AircraftMaintenanceCompany_View_AssignToTeam = 29,

        /// <summary>
        /// Maintenance Team Create.
        /// </summary>
        MaintenanceTeam_Create = 30,

        /// <summary>
        /// Maintenance Team Delete.
        /// </summary>
        MaintenanceTeam_Delete = 31,

        /// <summary>
        /// Maintenance Team List Access.
        /// </summary>
        MaintenanceTeam_List_Access = 32,

        /// <summary>
        /// Maintenance Team Read.
        /// </summary>
        MaintenanceTeam_Read = 33,

        /// <summary>
        /// Maintenance Team Save.
        /// </summary>
        MaintenanceTeam_Save = 34,

        /// <summary>
        /// Maintenance Team Update.
        /// </summary>
        MaintenanceTeam_Update = 35,

        /// <summary>
        /// Maintenance Team Member Update.
        /// </summary>
        MaintenanceTeam_Member_Update = 36,

        /// <summary>
        /// Maintenance Team Member Delete.
        /// </summary>
        MaintenanceTeam_Member_Delete = 37,

        /// <summary>
        /// Maintenance Team Member Create.
        /// </summary>
        MaintenanceTeam_Member_Create = 38,

        /// <summary>
        /// Maintenance Team Member List Access.
        /// </summary>
        MaintenanceTeam_Member_List_Access = 39,

        /// <summary>
        /// Maintenance Team View Add Team View.
        /// </summary>
        MaintenanceTeam_View_AddTeamView = 40,

        /// <summary>
        /// Maintenance Team View Update Team View.
        /// </summary>
        MaintenanceTeam_View_UpdateTeamView = 41,

        /// <summary>
        /// Maintenance Team View Set Default Team View.
        /// </summary>
        MaintenanceTeam_View_SetDefaultTeamView = 42,

        /// <summary>
        /// Maintenance Team View Assign To Team.
        /// </summary>
        MaintenanceTeam_View_AssignToTeam = 43,

        /// <summary>
        /// Maintenance Team Fix.
        /// </summary>
        MaintenanceTeam_Fix = 44,

        /// <summary>
        /// Pilot Create.
        /// </summary>
        Pilot_Create = 45,

        /// <summary>
        /// Pilot Delete.
        /// </summary>
        Pilot_Delete = 46,

        /// <summary>
        /// Pilot List Access.
        /// </summary>
        Pilot_List_Access = 47,

        /// <summary>
        /// Pilot Read.
        /// </summary>
        Pilot_Read = 48,

        /// <summary>
        /// Pilot Save.
        /// </summary>
        Pilot_Save = 49,

        /// <summary>
        /// Pilot Update.
        /// </summary>
        Pilot_Update = 50,

        /// <summary>
        /// Pilot Fix.
        /// </summary>
        Pilot_Fix = 51,

        /// <summary>
        /// Flight Create.
        /// </summary>
        Flight_Create = 52,

        /// <summary>
        /// Flight Delete.
        /// </summary>
        Flight_Delete = 53,

        /// <summary>
        /// Flight List Access.
        /// </summary>
        Flight_List_Access = 54,

        /// <summary>
        /// Flight Read.
        /// </summary>
        Flight_Read = 55,

        /// <summary>
        /// Flight Save.
        /// </summary>
        Flight_Save = 56,

        /// <summary>
        /// Flight Update.
        /// </summary>
        Flight_Update = 57,

        /// <summary>
        /// Flight Fix.
        /// </summary>
        Flight_Fix = 58,

        /// <summary>
        /// Airport Create.
        /// </summary>
        Airport_Create = 59,

        /// <summary>
        /// Airport Delete.
        /// </summary>
        Airport_Delete = 60,

        /// <summary>
        /// Airport List Access.
        /// </summary>
        Airport_List_Access = 61,

        /// <summary>
        /// Airport Read.
        /// </summary>
        Airport_Read = 62,

        /// <summary>
        /// Airport Save.
        /// </summary>
        Airport_Save = 63,

        /// <summary>
        /// Airport Update.
        /// </summary>
        Airport_Update = 64,

        /// <summary>
        /// Plane Type Create.
        /// </summary>
        PlaneType_Create = 65,

        /// <summary>
        /// Plane Type Delete.
        /// </summary>
        PlaneType_Delete = 66,

        /// <summary>
        /// Plane Type List Access.
        /// </summary>
        PlaneType_List_Access = 67,

        /// <summary>
        /// Plane Type Read.
        /// </summary>
        PlaneType_Read = 68,

        /// <summary>
        /// Plane Type Save.
        /// </summary>
        PlaneType_Save = 69,

        /// <summary>
        /// Plane Type Update.
        /// </summary>
        PlaneType_Update = 70,

        /// <summary>
        /// Maintenance Contract Create.
        /// </summary>
        MaintenanceContract_Create = 71,

        /// <summary>
        /// Maintenance Contract Delete.
        /// </summary>
        MaintenanceContract_Delete = 72,

        /// <summary>
        /// Maintenance Contract List Access.
        /// </summary>
        MaintenanceContract_List_Access = 73,

        /// <summary>
        /// Maintenance Contract Read.
        /// </summary>
        MaintenanceContract_Read = 74,

        /// <summary>
        /// Maintenance Contract Save.
        /// </summary>
        MaintenanceContract_Save = 75,

        /// <summary>
        /// Maintenance Contract Update.
        /// </summary>
        MaintenanceContract_Update = 76,

        /// <summary>
        /// Site Create.
        /// </summary>
        Site_Create = 77,

        /// <summary>
        /// Site Delete.
        /// </summary>
        Site_Delete = 78,

        /// <summary>
        /// Site List Access.
        /// </summary>
        Site_List_Access = 79,

        /// <summary>
        /// Site Read.
        /// </summary>
        Site_Read = 80,

        /// <summary>
        /// Site Save.
        /// </summary>
        Site_Save = 81,

        /// <summary>
        /// Site Update.
        /// </summary>
        Site_Update = 82,

        /// <summary>
        /// Site Member Create.
        /// </summary>
        Site_Member_Create = 83,

        /// <summary>
        /// Site Member Delete.
        /// </summary>
        Site_Member_Delete = 84,

        /// <summary>
        /// Site Member List Access.
        /// </summary>
        Site_Member_List_Access = 85,

        /// <summary>
        /// Site Member Update.
        /// </summary>
        Site_Member_Update = 86,

        /// <summary>
        /// Site Member Save.
        /// </summary>
        Site_Member_Save = 87,
    }
}
