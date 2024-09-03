// BIADemo only
// <copyright file="AircraftMaintenanceCompanyMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompanyModule.Aggregate
{
    using System.Security.Principal;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

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

        /// <summary>
        /// Precise the Id of the type of team.
        /// </summary>
        public override int TeamType
        {
            get { return (int)TeamTypeId.AircraftMaintenanceCompany; }
        }
    }
}