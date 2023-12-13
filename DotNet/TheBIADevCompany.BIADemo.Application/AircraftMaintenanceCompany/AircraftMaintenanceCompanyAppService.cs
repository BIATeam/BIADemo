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

            var userPermissions = (principal as BIAClaimsPrincipal).GetUserPermissions();
            bool accessAll = userPermissions?.Any(x => x == Rights.Teams.AccessAll) == true;
            var userId = (principal as BIAClaimsPrincipal).GetUserId();

            // You can see evrey team if your are member
            // For AircraftMaintenanceCompany we add
            //          - right for privilate acces (AccessAll) = Admin
            //          - right for member of a child team
            this.FiltersContext.Add(
                AccessMode.Read,
                new DirectSpecification<AircraftMaintenanceCompany>(team =>

                    // You should add here check of current parent teams if there is parent a team. (ex : team.ParentTeamId == this.currentParentTeamId && ( )
                        accessAll

                        // You should add here link relation to member of child teams if there are child teams. (ex : || team.ChildTeams.Any(childTeam => childTeam.Members.Any(m => m.UserId == this.userId))
                        // Begin Child MaintenanceTeam
                        || team.MaintenanceTeams.Any(child => child.Members.Any(m => m.UserId == userId))

                        // End Child MaintenanceTeam
                        || team.Members.Any(m => m.UserId == userId)));

            // In teams the right in jwt depends on current teams. So you should ensure that you are working on current team.
            this.FiltersContext.Add(
                AccessMode.Update,
                new DirectSpecification<AircraftMaintenanceCompany>(p => p.Id == currentAircraftMaintenanceCompanyId));
        }
    }
}