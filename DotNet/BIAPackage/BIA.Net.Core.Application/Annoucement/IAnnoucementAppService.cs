// <copyright file="IAnnoucementAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Annoucement
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Annoucement.Entities;
    using BIA.Net.Core.Domain.Dto.Annoucement;
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The interface defining the application service for annoucement.
    /// </summary>
    public interface IAnnoucementAppService : ICrudAppServiceBase<AnnoucementDto, Annoucement, int, PagingFilterFormatDto>
    {
        Task<List<AnnoucementDto>> GetActives();
    }
}
