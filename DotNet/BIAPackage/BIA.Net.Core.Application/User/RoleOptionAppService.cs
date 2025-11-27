// <copyright file="RoleOptionAppService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.QueryOrder;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using BIA.Net.Core.Domain.User.Entities;
    using BIA.Net.Core.Domain.User.Mappers;

    /// <summary>
    /// The application service used for role option.
    /// </summary>
    public class RoleOptionAppService : OptionAppServiceBase<OptionDto, Role, int, RoleOptionMapper>, IRoleOptionAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleOptionAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="userContext">The user context.</param>
        public RoleOptionAppService(ITGenericRepository<Role, int> repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Return options.
        /// </summary>
        /// <returns>List of OptionDto.</returns>
        /// <param name="teamTypeId">The team type id.</param>
        public async Task<IEnumerable<OptionDto>> GetAllOptionsAsync(int teamTypeId)
        {
            return await this.GetAllAsync<OptionDto, RoleOptionMapper>(filter: teamTypeId == (int)BiaTeamTypeId.All ? null : r => r.TeamTypes.Any(t => t.Id == teamTypeId));
        }
    }
}