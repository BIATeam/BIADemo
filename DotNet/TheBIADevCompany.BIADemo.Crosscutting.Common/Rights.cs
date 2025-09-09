// <copyright file="Rights.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Crosscutting.Common
{
    /// <summary>
    /// The list of all rights.
    /// </summary>
    public static class Rights
    {
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

            /// <summary>
            /// The right to access to the list of sites (options only).
            /// </summary>
            public const string Options = "Site_Options";
        }

        // BIAToolKit - Begin Rights
        // BIAToolKit - End Rights

        // BIAToolKit - Begin RightsForOption
        // BIAToolKit - End RightsForOption

        // Begin BIAToolKit Generation Ignore
        // BIAToolKit - Begin Partial RightsForOption Country

        /// <summary>
        /// The country options rights.
        /// </summary>
        public static class CountryOptions
        {
            /// <summary>
            /// The right to access to the list of countries (options only).
            /// </summary>
            public const string Options = "Country_Options";
        }

        // BIAToolKit - End Partial RightsForOption Country

        // BIAToolKit - Begin Partial RightsForOption PlaneType

        /// <summary>
        /// The plane type options rights.
        /// </summary>
        public static class PlaneTypeOptions
        {
            /// <summary>
            /// The right to access to the list of plane types (options only).
            /// </summary>
            public const string Options = "PlaneType_Options";
        }

        // BIAToolKit - End Partial RightsForOption PlaneType

        // BIAToolKit - Begin Partial Rights Plane

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
            /// The right to create plane.
            /// </summary>
            public const string Create = "Plane_Create";

            /// <summary>
            /// The right to read plane.
            /// </summary>
            public const string Read = "Plane_Read";

            /// <summary>
            /// The right to update plane.
            /// </summary>
            public const string Update = "Plane_Update";

            /// <summary>
            /// The right to delete plane.
            /// </summary>
            public const string Delete = "Plane_Delete";

            /// <summary>
            /// The right to save plane.
            /// </summary>
            public const string Save = "Plane_Save";

            /// <summary>
            /// The right to fixe plane.
            /// </summary>
            public const string Fix = "Plane_Fix";
        }

        // BIAToolKit - End Partial Rights Plane

        // BIAToolKit - Begin Partial Rights Engine

        /// <summary>
        /// The engines rights.
        /// </summary>
        public static class Engines
        {
            /// <summary>
            /// The right to access to the list of engines.
            /// </summary>
            public const string ListAccess = "Engine_List_Access";

            /// <summary>
            /// The right to create engine.
            /// </summary>
            public const string Create = "Engine_Create";

            /// <summary>
            /// The right to read engine.
            /// </summary>
            public const string Read = "Engine_Read";

            /// <summary>
            /// The right to update engine.
            /// </summary>
            public const string Update = "Engine_Update";

            /// <summary>
            /// The right to delete engine.
            /// </summary>
            public const string Delete = "Engine_Delete";

            /// <summary>
            /// The right to save engine.
            /// </summary>
            public const string Save = "Engine_Save";

            /// <summary>
            /// The right to fixe engine.
            /// </summary>
            public const string Fix = "Engine_Fix";
        }

        // BIAToolKit - End Partial Rights Engine

        // BIAToolKit - Begin Partial Rights AircraftMaintenanceCompany

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
            /// The right to create aircraft maintenance company.
            /// </summary>
            public const string Create = "AircraftMaintenanceCompany_Create";

            /// <summary>
            /// The right to read aircraft maintenance company.
            /// </summary>
            public const string Read = "AircraftMaintenanceCompany_Read";

            /// <summary>
            /// The right to update aircraft maintenance company.
            /// </summary>
            public const string Update = "AircraftMaintenanceCompany_Update";

            /// <summary>
            /// The right to delete aircraft maintenance company.
            /// </summary>
            public const string Delete = "AircraftMaintenanceCompany_Delete";

            /// <summary>
            /// The right to save aircraft maintenance company.
            /// </summary>
            public const string Save = "AircraftMaintenanceCompany_Save";
        }

        // BIAToolKit - End Partial Rights AircraftMaintenanceCompany

        // BIAToolKit - Begin Partial Rights MaintenanceTeam

        /// <summary>
        /// The maintenance teams rights.
        /// </summary>
        public static class MaintenanceTeams
        {
            /// <summary>
            /// The right to access to the list of maintenance teams.
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

            /// <summary>
            /// The right to fixe maintenance team.
            /// </summary>
            public const string Fix = "MaintenanceTeam_Fix";
        }

        // BIAToolKit - End Partial Rights MaintenanceTeam
        // End BIAToolKit Generation Ignore

        // Begin BIADemo

        /// <summary>
        /// The countries rights.
        /// </summary>
        public static class AircraftMaintenanceCompanyOptions
        {
            /// <summary>
            /// The right to access to the list of aircraftMaintenanceCompanies (options only).
            /// </summary>
            public const string Options = "AircraftMaintenanceCompany_Options";
        }

        /// <summary>
        /// The countries rights.
        /// </summary>
        public static class Parts
        {
            /// <summary>
            /// The right to access to the list of parts (options only).
            /// </summary>
            public const string Options = "Part_Options";
        }

        /// <summary>
        /// The plane options rights.
        /// </summary>
        public static class PlaneOptions
        {
            /// <summary>
            /// The right to access to the list of planes (options only).
            /// </summary>
            public const string Options = "Plane_Options";
        }

        /// <summary>
        /// The planes rights.
        /// </summary>
        public static class PlanesTypes
        {
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

        /// <summary>
        /// The airports rights.
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
            /// The right to save airports.
            /// </summary>
            public const string Save = "Airport_Save";
        }

        /// <summary>
        /// The maintenanceContracts rights.
        /// </summary>
        public static class MaintenanceContracts
        {
            /// <summary>
            /// The right to access to the list of maintenanceContracts.
            /// </summary>
            public const string ListAccess = "MaintenanceContract_List_Access";

            /// <summary>
            /// The right to create maintenanceContracts.
            /// </summary>
            public const string Create = "MaintenanceContract_Create";

            /// <summary>
            /// The right to read maintenanceContracts.
            /// </summary>
            public const string Read = "MaintenanceContract_Read";

            /// <summary>
            /// The right to update maintenanceContracts.
            /// </summary>
            public const string Update = "MaintenanceContract_Update";

            /// <summary>
            /// The right to delete maintenanceContracts.
            /// </summary>
            public const string Delete = "MaintenanceContract_Delete";

            /// <summary>
            /// The right to save maintenanceContracts.
            /// </summary>
            public const string Save = "MaintenanceContract_Save";
        }

        // BIAToolKit - Begin Partial Rights Pilot

        /// <summary>
        /// The pilots rights.
        /// </summary>
        public static class Pilots
        {
            /// <summary>
            /// The right to access to the list of pilots.
            /// </summary>
            public const string ListAccess = "Pilot_List_Access";

            /// <summary>
            /// The right to create pilot.
            /// </summary>
            public const string Create = "Pilot_Create";

            /// <summary>
            /// The right to read pilot.
            /// </summary>
            public const string Read = "Pilot_Read";

            /// <summary>
            /// The right to update pilot.
            /// </summary>
            public const string Update = "Pilot_Update";

            /// <summary>
            /// The right to delete pilot.
            /// </summary>
            public const string Delete = "Pilot_Delete";

            /// <summary>
            /// The right to save pilot.
            /// </summary>
            public const string Save = "Pilot_Save";

            /// <summary>
            /// The right to fixe pilot.
            /// </summary>
            public const string Fix = "Pilot_Fix";
        }

        /// <summary>
        /// The flight rights.
        /// </summary>
        public static class Flights
        {
            /// <summary>
            /// The right to access to the list of flights.
            /// </summary>
            public const string ListAccess = "Flight_List_Access";

            /// <summary>
            /// The right to create flight.
            /// </summary>
            public const string Create = "Flight_Create";

            /// <summary>
            /// The right to read flight.
            /// </summary>
            public const string Read = "Flight_Read";

            /// <summary>
            /// The right to update flight.
            /// </summary>
            public const string Update = "Flight_Update";

            /// <summary>
            /// The right to delete flight.
            /// </summary>
            public const string Delete = "Flight_Delete";

            /// <summary>
            /// The right to save flight.
            /// </summary>
            public const string Save = "Flight_Save";

            /// <summary>
            /// The right to fixe flight.
            /// </summary>
            public const string Fix = "Flight_Fix";
        }

        // BIAToolKit - End Partial Rights Pilot

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
    }
}