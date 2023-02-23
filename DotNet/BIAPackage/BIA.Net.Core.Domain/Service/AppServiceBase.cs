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
    public abstract class AppServiceBase<TEntity, TKey>
                where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        protected UserContext userContext = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppServiceBase"/> class.
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
        /// Init the mapper and the user context
        /// </summary>
        /// <typeparam name="TOtherDto"></typeparam>
        /// <typeparam name="TOtherMapper"></typeparam>
        /// <returns></returns>
        protected TOtherMapper InitMapper<TOtherDto, TOtherMapper>()
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherMapper : BaseMapper<TOtherDto, TEntity, TKey>, new()
        {
            TOtherMapper mapper = new TOtherMapper();
            if (this.userContext != null)
            {
                mapper.UserContext = this.userContext;
            }
            return mapper;
        }
    }
}