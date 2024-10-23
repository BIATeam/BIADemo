// BIADemo only
// <copyright file="MaintenanceTeamDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.CustomAttribute;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;

    /// <summary>
    /// The DTO used to represent a MaintenanceTeamDto.
    /// </summary>
    [BiaDtoClass(AncestorTeam = "AircraftMaintenanceCompany")]
    public class MaintenanceTeamDto : TeamDto
    {
        /// <summary>
        /// Gets or sets the aircraft maintenance company.
        /// </summary>
        [BiaDtoField(IsParent = true, Required = true)]
        public int AircraftMaintenanceCompanyId { get; set; }

        /// <summary>
        /// Gets or sets the maintenance team code.
        /// </summary>
        [BiaDtoField(Required = false)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the maintenance team active indicator.
        /// </summary>
        [BiaDtoField(Required = true)]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the maintenance team is approved.
        /// </summary>
        [BiaDtoField(Required = false)]
        public bool? IsApproved { get; set; }

        /// <summary>
        /// Gets or sets the first operation date and time.
        /// </summary>
        [BiaDtoField(Type = "datetime", Required = true)]
        public DateTime FirstOperation { get; set; }

        /// <summary>
        /// Gets or sets the last flight operation and time.
        /// </summary>
        [BiaDtoField(Type = "datetime", Required = false)]
        public DateTime? LastOperation { get; set; }

        /// <summary>
        /// Gets or sets the approval date.
        /// </summary>
        [BiaDtoField(Type = "date", Required = false)]
        public DateOnly? ApprovedDate { get; set; }

        /// <summary>
        /// Gets or sets the next operation date.
        /// </summary>
        [BiaDtoField(Type = "date", Required = true)]
        public DateOnly NextOperation { get; set; }

        /// <summary>
        /// Gets or sets the max travel duration hour.
        /// </summary>
        [BiaDtoField(Type = "time", Required = false)]
        public TimeOnly? MaxTravelDuration { get; set; }

        /// <summary>
        /// Gets or sets the max operation duration hour.
        /// </summary>
        [BiaDtoField(Type = "time", Required = true)]
        public TimeOnly MaxOperationDuration { get; set; }

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
        /// Gets or sets the total of operation duration hours.
        /// </summary>
        [BiaDtoField(Required = true)]
        public double TotalOperationDuration { get; set; }

        /// <summary>
        /// Gets or sets the average of operation duration hours.
        /// </summary>
        [BiaDtoField(Required = false)]
        public double? AverageOperationDuration { get; set; }

        /// <summary>
        /// Gets or sets the total of travel duration hours.
        /// </summary>
        [BiaDtoField(Required = true)]
        public float TotalTravelDuration { get; set; }

        /// <summary>
        /// Gets or sets the average of travel duration hours.
        /// </summary>
        [BiaDtoField(Required = false)]
        public float? AverageTravelDuration { get; set; }

        /// <summary>
        /// Gets or sets the total of operation cost.
        /// </summary>
        [BiaDtoField(Required = true)]
        public decimal TotalOperationCost { get; set; }

        /// <summary>
        /// Gets or sets the average of operation cost.
        /// </summary>
        [BiaDtoField(Required = false)]
        public decimal? AverageOperationCost { get; set; }

        /// <summary>
        /// Gets or sets the current airport title.
        /// </summary>
        [BiaDtoField(ItemType = "Airport", Required = true)]
        public OptionDto CurrentAirport { get; set; }

        /// <summary>
        /// Gets or sets the list of operation airports.
        /// </summary>
        [BiaDtoField(ItemType = "Airport", Required = true)]
        public ICollection<OptionDto> OperationAirports { get; set; }

        /// <summary>
        /// Gets or sets the country title.
        /// </summary>
        [BiaDtoField(ItemType = "Country")]
        public OptionDto CurrentCountry { get; set; }

        /// <summary>
        /// Gets or sets the list of operation countries.
        /// </summary>
        [BiaDtoField(ItemType = "Country")]
        public ICollection<OptionDto> OperationCountries { get; set; }
    }
}