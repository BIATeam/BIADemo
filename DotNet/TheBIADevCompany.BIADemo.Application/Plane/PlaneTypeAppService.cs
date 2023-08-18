// BIADemo only
// <copyright file="PlaneTypeAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Plane
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;

    /// <summary>
    /// The application service used for plane.
    /// </summary>
    public class PlaneTypeAppService : CrudAppServiceBase<PlaneTypeDto, PlaneType, int, PagingFilterFormatDto, PlaneTypeMapper>, IPlaneTypeAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneTypeAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public PlaneTypeAppService(ITGenericRepository<PlaneType, int> repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Return options.
        /// </summary>
        /// <returns>List of OptionDto.</returns>
        public async Task<IEnumerable<OptionDto>> GetAllOptionsAsync()
        {
            return await this.Repository.GetAllResultAsync(selectResult: this.InitMapper<OptionDto, PlaneTypeOptionMapper>().EntityToDto());
        }
    }
}