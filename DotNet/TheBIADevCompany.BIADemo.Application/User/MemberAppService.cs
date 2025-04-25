// <copyright file="MemberAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.User;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;
    using TheBIADevCompany.BIADemo.Domain.User.Mappers;
    using TheBIADevCompany.BIADemo.Domain.User.Specifications;

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
        /// The <see cref="Team"/> repository.
        /// </summary>
        private readonly ITGenericRepository<Team, int> teamRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        /// <param name="userContext">The user context.</param>
        public MemberAppService(ITGenericRepository<Member, int> repository, IPrincipal principal, ITGenericRepository<Team, int> teamRepository)
            : base(repository)
        {
            this.principal = principal as BiaClaimsPrincipal;
            this.teamRepository = teamRepository;
        }

        /// <inheritdoc cref="IMemberAppService.GetRangeByTeamAsync"/>
        public async Task<(IEnumerable<MemberDto> Members, int Total)> GetRangeByTeamAsync(PagingFilterFormatDto filters)
        {
            return await this.GetRangeAsync(filters: filters, specification: MemberSpecification.SearchGetAll(filters));
        }

        /// <inheritdoc cref="IMemberAppService.AddUsers"/>
        public async Task<IEnumerable<MemberDto>> AddUsers(MembersDto membersDto)
        {
            IEnumerable<MemberDto> dtoActualList = await this.GetAllAsync(specification: new DirectSpecification<Member>(s => s.TeamId == membersDto.TeamId));

            List<MemberDto> dtoList = new List<MemberDto>();
            foreach (var user in membersDto.Users)
            {
                MemberDto existingMember = dtoActualList.FirstOrDefault(m => m.User.Id == user.Id);
                if (existingMember != null)
                {
                    IEnumerable<OptionDto> newRoles = membersDto.Roles.Where(r => !existingMember.Roles.Any(re => re.Id == r.Id)).ToList();
                    if (newRoles.Any())
                    {
                        existingMember.Roles = existingMember.Roles.Union(newRoles);
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

            return await this.SaveAsync(dtoList);
        }

        /// <inheritdoc cref="IMemberAppService.SetDefaultSite"/>
        public async Task SetDefaultTeamAsync(int teamId, int teamTypeId)
        {
            int userId = this.principal.GetUserId();
            if (userId > 0 && teamId > 0)
            {
                IList<Member> members = (await this.Repository.GetAllEntityAsync(filter: x => x.UserId == userId && x.Team.TeamTypeId == teamTypeId)).ToList();
                var teamConfig = TeamConfig.Config.Single(t => t.TeamTypeId == teamTypeId);
                if (!teamConfig.Parents?.IsEmpty == true)
                {
                    var parentTeamIds = teamConfig.Parents.Select(p => p.TeamTypeId).ToList();
                    var parentTeams = await this.teamRepository.GetAllEntityAsync(
                        filter: t => parentTeamIds.Any(pti => pti == t.TeamTypeId),
                        isReadOnlyMode: true,
                        includes: [t => t.Members]);

                    if (parentTeams.SelectMany(t => t.Members).Any(m => m.UserId == userId) && !members.Any(m => m.TeamId == teamId && m.UserId == userId))
                    {
                        this.Repository.Add(new Member { TeamId = teamId, UserId = userId, IsDefault = true });
                    }
                }

                if (members.Any())
                {
                    foreach (Member member in members)
                    {
                        member.IsDefault = member.TeamId == teamId;
                    }
                }

                await this.Repository.UnitOfWork.CommitAsync();
            }
        }

        /// <inheritdoc cref="IMemberAppService.SetDefaultRoleAsync(int)"/>
        public async Task SetDefaultRoleAsync(int teamId, List<int> roleIds)
        {
            int userId = this.principal.GetUserId();
            if (userId > 0)
            {
                IList<Member> members = (await this.Repository.GetAllEntityAsync(filter: x => x.UserId == userId && x.Team.Id == teamId, includes: new Expression<Func<Member, object>>[] { member => member.MemberRoles })).ToList();

                if (members.Any())
                {
                    foreach (Member member in members)
                    {
                        foreach (MemberRole memberRole in member.MemberRoles)
                        {
                            memberRole.IsDefault = roleIds.Contains(memberRole.RoleId);
                        }

                        // this.Repository.Update(member)
                    }

                    await this.Repository.UnitOfWork.CommitAsync();
                }
            }
        }

        /// <inheritdoc cref="IMemberAppService.GetCsvAsync"/>
        public async Task<byte[]> GetCsvAsync(PagingFilterFormatDto filters)
        {
            return await this.GetCsvAsync<MemberDto, MemberMapper, PagingFilterFormatDto>(filters: filters, specification: MemberSpecification.SearchGetAll(filters));
        }
    }
}