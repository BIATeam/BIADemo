// BIADemo only
// <copyright file="PlaneSpecificDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Plane
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.CustomAttribute;

    /// <summary>
    /// The DTO used to represent a plane.
    /// </summary>
    [BiaDtoClass(AncestorTeam = "Site")]
    public class PlaneSpecificDto : PlaneDto
    {
        /// <summary>
        /// Gets or sets the list of engines.
        /// </summary>
        [BiaDtoField(ItemType = "Engine", Required = true)]
        public ICollection<EngineDto> Engines { get; set; }
    }
}