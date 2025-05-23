// <copyright file="BaseMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Bia.Base.Mappers
{
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.Mapper;

    /// <summary>
    /// The mapper used for site.
    /// </summary>
    /// <typeparam name="TDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public class BaseMapper<TDto, TEntity, TKey> : BiaBaseMapper<TDto, TEntity, TKey>
        where TDto : BaseDto<TKey>
        where TEntity : class, IEntity<TKey>, new()
    {
    }
}