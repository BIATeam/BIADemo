// <copyright file="IOptionAppServiceBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// IOptionAppServiceBase.
    /// </summary>
    /// <typeparam name="TOptionDto">The option DTO type.</typeparam>
    /// <typeparam name="TKey">Key type of the option DTO type.</typeparam>
    public interface IOptionAppServiceBase<TOptionDto, TKey>
        where TOptionDto : TOptionDto<TKey>, new()
    {
        /// <summary>
        /// Return all the options.
        /// </summary>
        /// <returns>List of <see cref="TOptionDto"/>.</returns>
        Task<IEnumerable<TOptionDto>> GetAllOptionsAsync();
    }
}
