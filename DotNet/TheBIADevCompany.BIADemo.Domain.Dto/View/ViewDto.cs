// <copyright file="ViewDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.View
{
    using System;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.CustomAttribute;
    using BIA.Net.Core.Domain.Dto.View;

    /// <summary>
    /// The DTO used to represent a view.
    /// </summary>
    public class ViewBDto : BaseDtoVersioned<int>
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [BiaDtoField(Required = false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [BiaDtoField(Required = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the preference.
        /// </summary>
        [BiaDtoField(Required = false)]
        public string Preference { get; set; }

        /// <summary>
        /// Gets or sets the table id.
        /// </summary>
        [BiaDtoField(Required = false)]
        public string TableId { get; set; }

        /// <summary>
        /// Gets or sets view type.
        /// </summary>
        [BiaDtoField(Required = true)]
        public int ViewType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this views is the default one for the user.
        /// </summary>
        [BiaDtoField(Required = false)]
        public bool IsUserDefault { get; set; }

        /// <summary>
        /// Gets or sets the sites.
        /// </summary>
        [BiaDtoField(Required = true)]
        public IList<ViewTeamDto> ViewTeams { get; set; }
    }
}
