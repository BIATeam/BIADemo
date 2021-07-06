// <copyright file="ViewDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.View
{
    using System.Collections.Generic;

    /// <summary>
    /// The DTO used for views.
    /// </summary>
    public class ViewDto
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the table Id.
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// Gets or sets the view name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the view description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets view type.
        /// </summary>
        public int ViewType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this views is the default one for the user.
        /// </summary>
        public bool IsUserDefault { get; set; }

        /// <summary>
        /// Gets or sets the table preference.
        /// </summary>
        public string Preference { get; set; }

        /// <summary>
        /// Gets or sets the sites.
        /// </summary>
        public IList<ViewSiteDto> ViewSites { get; set; }
    }
}