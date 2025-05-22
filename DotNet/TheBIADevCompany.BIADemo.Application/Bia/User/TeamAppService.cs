// <copyright file="TeamAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Bia.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Specification;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Bia.User.Entities;
    using TheBIADevCompany.BIADemo.Domain.Bia.User.Mappers;
    using TheBIADevCompany.BIADemo.Domain.User;

    /// <summary>
    /// The application service used for team.
    /// </summary>
    public class TeamAppService : CrudAppServiceBase<BaseDtoVersionedTeam, Team, int, PagingFilterFormatDto, TeamMapper>, ITeamAppService
    {
        /// <summary>
        /// The claims principal.
        /// </summary>
        private readonly BiaClaimsPrincipal principal;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public TeamAppService(ITGenericRepository<Team, int> repository, IPrincipal principal)
            : base(repository)
        {
            this.principal = principal as BiaClaimsPrincipal;
        }

        /// <summary>
        /// Provide a standard specification to use in filter context read, in a team service constructor:
        /// this.FiltersContext.Add(AccessMode.Read,theStandardReadSpecification).
        /// </summary>
        /// <typeparam name="TTeam">the team type.</typeparam>
        /// <param name="teamTypeId">the team type id.</param>
        /// <param name="principal">the user claims.</param>
        /// <returns>the Standard Read Specification.</returns>
        public static Specification<TTeam> ReadSpecification<TTeam>(TeamTypeId teamTypeId, IPrincipal principal)
            where TTeam : Team
        {
            var userData = (principal as BiaClaimsPrincipal).GetUserData<UserDataDto>();
            IEnumerable<string> currentUserPermissions = (principal as BiaClaimsPrincipal).GetUserPermissions();
            bool accessAll = currentUserPermissions?.Any(x => x == Rights.Teams.AccessAll) == true;
            int userId = (principal as BiaClaimsPrincipal).GetUserId();

            return ReadSpecification<TTeam>(teamTypeId, userData, accessAll, userId);
        }

        /// <summary>
        /// Provide a standard specification to use in filter context read, in a team service constructor:
        /// this.FiltersContext.Add(AccessMode.Read,theStandardReadSpecification).
        /// </summary>
        /// <typeparam name="TTeam">the team type.</typeparam>
        /// <param name="teamTypeId">the team type id.</param>
        /// <param name="userData">the user data.</param>
        /// <param name="viewAll">flag that give access to all elements.</param>
        /// <param name="userId">the user id.</param>
        /// <returns>the Standard Read Specification.</returns>
        public static Specification<TTeam> ReadSpecification<TTeam>(TeamTypeId teamTypeId, UserDataDto userData, bool viewAll, int userId)
             where TTeam : Team
        {
            // You can a team if your are member or have the viewAll permission
            Specification<TTeam> specification = new DirectSpecification<TTeam>(team => viewAll || team.Members.Any(m => m.UserId == userId));
            var teamConfig = TeamConfig.Config.Find(tc => tc.TeamTypeId == (int)teamTypeId);
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
                    var currentParentId = userData != null ? userData.GetCurrentTeamId(parent.TeamTypeId) : 0;
                    specificationCurrentIsOneOfTheParent |= new DirectSpecification<TTeam>(IsCorrectId(Convert<TTeam>(parent.GetParent), currentParentId));
                }

                // We filter the list on current parrents:
                specification &= specificationCurrentIsOneOfTheParent;
            }

            return specification;
        }

        /// <summary>
        /// Provide a standard specification to use in filter context update, in a team service constructor:
        /// this.FiltersContext.Add(AccessMode.Update,theStandardUpdateSpecification).
        /// </summary>
        /// <typeparam name="TTeam">the team type.</typeparam>
        /// <param name="teamTypeId">the team type id.</param>
        /// <param name="principal">the user claims.</param>
        /// <returns>the Standard Update Specification.</returns>
        public static Specification<TTeam> UpdateSpecification<TTeam>(TeamTypeId teamTypeId, IPrincipal principal)
            where TTeam : Team
        {
            var userData = (principal as BiaClaimsPrincipal).GetUserData<UserDataDto>();
            var currentTeamId = userData != null ? userData.GetCurrentTeamId((int)teamTypeId) : 0;
            return new DirectSpecification<TTeam>(p => p.Id == currentTeamId);
        }

        /// <summary>
        /// Convert a Expression for specification.
        /// </summary>
        /// <typeparam name="TTeam">the team type.</typeparam>
        /// <param name="getTeams">the list of teams.</param>
        /// <returns>list of teams casted in Team.</returns>
        public static Expression<Func<TTeam, IEnumerable<Team>>> Convert<TTeam>(Expression<Func<Team, IEnumerable<Team>>> getTeams)
            where TTeam : Team
        {
            var typeConverter = (Expression<Func<TTeam, Team>>)(team => team as Team);

            return Combine(typeConverter, getTeams);
        }

        /// <summary>
        /// Convert a Expression for specification.
        /// </summary>
        /// <typeparam name="TTeam">the team type.</typeparam>
        /// <param name="getTeam">the team.</param>
        /// <returns>team casted in Team.</returns>
        public static Expression<Func<TTeam, Team>> Convert<TTeam>(Expression<Func<Team, Team>> getTeam)
            where TTeam : Team
        {
            var typeConverter = (Expression<Func<TTeam, Team>>)(team => team as Team);

            return Combine(typeConverter, getTeam);
        }

        /// <summary>
        /// Function IsMemberOfTeam for specification.
        /// </summary>
        /// <typeparam name="TTeam">the team type.</typeparam>
        /// <param name="getTeam">the team.</param>
        /// <param name="userId">the user Id.</param>
        /// <returns>expresion that return true if user is member of the team.</returns>
        public static Expression<Func<TTeam, bool>> IsMemberOfTeam<TTeam>(Expression<Func<TTeam, Team>> getTeam, int userId)
            where TTeam : Team
        {
            var isMember = (Expression<Func<Team, bool>>)(team => team.Members.Any(b => b.UserId == userId));

            return Combine(getTeam, isMember);
        }

        /// <summary>
        /// Function IsMemberOfOneOfTeams for specification.
        /// </summary>
        /// <typeparam name="TTeam">the team type.</typeparam>
        /// <param name="getTeams">the list of teams.</param>
        /// <param name="userId">the user Id.</param>
        /// <returns>expresion that return true if user is member of one of the teams.</returns>
        public static Expression<Func<TTeam, bool>> IsMemberOfOneOfTeams<TTeam>(Expression<Func<TTeam, IEnumerable<Team>>> getTeams, int userId)
             where TTeam : Team
        {
            var isMemberOfOne = (Expression<Func<IEnumerable<Team>, bool>>)(teams => teams.Any(a => a.Members.Any(b => b.UserId == userId)));

            return Combine(getTeams, isMemberOfOne);
        }

        /// <summary>
        /// Function IsCorrectId for specification.
        /// </summary>
        /// <typeparam name="TTeam">the team type.</typeparam>
        /// <param name="getTeam">the team.</param>
        /// <param name="teamId">the team Id to check.</param>
        /// <returns>expresion that return true if the team id correspond.</returns>
        public static Expression<Func<TTeam, bool>> IsCorrectId<TTeam>(Expression<Func<TTeam, Team>> getTeam, int teamId)
            where TTeam : Team
        {
            var isCorrectId = (Expression<Func<Team, bool>>)(team => team.Id == teamId);

            return Combine(getTeam, isCorrectId);
        }

        /// <summary>
        /// Function to combine 2 Expressions in an Expression.
        /// </summary>
        /// <typeparam name="T1">type of parameter in first expression.</typeparam>
        /// <typeparam name="T2">type of result in first expression and type of parameter in second expression.</typeparam>
        /// <typeparam name="T3">type of result in second expression.</typeparam>
        /// <param name="first">first expression.</param>
        /// <param name="second">second expression.</param>
        /// <returns>A combined expression with T1 as type of parameter and T3 as type of result.</returns>
        public static Expression<Func<T1, T3>> Combine<T1, T2, T3>(
            Expression<Func<T1, T2>> first,
            Expression<Func<T2, T3>> second)
        {
            var param = Expression.Parameter(typeof(T1), "param");
            var body = Expression.Invoke(second, Expression.Invoke(first, param));
            return Expression.Lambda<Func<T1, T3>>(body, param);
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
        public async Task<IEnumerable<BaseDtoVersionedTeam>> GetAllAsync(int userId = 0, IEnumerable<string> userPermissions = null)
        {
            userPermissions = userPermissions != null ? userPermissions : this.principal.GetUserPermissions();
            userId = userId > 0 ? userId : this.principal.GetUserId();

            TeamMapper mapper = this.InitMapper<BaseDtoVersionedTeam, TeamMapper>();
            if (userPermissions?.Any(x => x == Rights.Teams.AccessAll) == true)
            {
                return await this.Repository.GetAllResultAsync(mapper.EntityToDto(userId));
            }
            else
            {
                Specification<Team> specification = new DirectSpecification<Team>(team => team.Members.Any(member => member.UserId == userId));

                foreach (var teamConfig in TeamConfig.Config)
                {
                    if (teamConfig.Children != null)
                    {
                        foreach (var child in teamConfig.Children)
                        {
                            specification |= new DirectSpecification<Team>(team => team.TeamTypeId == teamConfig.TeamTypeId)
                                          && new DirectSpecification<Team>(IsMemberOfOneOfTeams(child.GetChilds, userId));
                        }
                    }

                    if (teamConfig.Parents != null)
                    {
                        foreach (var parent in teamConfig.Parents)
                        {
                            specification |= new DirectSpecification<Team>(team => team.TeamTypeId == teamConfig.TeamTypeId)
                                          && new DirectSpecification<Team>(IsMemberOfTeam(parent.GetParent, userId));
                        }
                    }
                }

                return await this.Repository.GetAllResultAsync(
                    mapper.EntityToDto(userId),
                    specification: specification);
            }
        }

        /// <inheritdoc/>
        public bool IsAuthorizeForTeamType(ClaimsPrincipal principal, TeamTypeId teamTypeId, int teamId, string roleSuffix)
        {
            var config = TeamConfig.Config.Find(tc => tc.TeamTypeId == (int)teamTypeId);
            if (config != null)
            {
                if (!principal.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == config.RightPrefix + roleSuffix))
                {
                    return false;
                }

                var userData = new BiaClaimsPrincipal(principal).GetUserData<UserDataDto>();
                if (userData.GetCurrentTeamId((int)teamTypeId) != teamId)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}