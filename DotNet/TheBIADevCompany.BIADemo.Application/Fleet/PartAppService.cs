// BIADemo only
// <copyright file="PartAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Fleet
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.Dto.Fleet;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Mappers;

    /// <summary>
    /// The application service used for plane.
    /// </summary>
    public class PartAppService : CrudAppServiceBase<PartDto, Part, int, PagingFilterFormatDto, PartMapper>, IPartAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public PartAppService(ITGenericRepository<Part, int> repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Return options.
        /// </summary>
        /// <returns>List of OptionDto.</returns>
        public Task<IEnumerable<OptionDto>> GetAllOptionsAsync()
        {
            return this.GetAllAsync<OptionDto, PartOptionMapper>();
        }
    }
}