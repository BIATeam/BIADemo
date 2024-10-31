// BIADemo only
// <copyright file="MaintenanceTeam.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The AircraftMaintenanceCompany entity.
    /// </summary>
    public class MaintenanceTeam : Team
    {
        /// <summary>
        /// Gets or sets the aircraft maintenance company.
        /// </summary>
        public virtual AircraftMaintenanceCompany AircraftMaintenanceCompany { get; set; }

        /// <summary>
        /// Gets or sets the aircraft maintenance company id.
        /// </summary>
        public int AircraftMaintenanceCompanyId { get; set; }

        /// <summary>
        /// Add row version timestamp in table Site.
        /// </summary>
        [Timestamp]
        [Column("RowVersion")]
        public byte[] RowVersionAircraftMaintenanceCompany { get; set; }

        /// <summary>
        /// Gets or sets the maintenance team code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the maintenance team active indicator.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the maintenance team is approved.
        /// </summary>
        public bool? IsApproved { get; set; }

        /// <summary>
        /// Gets or sets the first operation date and time.
        /// </summary>
        public DateTime FirstOperation { get; set; }

        /// <summary>
        /// Gets or sets the last flight operation and time.
        /// </summary>
        public DateTime? LastOperation { get; set; }

        /// <summary>
        /// Gets or sets the approval date.
        /// </summary>
        public DateOnly? ApprovedDate { get; set; }

        /// <summary>
        /// Gets or sets the next operation date.
        /// </summary>
        public DateOnly NextOperation { get; set; }

        /// <summary>
        /// Gets or sets the max travel duration hour.
        /// </summary>
        public TimeOnly? MaxTravelDuration { get; set; }

        /// <summary>
        /// Gets or sets the max operation duration hour.
        /// </summary>
        public TimeOnly MaxOperationDuration { get; set; }

        /// <summary>
        /// Gets or sets the operation count.
        /// </summary>
        public int OperationCount { get; set; }

        /// <summary>
        /// Gets or sets the incident count.
        /// </summary>
        public int? IncidentCount { get; set; }

        /// <summary>
        /// Gets or sets the total of operation duration hours.
        /// </summary>
        public double TotalOperationDuration { get; set; }

        /// <summary>
        /// Gets or sets the average of operation duration hours.
        /// </summary>
        public double? AverageOperationDuration { get; set; }

        /// <summary>
        /// Gets or sets the total of travel duration hours.
        /// </summary>
        public float TotalTravelDuration { get; set; }

        /// <summary>
        /// Gets or sets the average of travel duration hours.
        /// </summary>
        public float? AverageTravelDuration { get; set; }

        /// <summary>
        /// Gets or sets the total of operation cost.
        /// </summary>
        public decimal TotalOperationCost { get; set; }

        /// <summary>
        /// Gets or sets the average of operation cost.
        /// </summary>
        public decimal? AverageOperationCost { get; set; }

        /// <summary>
        /// Gets or sets the current airport id.
        /// </summary>
        public int CurrentAirportId { get; set; }

        /// <summary>
        /// Gets or sets the current airport title.
        /// </summary>
        public Airport CurrentAirport { get; set; }

        /// <summary>
        /// Gets or sets the list of operation airports.
        /// </summary>
        public ICollection<Airport> OperationAirports { get; set; }

        /// <summary>
        /// Gets or sets the list of operation airports. Via jointure table.
        /// </summary>
        public ICollection<MaintenanceTeamAirport> OperationMaintenanceTeamAirports { get; set; }

        /// <summary>
        /// Gets or sets the current airport id.
        /// </summary>
        public int? CurrentCountryId { get; set; }

        /// <summary>
        /// Gets or sets the country title.
        /// </summary>
        public Country CurrentCountry { get; set; }

        /// <summary>
        /// Gets or sets the list of operation countries. Direct access.
        /// </summary>
        public ICollection<Country> OperationCountries { get; set; }

        /// <summary>
        /// Gets or sets the list of operation countries. Via jointure table.
        /// </summary>
        public ICollection<MaintenanceTeamCountry> OperationMaintenanceTeamCountries { get; set; }
    }
}