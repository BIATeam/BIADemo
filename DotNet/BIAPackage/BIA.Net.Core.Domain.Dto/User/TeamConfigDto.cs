// <copyright file="TeamConfigDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    using BIA.Net.Core.Common.Enum;

    /// <summary>
    /// The DTO used for the configuration of the teams in front end.
    /// </summary>
    public class TeamConfigDto
    {
        /// <summary>
        /// Gets or sets is the default value.
        /// </summary>
        public int TeamTypeId { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        public RoleMode RoleMode { get; set; }

        /// <summary>
        /// Gets or sets if appear in UI header (normaly should not be use in back).
        /// </summary>
        public bool InHeader { get; set; }

        /// <summary>
        /// Indicates weither the team selection can be empty or not.
        /// </summary>
        public bool TeamSelectionCanBeEmpty { get; set; }

        /// <summary>
        /// Gets or sets it the display of the team should be displayed one or not.
        /// </summary>
        public bool DisplayOne { get; set; }

        /// <summary>
        /// Gets or sets if the display of the team should be displayed always or not.
        /// </summary>
        public bool DisplayAlways { get; set; }

        /// <summary>
        /// Gets or sets the label key of the team.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets if the label should be displayed or not.
        /// </summary>
        public bool DisplayLabel { get; set; }
    }
}