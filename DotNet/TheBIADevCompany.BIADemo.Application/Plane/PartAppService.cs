// BIADemo only
// <copyright file="PartAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Plane
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;

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