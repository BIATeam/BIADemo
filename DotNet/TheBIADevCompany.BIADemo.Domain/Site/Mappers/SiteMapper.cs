// <copyright file="SiteMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Site.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Principal;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Bia.User.Mappers;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;
    using TheBIADevCompany.BIADemo.Domain.Site.Entities;

    /// <summary>
    /// The mapper used for site.
    /// </summary>
    public class SiteMapper : TTeamMapper<SiteDto, Site>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapper"/> class.
        /// </summary>
        /// <param name="principal">The principal.</param>
        public SiteMapper(IPrincipal principal)
            : base(principal)
        {
        }

        /// <summary>
        /// Precise the Id of the type of team.
        /// </summary>
        public override int TeamType
        {
            get { return (int)TeamTypeId.Site; }
        }
    }
}