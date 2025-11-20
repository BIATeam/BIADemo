// <copyright file="IAnnoucementTypeOptionAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Annoucement
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The interface defining the application service for annoucement type option.
    /// </summary>
    public interface IAnnoucementOptionAppService : IOptionAppServiceBase<TOptionDto<BiaAnnoucementType>, BiaAnnoucementType>
    {
    }
}
