// BIADemo only
// <copyright file="MaintenanceTeamDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Maintenance
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.CustomAttribute;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The DTO used to represent a maintenance team.
    /// </summary>
    [BiaDtoClass(AncestorTeam = "AircraftMaintenanceCompany")]
    public class MaintenanceTeamDto : BaseDtoVersionedTeamFixableArchivable
    {
        /// <summary>
        /// Gets or sets the aircraft maintenance company id.
        /// </summary>
        [BiaDtoField(Required = true, IsParent = true)]
        public int AircraftMaintenanceCompanyId { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        [BiaDtoField(Required = false)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the MaintenanceTeam is active.
        /// </summary>
        [BiaDtoField(Required = true)]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the MaintenanceTeam is approved.
        /// </summary>
        [BiaDtoField(Required = false)]
        public bool? IsApproved { get; set; }

        /// <summary>
        /// Gets or sets the first operation.
        /// </summary>
        [BiaDtoField(Required = true, Type = "datetime")]
        public DateTime FirstOperation { get; set; }

        /// <summary>
        /// Gets or sets the last operation.
        /// </summary>
        [BiaDtoField(Required = false, Type = "datetime")]
        public DateTime? LastOperation { get; set; }

        /// <summary>
        /// Gets or sets the approved date.
        /// </summary>
        [BiaDtoField(Required = false, Type = "date")]
        public DateTime? ApprovedDate { get; set; }

        /// <summary>
        /// Gets or sets the next operation.
        /// </summary>
        [BiaDtoField(Required = true, Type = "date")]
        public DateTime NextOperation { get; set; }

        /// <summary>
        /// Gets or sets the max travel duration.
        /// </summary>
        [BiaDtoField(Required = false, Type = "time")]
        public string MaxTravelDuration { get; set; }

        /// <summary>
        /// Gets or sets the max operation duration.
        /// </summary>
        [BiaDtoField(Required = true, Type = "time")]
        public string MaxOperationDuration { get; set; }

        /// <summary>
        /// Gets or sets the operation count.
        /// </summary>
        [BiaDtoField(Required = true)]
        public int OperationCount { get; set; }

        /// <summary>
        /// Gets or sets the incident count.
        /// </summary>
        [BiaDtoField(Required = false)]
        public int? IncidentCount { get; set; }

        /// <summary>
        /// Gets or sets the total operation duration.
        /// </summary>
        [BiaDtoField(Required = true)]
        public double TotalOperationDuration { get; set; }

        /// <summary>
        /// Gets or sets the average operation duration.
        /// </summary>
        [BiaDtoField(Required = false)]
        public double? AverageOperationDuration { get; set; }

        /// <summary>
        /// Gets or sets the total travel duration.
        /// </summary>
        [BiaDtoField(Required = true)]
        public float TotalTravelDuration { get; set; }

        /// <summary>
        /// Gets or sets the average travel duration.
        /// </summary>
        [BiaDtoField(Required = false)]
        public float? AverageTravelDuration { get; set; }

        /// <summary>
        /// Gets or sets the total operation cost.
        /// </summary>
        [BiaDtoField(Required = true)]
        public decimal TotalOperationCost { get; set; }

        /// <summary>
        /// Gets or sets the average operation cost.
        /// </summary>
        [BiaDtoField(Required = false)]
        public decimal? AverageOperationCost { get; set; }

        /// <summary>
        /// Gets or sets the current airport.
        /// </summary>
        [BiaDtoField(Required = true, ItemType = "Airport")]
        public OptionDto CurrentAirport { get; set; }

        /// <summary>
        /// Gets or sets the list of operation airports.
        /// </summary>
        [BiaDtoField(Required = true, ItemType = "Airport")]
        public ICollection<OptionDto> OperationAirports { get; set; }

        /// <summary>
        /// Gets or sets the current country.
        /// </summary>
        [BiaDtoField(Required = false, ItemType = "Country")]
        public OptionDto CurrentCountry { get; set; }

        /// <summary>
        /// Gets or sets the list of operation countries.
        /// </summary>
        [BiaDtoField(Required = false, ItemType = "Country")]
        public ICollection<OptionDto> OperationCountries { get; set; }
    }
}