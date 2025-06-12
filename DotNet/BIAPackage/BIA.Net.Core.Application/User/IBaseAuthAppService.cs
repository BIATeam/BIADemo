// <copyright file="IBaseAuthAppService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.User
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.User;

    /// <summary>
    /// Interface AuthService.
    /// </summary>
    public interface IBaseAuthAppService
    {
        /// <summary>
        /// Logins.
        /// </summary>
        /// <returns>The JWT.</returns>
        /// <typeparam name="TAdditionalInfoDto">The type of AdditionalInfoDto.</typeparam>
        Task<string> LoginAsync<TAdditionalInfoDto>()
             where TAdditionalInfoDto : BaseAdditionalInfoDto, new();
    }
}