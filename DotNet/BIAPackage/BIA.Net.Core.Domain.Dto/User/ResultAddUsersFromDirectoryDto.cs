// <copyright file="ResultAddUsersFromAddDto.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>


namespace BIA.Net.Core.Domain.Dto.User
{
    using BIA.Net.Core.Domain.Dto.Option;
    using System.Collections.Generic;

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
