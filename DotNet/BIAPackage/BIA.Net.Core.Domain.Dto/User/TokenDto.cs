// <copyright file="TokenDto.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Domain.Dto.User
{
    using System.Collections.Generic;

    /// <summary>
    /// TokenDto.
    /// </summary>
    /// <typeparam name="TUserDataDto">The type of the user data dto.</typeparam>
    public class TokenDto<TUserDataDto>
        where TUserDataDto : UserDataDto
    {
        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the user Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the RoleIds.
        /// </summary>
        public List<int> RoleIds { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        public IEnumerable<string> Permissions { get; set; }

        /// <summary>
        /// Gets or sets the user data.
        /// </summary>
        public TUserDataDto UserData { get; set; }
    }
}
