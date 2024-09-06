// <copyright file="RoleId.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Crosscutting.Common.Enum
{
    /// <summary>
    /// The enumeration of all roles.
    /// </summary>
    public enum RoleId
    {
        /// <summary>
        /// The admin role identifier.
        /// </summary>
        Admin = 10001,

        /// <summary>
        /// The admin of the worker service role identifier.
        /// </summary>
        BackAdmin = 10002,

        /// <summary>
        /// The read only on worker service role identifier.
        /// </summary>
        BackReadOnly = 10003,

        /// <summary>
        /// The site admin role identifier.
        /// </summary>
        SiteAdmin = 1,

        // Begin BIADemo
#pragma warning disable SA1602 // Enumeration items should be documented
        Pilot = 2,

        Supervisor = 101,
        Expert = 102,

        TeamLeader = 201,
        Operator = 202,
#pragma warning restore SA1602 // Enumeration items should be documented

        // BIAToolKit - Begin Partial RoleId MaintenanceTeam

        /// <summary>
        /// The maintenanceTeam admin role identifier.
        /// </summary>
        // TODO after creation of team MaintenanceTeam : adapt the enum value
        MaintenanceTeamAdmin = 3,

        // BIAToolKit - End Partial RoleId MaintenanceTeam

        // End BIADemo
    }
}