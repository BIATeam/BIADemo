// BIADemo only
// <copyright file="EngineDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Plane
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;

    /// <summary>
    /// The DTO used to represent a Engine.
    /// </summary>
    public class EngineDto : BaseDto<int>
    {
        /// <summary>
        /// Gets or sets the Manufacturer's Serial Number.
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets the last maintenance date and time.
        /// </summary>
        public DateTime LastMaintenanceDate { get; set; }

        /// <summary>
        /// Gets or sets the daily synchronisation hour.
        /// </summary>
        public string SyncTime { get; set; }

        /// <summary>
        /// Gets or sets the power.
        /// </summary>
        public int? Power { get; set; }

        /// <summary>
        /// Gets or sets the Plane.
        /// </summary>
        public int PlaneId { get; set; }
    }
}