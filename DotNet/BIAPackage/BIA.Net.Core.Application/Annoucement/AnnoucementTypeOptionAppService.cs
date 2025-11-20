// <copyright file="AnnoucementTypeOptionAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Annoucement
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Annoucement.Entities;
    using BIA.Net.Core.Domain.Annoucement.Mappers;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.RepoContract;

    /// <summary>
    /// The application service used for annoucement type option.
    /// </summary>
    public class AnnoucementTypeOptionAppService : OptionAppServiceBase<TOptionDto<BiaAnnoucementType>, AnnoucementType, BiaAnnoucementType, AnnoucementOptionMapper>, IAnnoucementOptionAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnnoucementTypeOptionAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public AnnoucementTypeOptionAppService(ITGenericRepository<AnnoucementType, BiaAnnoucementType> repository)
            : base(repository)
        {
        }
    }
}
