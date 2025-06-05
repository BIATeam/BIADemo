// <copyright file="ITeamMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Mapper
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// Interface for Team Mapper.
    /// </summary>
    public interface ITeamMapper
    {
        /// <summary>
        /// Entities to dto.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>the Dto.</returns>
        Expression<Func<Team, BaseDtoVersionedTeam>> EntityToDto(int userId);
    }
}
