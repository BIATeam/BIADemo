// <copyright file="UserOptionMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Mappers
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.User.Entities;
    using TheBIADevCompany.BIADemo.Domain.Bia.User;

    /// <summary>
    /// The mapper used for user option.
    /// </summary>
    public class UserOptionMapper<TUser> : BaseMapper<OptionDto, TUser, int>
        where TUser : User, new()
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