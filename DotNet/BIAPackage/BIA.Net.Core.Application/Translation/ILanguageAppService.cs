// <copyright file="ILanguageAppService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Translation
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The interface defining the application service for language.
    /// </summary>
    public interface ILanguageAppService : IOptionAppServiceBase<OptionDto, int>
    {
    }
}