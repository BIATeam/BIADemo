// <copyright file="BaseTeamAppService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.User
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Specification;
    using BIA.Net.Core.Domain.User.Entities;
    using BIA.Net.Core.Domain.User.Mappers;

    /// <summary>
    /// The application service used for team.
    /// </summary>
    /// <typeparam name="TEnumTeamTypeId">The type for enum Team Type Id.</typeparam>
    /// <typeparam name="TTeamMapper">The type of team Mapper.</typeparam>
    public class BaseTeamAppService<TEnumTeamTypeId, TTeamMapper> : CrudAppServiceBase<BaseDtoVersionedTeam, BaseEntityTeam, int, PagingFilterFormatDto, TTeamMapper>, IBaseTeamAppService<TEnumTeamTypeId>
        where TEnumTeamTypeId : struct, Enum
        where TTeamMapper : BiaBaseMapper<BaseDtoVersionedTeam, BaseEntityTeam, int>, ITeamMapper
    {
        /// <summary>
        /// The claims principal.
        /// </summary>
        private readonly BiaClaimsPrincipal principal;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTeamAppService{TEnumTeamTypeId, TTeamMapper}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public BaseTeamAppService(ITGenericRepository<BaseEntityTeam, int> repository, IPrincipal principal)
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
        /// <param name="teamsConfig">The teams configuration.</param>
        /// <returns>the Standard Read Specification.</returns>
        public static Specification<TTeam> ReadSpecification<TTeam>(TEnumTeamTypeId teamTypeId, IPrincipal principal, ImmutableList<BiaTeamConfig<BaseEntityTeam>> teamsConfig)
            where TTeam : class, IEntity<int>, IEntityTeam
        {
            var userData = (principal as BiaClaimsPrincipal).GetUserData<BaseUserDataDto>();
            IEnumerable<string> currentUserPermissions = (principal as BiaClaimsPrincipal).GetUserPermissions();
            bool accessAll = currentUserPermissions?.Any(x => x == BiaRights.Teams.AccessAll) == true;
            int userId = (principal as BiaClaimsPrincipal).GetUserId();

            return ReadSpecification<TTeam>(teamTypeId, userData, accessAll, userId, teamsConfig);
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
        /// <param name="teamsConfig">The teams configuration.</param>
        /// <returns>the Standard Read Specification.</returns>
        public static Specification<TTeam> ReadSpecification<TTeam>(TEnumTeamTypeId teamTypeId, BaseUserDataDto userData, bool viewAll, int userId, ImmutableList<BiaTeamConfig<BaseEntityTeam>> teamsConfig)
            where TTeam : class, IEntity<int>, IEntityTeam
        {
            // You can a team if your are member or have the viewAll permission
            Specification<TTeam> specification = new DirectSpecification<TTeam>(team => viewAll || team.Members.Any(m => m.UserId == userId));
            var teamConfig = teamsConfig.Find(tc => tc.TeamTypeId == System.Convert.ToInt32(teamTypeId));
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
        /// <typeparam name="TUserDataDto">The type of the user data dto.</typeparam>
        /// <param name="teamTypeId">the team type id.</param>
        /// <param name="principal">the user claims.</param>
        /// <returns>
        /// the Standard Update Specification.
        /// </returns>
        public static Specification<TTeam> UpdateSpecification<TTeam, TUserDataDto>(TEnumTeamTypeId teamTypeId, IPrincipal principal)
            where TTeam : class, IEntity<int>, IEntityTeam
            where TUserDataDto : BaseUserDataDto
        {
            var userData = (principal as BiaClaimsPrincipal).GetUserData<TUserDataDto>();
            var currentTeamId = userData != null ? userData.GetCurrentTeamId(System.Convert.ToInt32(teamTypeId)) : 0;
            return new DirectSpecification<TTeam>(p => p.Id == currentTeamId);
        }

        /// <summary>
        /// Convert a Expression for specification.
        /// </summary>
        /// <typeparam name="TTeam">the team type.</typeparam>
        /// <param name="getTeams">the list of teams.</param>
        /// <returns>list of teams casted in Team.</returns>
        public static Expression<Func<TTeam, IEnumerable<BaseEntityTeam>>> Convert<TTeam>(Expression<Func<BaseEntityTeam, IEnumerable<BaseEntityTeam>>> getTeams)
            where TTeam : class, IEntity<int>, IEntityTeam
        {
            var typeConverter = (Expression<Func<TTeam, BaseEntityTeam>>)(team => team as BaseEntityTeam);

            return Combine(typeConverter, getTeams);
        }

        /// <summary>
        /// Convert a Expression for specification.
        /// </summary>
        /// <typeparam name="TTeam">the team type.</typeparam>
        /// <param name="getTeam">the team.</param>
        /// <returns>team casted in Team.</returns>
        public static Expression<Func<TTeam, BaseEntityTeam>> Convert<TTeam>(Expression<Func<BaseEntityTeam, BaseEntityTeam>> getTeam)
            where TTeam : class, IEntity<int>, IEntityTeam
        {
            var typeConverter = (Expression<Func<TTeam, BaseEntityTeam>>)(team => team as BaseEntityTeam);

            return Combine(typeConverter, getTeam);
        }

        /// <summary>
        /// Function IsMemberOfTeam for specification.
        /// </summary>
        /// <typeparam name="TTeam">the team type.</typeparam>
        /// <param name="getTeam">the team.</param>
        /// <param name="userId">the user Id.</param>
        /// <returns>expresion that return true if user is member of the team.</returns>
        public static Expression<Func<TTeam, bool>> IsMemberOfTeam<TTeam>(Expression<Func<TTeam, BaseEntityTeam>> getTeam, int userId)
            where TTeam : class, IEntity<int>, IEntityTeam
        {
            var isMember = (Expression<Func<BaseEntityTeam, bool>>)(team => team.Members.Any(b => b.UserId == userId));

            return Combine(getTeam, isMember);
        }

        /// <summary>
        /// Function IsMemberOfOneOfTeams for specification.
        /// </summary>
        /// <typeparam name="TTeam">the team type.</typeparam>
        /// <param name="getTeams">the list of teams.</param>
        /// <param name="userId">the user Id.</param>
        /// <returns>expresion that return true if user is member of one of the teams.</returns>
        public static Expression<Func<TTeam, bool>> IsMemberOfOneOfTeams<TTeam>(Expression<Func<TTeam, IEnumerable<BaseEntityTeam>>> getTeams, int userId)
             where TTeam : class, IEntity<int>, IEntityTeam
        {
            var isMemberOfOne = (Expression<Func<IEnumerable<BaseEntityTeam>, bool>>)(teams => teams.Any(a => a.Members.Any(b => b.UserId == userId)));

            return Combine(getTeams, isMemberOfOne);
        }

        /// <summary>
        /// Function IsCorrectId for specification.
        /// </summary>
        /// <typeparam name="TTeam">the team type.</typeparam>
        /// <param name="getTeam">the team.</param>
        /// <param name="teamId">the team Id to check.</param>
        /// <returns>expresion that return true if the team id correspond.</returns>
        public static Expression<Func<TTeam, bool>> IsCorrectId<TTeam>(Expression<Func<TTeam, BaseEntityTeam>> getTeam, int teamId)
            where TTeam : class, IEntity<int>, IEntityTeam
        {
            var isCorrectId = (Expression<Func<BaseEntityTeam, bool>>)(team => team.Id == teamId);

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

        /// <inheritdoc />
        public async Task<IEnumerable<BaseDtoVersionedTeam>> GetAllAsync(ImmutableList<BiaTeamConfig<BaseEntityTeam>> teamsConfig, int userId = 0, IEnumerable<string> userPermissions = null)
        {
            userPermissions = userPermissions != null ? userPermissions : this.principal.GetUserPermissions();
            userId = userId > 0 ? userId : this.principal.GetUserId();

            TTeamMapper mapper = this.InitMapper<BaseDtoVersionedTeam, TTeamMapper>();
            if (userPermissions?.Any(x => x == BiaRights.Teams.AccessAll) == true)
            {
                return await this.Repository.GetAllResultAsync(mapper.EntityToDto(userId));
            }
            else
            {
                Specification<BaseEntityTeam> specification = new DirectSpecification<BaseEntityTeam>(team => team.Members.Any(member => member.UserId == userId));

                foreach (var teamConfig in teamsConfig)
                {
                    if (teamConfig.Children != null)
                    {
                        foreach (var child in teamConfig.Children)
                        {
                            specification |= new DirectSpecification<BaseEntityTeam>(team => team.TeamTypeId == teamConfig.TeamTypeId)
                                          && new DirectSpecification<BaseEntityTeam>(IsMemberOfOneOfTeams(child.GetChilds, userId));
                        }
                    }

                    if (teamConfig.Parents != null)
                    {
                        foreach (var parent in teamConfig.Parents)
                        {
                            specification |= new DirectSpecification<BaseEntityTeam>(team => team.TeamTypeId == teamConfig.TeamTypeId)
                                          && new DirectSpecification<BaseEntityTeam>(IsMemberOfTeam(parent.GetParent, userId));
                        }
                    }
                }

                return await this.Repository.GetAllResultAsync(
                    mapper.EntityToDto(userId),
                    specification: specification);
            }
        }

        /// <inheritdoc/>
        public bool IsAuthorizeForTeamType(ClaimsPrincipal principal, TEnumTeamTypeId teamTypeId, int teamId, string roleSuffix, ImmutableList<BiaTeamConfig<BaseEntityTeam>> teamsConfig)
        {
            var config = teamsConfig.Find(tc => tc.TeamTypeId == System.Convert.ToInt32(teamTypeId));
            if (config != null)
            {
                if (!principal.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == config.RightPrefix + roleSuffix))
                {
                    return false;
                }

                var userData = new BiaClaimsPrincipal(principal).GetUserData<BaseUserDataDto>();
                if (userData.GetCurrentTeamId(System.Convert.ToInt32(teamTypeId)) != teamId)
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