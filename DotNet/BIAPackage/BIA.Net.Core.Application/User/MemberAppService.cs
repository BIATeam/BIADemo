// <copyright file="MemberAppService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Specification;
    using BIA.Net.Core.Domain.User.Entities;
    using BIA.Net.Core.Domain.User.Mappers;
    using BIA.Net.Core.Domain.User.Specifications;

    /// <summary>
    /// The application service used for member.
    /// </summary>
    public class MemberAppService : CrudAppServiceBase<MemberDto, Member, int, PagingFilterFormatDto, MemberMapper>, IMemberAppService
    {
        /// <summary>
        /// The claims principal.
        /// </summary>
        private readonly BiaClaimsPrincipal principal;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        /// <param name="userContext">The user context.</param>
        public MemberAppService(ITGenericRepository<Member, int> repository, IPrincipal principal)
            : base(repository)
        {
            this.principal = principal as BiaClaimsPrincipal;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<MemberDto>> AddUsers(MembersDto membersDto, bool addFromRoleApi = false)
        {
            IEnumerable<MemberDto> dtoActualList = await this.GetAllAsync(specification: new DirectSpecification<Member>(s => s.TeamId == membersDto.TeamId));

            List<MemberDto> dtoList = new List<MemberDto>();
            foreach (var user in membersDto.Users)
            {
                MemberDto existingMember = dtoActualList.FirstOrDefault(m => m.User.Id == user.Id);
                if (existingMember != null)
                {
                    IEnumerable<OptionDto> newRoles = membersDto.Roles.Where(r => !existingMember.Roles.Any(re => re.Id == r.Id)).ToList();
                    IEnumerable<OptionDto> missingRoles = existingMember.Roles.Where(r => !membersDto.Roles.Any(re => re.Id == r.Id)).ToList();
                    if (newRoles.Any() || missingRoles.Any())
                    {
                        if (newRoles.Any())
                        {
                            existingMember.Roles = existingMember.Roles.Union(newRoles);
                            dtoList.Add(existingMember);
                        }

                        if (addFromRoleApi)
                        {
                            foreach (var item in missingRoles)
                            {
                                item.DtoState = DtoState.Deleted;
                            }
                        }

                        existingMember.DtoState = DtoState.Modified;
                        dtoList.Add(existingMember);
                    }
                }
                else
                {
                    MemberDto dto = new MemberDto
                    {
                        User = user,
                        Roles = membersDto.Roles,
                        TeamId = membersDto.TeamId,
                        DtoState = DtoState.Added,
                    };
                    dtoList.Add(dto);
                }
            }

            return await this.SaveAsync(dtoList, mapperMode: addFromRoleApi ? BiaConstants.RoleApi.IsFromRoleApi : null);
        }

        /// <inheritdoc />
        public async Task RemoveRolesAndUserFromTeam(int userId, int teamId, bool removeManualRoles = false)
        {
            Member member = await this.Repository.GetEntityAsync(
                specification: new DirectSpecification<Member>(s => s.TeamId == teamId && s.UserId == userId),
                includes: [member => member.MemberRoles]);

            if (member != null)
            {
                if (!removeManualRoles && member.MemberRoles != null && member.MemberRoles.Any(mr => mr.IsFromRoleApi))
                {
                    var memberRoles = member.MemberRoles.Where(mr => mr.IsFromRoleApi);
                    foreach (var item in memberRoles)
                    {
                        member.MemberRoles.Remove(item);
                    }
                }
                else
                {
                    this.Repository.Remove(member);
                }

                await this.Repository.UnitOfWork.CommitAsync();
            }
        }

        /// <inheritdoc />
        public async Task SetDefaultRoleAsync(int teamId, List<int> roleIds)
        {
            int userId = this.principal.GetUserId();
            if (userId > 0)
            {
                IList<Member> members = (await this.Repository.GetAllEntityAsync(filter: x => x.UserId == userId && x.Team.Id == teamId, includes: new Expression<Func<Member, object>>[] { member => member.MemberRoles })).ToList();

                if (members.Any())
                {
                    foreach (var memberRole in members.SelectMany(x => x.MemberRoles))
                    {
                        memberRole.IsDefault = roleIds.Contains(memberRole.RoleId);
                    }

                    await this.Repository.UnitOfWork.CommitAsync();
                }
            }
        }

        /// <inheritdoc />
        public async Task ResetDefaultRoleAsync(int teamId)
        {
            int userId = this.principal.GetUserId();
            if (userId > 0)
            {
                IList<Member> members = (await this.Repository.GetAllEntityAsync(filter: x => x.UserId == userId && x.Team.Id == teamId, includes: new Expression<Func<Member, object>>[] { member => member.MemberRoles })).ToList();

                if (members.Any())
                {
                    foreach (var memberRole in members.SelectMany(x => x.MemberRoles))
                    {
                        memberRole.IsDefault = false;
                    }

                    await this.Repository.UnitOfWork.CommitAsync();
                }
            }
        }

        /// <inheritdoc/>
        protected override Specification<Member> GetFilterSpecification(PagingFilterFormatDto filters)
        {
            var specification = base.GetFilterSpecification(filters);
            specification &= MemberSpecification.SearchGetAll(filters);
            return specification;
        }
    }
}