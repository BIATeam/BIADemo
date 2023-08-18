// <copyright file="ILanguageAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Translation
{
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.TranslationModule.Aggregate;

    /// <summary>
    /// The interface defining the application service for language.
    /// </summary>
    public interface ILanguageAppService : ICrudAppServiceBase<OptionDto, Language, int, LazyLoadDto>
    {
    }
}