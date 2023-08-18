// <copyright file="AppServiceBase.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Service
{
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;

    /// <summary>
    /// The base class for all application service.
    /// </summary>
    /// <typeparam name="TDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TMapper">The mapper used between entity and DTO.</typeparam>
    public abstract class AppServiceBase<TDto, TEntity, TKey, TMapper>
        where TDto : BaseDto<TKey>, new()
        where TEntity : class, IEntity<TKey>, new()
        where TMapper : BaseMapper<TDto, TEntity, TKey>, new()
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        protected UserContext userContext = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppServiceBase{TDto, TEntity, TKey, TMapper}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        protected AppServiceBase(ITGenericRepository<TEntity, TKey> repository)
        {
            this.Repository = repository;
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        protected ITGenericRepository<TEntity, TKey> Repository { get; }

        /// <summary>
        /// Init the mapper and the user context.
        /// </summary>
        /// <typeparam name="TOtherDto">Dto type.</typeparam>
        /// <typeparam name="TOtherMapper">Dto to entity mapper type.</typeparam>
        /// <returns>The mapper.</returns>
        protected virtual TOtherMapper InitMapper<TOtherDto, TOtherMapper>()
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherMapper : BaseMapper<TOtherDto, TEntity, TKey>, new()
        {
            TOtherMapper mapper = new();
            if (this.userContext != null)
            {
                mapper.UserContext = this.userContext;
            }

            return mapper;
        }

        /// <summary>
        /// Init the mapper and the user context.
        /// </summary>
        /// <returns>The mapper.</returns>
        protected virtual TMapper InitMapper()
        {
            return this.InitMapper<TDto, TMapper>();
        }
    }
}