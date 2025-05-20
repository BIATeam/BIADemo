// BIADemo only
// <copyright file="AircraftMaintenanceCompanyMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Maintenance.Mappers
{
    using System.Security.Principal;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.Maintenance;
    using TheBIADevCompany.BIADemo.Domain.Maintenance.Entities;
    using TheBIADevCompany.BIADemo.Domain.User.Mappers;

    /// <summary>
    /// The mapper used for AircraftMaintenanceCompany.
    /// </summary>
    public class AircraftMaintenanceCompanyMapper : TTeamMapper<AircraftMaintenanceCompanyDto, AircraftMaintenanceCompany>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AircraftMaintenanceCompanyMapper"/> class.
        /// </summary>
        /// <param name="principal">The principal.</param>
        public AircraftMaintenanceCompanyMapper(IPrincipal principal)
            : base(principal)
        {
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override int TeamType => (int)TeamTypeId.AircraftMaintenanceCompany;
    }
}