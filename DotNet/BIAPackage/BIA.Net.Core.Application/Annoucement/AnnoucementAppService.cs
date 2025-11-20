// <copyright file="AnnoucementAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Annoucement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Annoucement.Entities;
    using BIA.Net.Core.Domain.Annoucement.Mappers;
    using BIA.Net.Core.Domain.Dto.Annoucement;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;

    /// <summary>
    /// The application service used for annoucement.
    /// </summary>
    public class AnnoucementAppService : CrudAppServiceBase<AnnoucementDto, Annoucement, int, PagingFilterFormatDto, AnnoucementMapper>, IAnnoucementAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnnoucementAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public AnnoucementAppService(
            ITGenericRepository<Annoucement, int> repository,
            IPrincipal principal)
            : base(repository)
        {
        }

        public async Task<List<AnnoucementDto>> GetActives()
        {
            var currentDatetime = DateTime.UtcNow;
            var actives = await this.GetAllAsync(filter: x => x.End > currentDatetime && x.Start <= currentDatetime);
            return [.. actives.OrderBy(x => x.Start)];
        }
    }
}
