// <copyright file="OptionAppServiceBase.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;

    /// <summary>
    /// The base class for all option application service.
    /// </summary>
    /// <typeparam name="TOptionDto">The option DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type of the option DTO and entity.</typeparam>
    /// <typeparam name="TMapper">The mapper between entity and option DTO.</typeparam>
    public abstract class OptionAppServiceBase<TOptionDto, TEntity, TKey, TMapper> : CrudAppServiceBase<TOptionDto, TEntity, TKey, PagingFilterFormatDto, TMapper>, IOptionAppServiceBase<TOptionDto, TKey>
        where TOptionDto : TOptionDto<TKey>, new()
        where TEntity : class, IEntity<TKey>, new()
        where TMapper : BiaBaseMapper<TOptionDto, TEntity, TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionAppServiceBase{TOptionDto, TEntity, TKey, TMapper}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        protected OptionAppServiceBase(ITGenericRepository<TEntity, TKey> repository)
            : base(repository)
        {
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TOptionDto>> GetAllOptionsAsync()
        {
            return await this.GetAllAsync<TOptionDto, TMapper>();
        }
    }
}
