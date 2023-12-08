// BIADemo only
// <copyright file="AircraftMaintenanceCompanyAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.AircraftMaintenanceCompany
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompanyModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;

    /// <summary>
    /// The application service used for AircraftMaintenanceCompany.
    /// </summary>
    public class AircraftMaintenanceCompanyAppService : CrudAppServiceBase<AircraftMaintenanceCompanyDto, AircraftMaintenanceCompany, int, PagingFilterFormatDto, AircraftMaintenanceCompanyMapper>, IAircraftMaintenanceCompanyAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AircraftMaintenanceCompanyAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public AircraftMaintenanceCompanyAppService(ITGenericRepository<AircraftMaintenanceCompany, int> repository, IPrincipal principal)
            : base(repository)
        {
            var userData = (principal as BIAClaimsPrincipal).GetUserData<UserDataDto>();
            var currentAircraftMaintenanceCompanyId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.AircraftMaintenanceCompany) : 0;

            this.userPermissions = (principal as BIAClaimsPrincipal).GetUserPermissions();
            bool accessAll = this.userPermissions?.Any(x => x == Rights.Teams.AccessAll) == true;
            this.userId = (principal as BIAClaimsPrincipal).GetUserId();

            // You can see evrey team if your are member
            // For AircraftMaintenanceCompany we add
            //          - right for privilate acces (AccessAll) = Admin
            //          - right for member of a child team
            this.FiltersContext.Add(
                AccessMode.Read,
                new DirectSpecification<AircraftMaintenanceCompany>(
                    p => accessAll
                    || p.Members.Any(m => m.UserId == this.userId)

                    // You should add here link relation to member of child teams if there is child teams. (ex : || p.ChildTeams.Any(child => child.Members.Any(m => m.UserId == this.userId))
                    // Begin Child MaintenanceTeam
                    || p.MaintenanceTeams.Any(child => child.Members.Any(m => m.UserId == this.userId))

                    // End Child MaintenanceTeam
                    ));

            // In teams the right in jwt depends on current teams. So you should ensure that you are working on current team.
            this.FiltersContext.Add(
                AccessMode.Update,
                new DirectSpecification<AircraftMaintenanceCompany>(p => p.Id == currentAircraftMaintenanceCompanyId));
        }
    }
}