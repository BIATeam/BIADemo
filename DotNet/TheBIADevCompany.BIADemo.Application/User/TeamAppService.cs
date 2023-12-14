// <copyright file="TeamAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompanyModule.Aggregate;
    // End BIADemo
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The application service used for team.
    /// </summary>
    public class TeamAppService : FilteredServiceBase<Team, int>, ITeamAppService
    {
        /// <summary>
        /// The claims principal.
        /// </summary>
        private readonly BIAClaimsPrincipal principal;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        /// <param name="userContext">The user context.</param>
        public TeamAppService(ITGenericRepository<Team, int> repository, IPrincipal principal)
            : base(repository)
        {
            this.principal = principal as BIAClaimsPrincipal;
        }

        /// <summary>
        /// Return options.
        /// </summary>
        /// <returns>List of OptionDto.</returns>
        /// <param name="teamTypeId">The team type id.</param>
        public Task<IEnumerable<OptionDto>> GetAllOptionsAsync()
        {
            return this.GetAllAsync<OptionDto, TeamOptionMapper>();
        }

        /// <inheritdoc cref="ITeamAppService.GetAllAsync"/>
        public async Task<IEnumerable<TeamDto>> GetAllAsync(int userId = 0, IEnumerable<string> userPermissions = null)
        {
            userPermissions = userPermissions != null ? userPermissions : this.principal.GetUserPermissions();
            userId = userId > 0 ? userId : this.principal.GetUserId();

            TeamMapper mapper = this.InitMapper<TeamDto, TeamMapper>();
            if (userPermissions?.Any(x => x == Rights.Teams.AccessAll) == true)
            {
                return await this.Repository.GetAllResultAsync(mapper.EntityToDto(userId));
            }
            else
            {
                Specification<Team> specification = new DirectSpecification<Team>(team => team.Members.Any(member => member.UserId == userId));

                foreach (var teamConfig in TeamConfig.Config)
                {
                    if (teamConfig.Value.Children != null)
                    {
                        foreach (var child in teamConfig.Value.Children)
                        {
                            specification |= new DirectSpecification<Team>(team => team.TeamTypeId == (int)teamConfig.Key)
                                          && new DirectSpecification<Team>(IsMemberOfATeam(child.GetChilds, userId));
                        }
                    }

                    if (teamConfig.Value.Parents != null)
                    {
                        foreach (var parent in teamConfig.Value.Parents)
                        {
                            specification |= new DirectSpecification<Team>(team => team.TeamTypeId == (int)teamConfig.Key)
                                          && new DirectSpecification<Team>(IsMemberOfTeam(parent.GetParent, userId));
                        }
                    }
                }

                return await this.Repository.GetAllResultAsync(
                    mapper.EntityToDto(userId),
                    specification: specification);
            }
        }

        private static Expression<Func<Team, bool>> IsMemberOfTeam(Expression<Func<Team, Team>> GetTeam, int userId)
        {
            var IsMember = (Expression<Func<Team, bool>>)(team => team.Members.Any(b => b.UserId == userId));

            return Combine(GetTeam, IsMember);
        }

        private static Expression<Func<Team, bool>> IsMemberOfATeam(Expression<Func<Team, IEnumerable<Team>>> GetTeams, int userId)
        {
            var IsMemberOfOne = (Expression<Func<IEnumerable<Team>, bool>>)(teams => teams.Any(a => a.Members.Any(b => b.UserId == userId)));

            return Combine(GetTeams, IsMemberOfOne);
        }

        private static Expression<Func<T1, T3>> Combine<T1, T2, T3>(
            Expression<Func<T1, T2>> first,
            Expression<Func<T2, T3>> second)
        {
            var param = Expression.Parameter(typeof(T1), "param");
            var body = Expression.Invoke(second, Expression.Invoke(first, param));
            return Expression.Lambda<Func<T1, T3>>(body, param);
        }
    }

}