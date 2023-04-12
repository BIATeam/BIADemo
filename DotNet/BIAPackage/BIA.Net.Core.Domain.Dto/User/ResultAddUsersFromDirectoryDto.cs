// <copyright file="ResultAddUsersFromDirectoryDto.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Option;

    public class ResultAddUsersFromDirectoryDto
    {
        /// <summary>
        /// The error encoured durring procces.
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// The users added durring procces.
        /// </summary>
        public List<OptionDto> UsersAddedDtos { get; set; }
    }
}
