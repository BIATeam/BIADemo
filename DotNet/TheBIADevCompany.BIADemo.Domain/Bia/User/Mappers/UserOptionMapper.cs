// <copyright file="UserOptionMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Bia.User.Mappers
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Domain.Bia.Base.Mappers;
    using TheBIADevCompany.BIADemo.Domain.Bia.User;
    using TheBIADevCompany.BIADemo.Domain.Bia.User.Entities;

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