// BIADemo only
// <copyright file="MaintenanceTeamAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.AircraftMaintenanceCompany
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
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
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The application service used for AircraftMaintenanceCompany.
    /// </summary>
    public class MaintenanceTeamAppService : CrudAppServiceBase<MaintenanceTeamDto, MaintenanceTeam, int, PagingFilterFormatDto, MaintenanceTeamMapper>, IMaintenanceTeamAppService
    {
        /// <summary>
        /// The current Aircraft Maintenance Company Id.
        /// </summary>
        private readonly int currentAircraftMaintenanceCompanyId;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintenanceTeamAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public MaintenanceTeamAppService(ITGenericRepository<MaintenanceTeam, int> repository, IPrincipal principal)
            : base(repository)
        {
            var userData = (principal as BIAClaimsPrincipal).GetUserData<UserDataDto>();
            this.currentAircraftMaintenanceCompanyId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.AircraftMaintenanceCompany) : 0;
            var currentMaintenanceTeamyId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.MaintenanceTeam) : 0;

            IEnumerable<string> currentUserPermissions = (principal as BIAClaimsPrincipal).GetUserPermissions();
            bool accessAll = currentUserPermissions?.Any(x => x == Rights.MaintenanceTeams.ListViewAll) == true;
            int userId = (principal as BIAClaimsPrincipal).GetUserId();

            // You can see every team if your are member
            // For MaintenanceTeam we add
            //          - filter on current AircraftMaintenanceCompany to see only MaintenanceTeam of the current AircraftMaintenanceCompany
            //          - right for privilate acces (ListViewAll) = Admin and Supervisor of the Parent team (AircraftMaintenanceCompany)
            //          - right for member of the current AircraftMaintenanceCompany
            Specification<MaintenanceTeam> specification = new DirectSpecification<MaintenanceTeam>(team => accessAll || team.Members.Any(m => m.UserId == userId));
            var teamConfig = TeamConfig.Config[TeamTypeId.MaintenanceTeam];
            if (teamConfig?.Parents != null)
            {
                // add teams if member of parent
                // TODO use TeamConfig
            }

            if (teamConfig?.Parents != null)
            {
                Specification<MaintenanceTeam> specificationCurrentIsOneOfTheParent = new FalseSpecification<MaintenanceTeam>();
                foreach (var parent in teamConfig.Parents)
                {
                    var currentParentId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.AircraftMaintenanceCompany) : 0;
                    specificationCurrentIsOneOfTheParent |= new DirectSpecification<MaintenanceTeam>(IsCorrectId<MaintenanceTeam>(parent.GetParent, currentParentId));
                }

                // filter on current parrent:
                specification &= specificationCurrentIsOneOfTheParent;
            }

            this.FiltersContext.Add(
                AccessMode.Read,
                specification);

            //this.FiltersContext.Add(
            //    AccessMode.Read,
            //    new DirectSpecification<MaintenanceTeam>(team =>

            //        // You should add here check of current parent teams if there is parent a team. (ex : team.ParentTeamId == this.currentParentTeamId && ( )
            //        // Begin Parent AircraftMaintenanceCompany
            //        team.AircraftMaintenanceCompanyId == this.currentAircraftMaintenanceCompanyId && (

            //            // End Parent AircraftMaintenanceCompany
            //            accessAll

            //            // You should add here link relation to member of parent teams if there is a parent team. (ex : || team.ParentTeam.Members.Any(m => m.UserId == userId))
            //            // Begin Parent AircraftMaintenanceCompany
            //            || team.AircraftMaintenanceCompany.Members.Any(m => m.UserId == userId)

            //            // End Parent AircraftMaintenanceCompany

            //            // You should add here link relation to member of child teams if there are child teams. (ex : || team.ChildTeams.Any(childTeam => childTeam.Members.Any(m => m.UserId == this.userId))
            //            || team.Members.Any(m => m.UserId == userId) /* Begin Parent AircraftMaintenanceCompany*/) /* End Parent AircraftMaintenanceCompany */));

            // In teams the right in jwt depends on current teams. So you should ensure that you are working on current team.
            this.FiltersContext.Add(
                AccessMode.Update,
                new DirectSpecification<MaintenanceTeam>(p => p.Id == currentMaintenanceTeamyId));
        }

        private static Expression<Func<TTeam, bool>> IsCorrectId<TTeam>(Expression<Func<Team, Team>> getTeam, int teamId)
            where TTeam : Team
        {
            var typeConverter = (Expression<Func<TTeam, Team>>)(team => (team as Team));
            var isCorrectId = (Expression<Func<Team, bool>>)(team => team.Id == teamId);

            return Combine(Combine(typeConverter, getTeam), isCorrectId);
        }

        private static Expression<Func<T1, T3>> Combine<T1, T2, T3>(
            Expression<Func<T1, T2>> first,
            Expression<Func<T2, T3>> second)
        {
            var param = Expression.Parameter(typeof(T1), "param");
            var body = Expression.Invoke(second, Expression.Invoke(first, param));
            return Expression.Lambda<Func<T1, T3>>(body, param);
        }

        /// <inheritdoc/>
        public override Task<MaintenanceTeamDto> AddAsync(MaintenanceTeamDto dto, string mapperMode = null)
        {
            dto.AircraftMaintenanceCompanyId = this.currentAircraftMaintenanceCompanyId;
            return base.AddAsync(dto, mapperMode);
        }
    }
}