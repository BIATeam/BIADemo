// <copyright file="MemberAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
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
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The application service used for member.
    /// </summary>
    public class MemberAppService : CrudAppServiceBase<MemberDto, Member, LazyLoadDto, MemberMapper>, IMemberAppService
    {
        /// <summary>
        /// The claims principal.
        /// </summary>
        private readonly BIAClaimsPrincipal principal;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        /// <param name="userContext">The user context.</param>
        public MemberAppService(ITGenericRepository<Member> repository, IPrincipal principal, UserContext userContext)
            : base(repository)
        {
            this.principal = principal as BIAClaimsPrincipal;
            this.userContext = userContext;

            // Include already add with the mapper MemberMapper
            // this.Repository.QueryCustomizer = queryCustomizer
        }

        /// <inheritdoc cref="IMemberAppService.GetRangeBySiteAsync"/>
        public async Task<(IEnumerable<MemberDto> Members, int Total)> GetRangeBySiteAsync(LazyLoadDto filters)
        {
            return await this.GetRangeAsync(filters: filters, specification: MemberSpecification.SearchGetAll(filters));
        }

        /// <inheritdoc cref="IMemberAppService.SetDefaultSite"/>
        public async Task SetDefaultSiteAsync(int siteId)
        {
            int userId = this.principal.GetUserId();
            if (userId > 0 && siteId > 0)
            {
                IList<Member> members = (await this.Repository.GetAllEntityAsync(filter: x => x.UserId == userId)).ToList();

                if (members?.Any() == true)
                {
                    foreach (Member member in members)
                    {
                        member.IsDefault = member.SiteId == siteId;
                        this.Repository.Update(member);
                    }

                    await this.Repository.UnitOfWork.CommitAsync();
                }
            }
        }

        /// <inheritdoc cref="IMemberAppService.SetDefaultRoleAsync(int)"/>
        public async Task SetDefaultRoleAsync(int roleId)
        {
            int userId = this.principal.GetUserId();
            if (userId > 0 && roleId > 0)
            {
                IList<Member> members = (await this.Repository.GetAllEntityAsync(filter: x => x.UserId == userId, includes: new Expression<Func<Member, object>>[] { member => member.MemberRoles })).ToList();

                if (members?.Any() == true)
                {
                    foreach (Member member in members)
                    {
                        foreach (MemberRole memberRole in member.MemberRoles)
                        {
                            memberRole.IsDefault = memberRole.RoleId == roleId;
                        }

                        this.Repository.Update(member);
                    }

                    await this.Repository.UnitOfWork.CommitAsync();
                }
            }
        }

        /// <inheritdoc cref="IMemberAppService.ExportCSV(LazyLoadDto)"/>
        public async Task<byte[]> ExportCSV(LazyLoadDto filters)
        {
            // We ignore paging to return all records
            filters.First = 0;
            filters.Rows = 0;

            var query = await this.GetRangeAsync(filters: filters, specification: MemberSpecification.SearchGetAll(filters));

            List<object[]> records = query.Results.Select(member => new object[]
            {
                member.User.Display,
                string.Join("; ", member.Roles.Select(r => r.Display)),
            }).ToList();

            List<string> columnHeaders = null;
            if (filters.Columns != null && filters.Columns.Count > 0)
            {
                columnHeaders = filters.Columns.Select(x => x.Value).ToList();
            }

            StringBuilder csv = new StringBuilder();
            records.ForEach(line =>
                    {
                        csv.AppendLine(string.Join(BIAConstants.Csv.Separator, line));
                    });

            string csvSep = $"sep={BIAConstants.Csv.Separator}\n";
            var buffer = Encoding.GetEncoding("iso-8859-1").GetBytes($"{csvSep}{string.Join(BIAConstants.Csv.Separator, columnHeaders ?? new List<string>())}\r\n{csv}");
            return buffer;
        }
    }
}