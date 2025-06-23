// <copyright file="UserOptionMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Mappers
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.User;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// The mapper used for user option.
    /// </summary>
    /// <typeparam name="TUser">The type of user.</typeparam>
    public class UserOptionMapper<TUser> : BaseMapper<OptionDto, TUser, int>
        where TUser : BaseEntityUser, new()
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<TUser, OptionDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new OptionDto
            {
                Display = entity.Display(),
            });
        }
    }
}