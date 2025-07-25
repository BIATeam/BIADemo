// BIADemo only
// <copyright file="AircraftMaintenanceCompanyMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Maintenance.Mappers
{
    // Begin BIADemo
    using System;
    using System.Linq.Expressions;

    // End BIADemo
    using System.Security.Principal;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Bia.Base.Mappers;
    using TheBIADevCompany.BIADemo.Domain.Dto.Maintenance;
    using TheBIADevCompany.BIADemo.Domain.Maintenance.Entities;

    /// <summary>
    /// The mapper used for AircraftMaintenanceCompany.
    /// </summary>
    public class AircraftMaintenanceCompanyMapper : BaseTeamMapper<AircraftMaintenanceCompanyDto, AircraftMaintenanceCompany>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AircraftMaintenanceCompanyMapper"/> class.
        /// </summary>
        /// <param name="principal">The principal.</param>
        public AircraftMaintenanceCompanyMapper(IPrincipal principal)
            : base(principal)
        {
        }

        /// <inheritdoc />
        public override int TeamType => (int)TeamTypeId.AircraftMaintenanceCompany;

        // Begin BIADemo

        /// <inheritdoc />
        public override Expression<Func<AircraftMaintenanceCompany, object>>[] IncludesBeforeDelete()
        {
            return
            [
                x => x.MaintenanceTeams,
                x => x.MaintenanceContracts
            ];
        }

        // End BIADemo
    }
}