// <copyright file="IAuthAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Bia.User
{
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.Entity.Interface;
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Domain.Dto.Bia.User;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.Bia.User.Entities;

    /// <summary>
    /// Interface AuthService.
    /// </summary>
    public interface IAuthAppService<TUserDto, TUser>
        where TUserDto : UserDto, new()
        where TUser : User, IEntity<int>, new()
    {
#if BIA_BACK_TO_BACK_AUTH
        /// <summary>
        /// Logins.
        /// </summary>
        /// <returns>The JWT.</returns>
        Task<string> LoginAsync();
#endif
#if BIA_FRONT_FEATURE

        /// <summary>
        /// Logins the on teams asynchronous.
        /// </summary>
        /// <param name="loginParam">The login parameter.</param>
        /// <returns>AuthInfo.</returns>
        Task<AuthInfoDto<AdditionalInfoDto>> LoginOnTeamsAsync(LoginParamDto loginParam);
#endif
    }
}