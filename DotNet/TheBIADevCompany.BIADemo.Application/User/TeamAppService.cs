// <copyright file="TeamAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
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
                                          && new DirectSpecification<Team>(IsMemberOfOneOfTeams(child.GetChilds, userId));
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

        public static Specification<TTeam> ReadSpecification<TTeam>(TeamTypeId teamTypeId, IPrincipal principal)
            where TTeam : Team
        {
            var userData = (principal as BIAClaimsPrincipal).GetUserData<UserDataDto>();
            IEnumerable<string> currentUserPermissions = (principal as BIAClaimsPrincipal).GetUserPermissions();
            bool accessAll = currentUserPermissions?.Any(x => x == Rights.Teams.AccessAll) == true;
            int userId = (principal as BIAClaimsPrincipal).GetUserId();

            return ReadSpecification<TTeam>(teamTypeId, userData, accessAll, userId);
        }

        public static Specification<TTeam> ReadSpecification<TTeam>(TeamTypeId teamTypeId, UserDataDto userData, bool viewAll, int userId)
             where TTeam : Team
        {

            // You can a team if your are member or have the viewAll permission
            Specification<TTeam> specification = new DirectSpecification<TTeam>(team => viewAll || team.Members.Any(m => m.UserId == userId));
            var teamConfig = TeamConfig.Config[teamTypeId];
            if (teamConfig?.Parents != null)
            {
                // You can see a team if member of parent
                foreach (var parent in teamConfig.Parents)
                {
                    specification |= new DirectSpecification<TTeam>(IsMemberOfTeam(Convert<TTeam>(parent.GetParent), userId));
                }
            }

            if (teamConfig?.Children != null)
            {
                // You can see a team if member of one child
                foreach (var child in teamConfig.Children)
                {
                    specification |= new DirectSpecification<TTeam>(IsMemberOfOneOfTeams(Convert<TTeam>(child.GetChilds), userId));
                }
            }

            if (teamConfig?.Parents != null)
            {
                Specification<TTeam> specificationCurrentIsOneOfTheParent = new FalseSpecification<TTeam>();
                foreach (var parent in teamConfig.Parents)
                {
                    var currentParentId = userData != null ? userData.GetCurrentTeamId((int)parent.TypeId) : 0;
                    specificationCurrentIsOneOfTheParent |= new DirectSpecification<TTeam>(IsCorrectId(Convert<TTeam>(parent.GetParent), currentParentId));
                }

                // We filter the list on current parrents:
                specification &= specificationCurrentIsOneOfTheParent;
            }

            return specification;
        }

        public static Specification<TTeam> UpdateSpecification<TTeam>(TeamTypeId teamTypeId, IPrincipal principal)
            where TTeam : Team
        {
            var userData = (principal as BIAClaimsPrincipal).GetUserData<UserDataDto>();
            var currentTeamId = userData != null ? userData.GetCurrentTeamId((int)teamTypeId) : 0;
            return new DirectSpecification<TTeam>(p => p.Id == currentTeamId);
        }

        public static Expression<Func<TTeam, IEnumerable<Team>>> Convert<TTeam>(Expression<Func<Team, IEnumerable<Team>>> getTeams)
            where TTeam : Team
        {
            var typeConverter = (Expression<Func<TTeam, Team>>)(team => (team as Team));

            return Combine(typeConverter, getTeams);
        }

        public static Expression<Func<TTeam, Team>> Convert<TTeam>(Expression<Func<Team, Team>> getTeam)
            where TTeam : Team
        {
            var typeConverter = (Expression<Func<TTeam, Team>>)(team => (team as Team));

            return Combine(typeConverter, getTeam);
        }

        public static Expression<Func<TTeam, bool>> IsMemberOfTeam<TTeam>(Expression<Func<TTeam, Team>> getTeam, int userId)
            where TTeam : Team
        {
            var isMember = (Expression<Func<Team, bool>>)(team => team.Members.Any(b => b.UserId == userId));

            return Combine(getTeam, isMember);
        }

        public static Expression<Func<TTeam, bool>> IsMemberOfOneOfTeams<TTeam>(Expression<Func<TTeam, IEnumerable<Team>>> getTeams, int userId)
             where TTeam : Team
        {
            var isMemberOfOne = (Expression<Func<IEnumerable<Team>, bool>>)(teams => teams.Any(a => a.Members.Any(b => b.UserId == userId)));

            return Combine(getTeams, isMemberOfOne);
        }

        public static Expression<Func<TTeam, bool>> IsCorrectId<TTeam>(Expression<Func<TTeam, Team>> getTeam, int teamId)
            where TTeam : Team
        {
            var isCorrectId = (Expression<Func<Team, bool>>)(team => team.Id == teamId);

            return Combine(getTeam, isCorrectId);
        }

        public static Expression<Func<T1, T3>> Combine<T1, T2, T3>(
            Expression<Func<T1, T2>> first,
            Expression<Func<T2, T3>> second)
        {
            var param = Expression.Parameter(typeof(T1), "param");
            var body = Expression.Invoke(second, Expression.Invoke(first, param));
            return Expression.Lambda<Func<T1, T3>>(body, param);
        }
    }
}