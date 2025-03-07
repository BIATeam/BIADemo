// <copyright file="FixableCrudAppServiceBase.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;

    /// <summary>
    /// The base class for all fixable CRUD application service.
    /// </summary>
    /// <typeparam name="TDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TFilterDto">The filter DTO type.</typeparam>
    /// <typeparam name="TMapper">The mapper used between entity and DTO.</typeparam>
    public abstract class FixableCrudAppServiceBase<TDto, TEntity, TKey, TFilterDto, TMapper> : CrudAppServiceBase<TDto, TEntity, TKey, TFilterDto, TMapper>, IFixableCrudAppServiceBase<TDto, TEntity, TKey, TFilterDto>
        where TDto : BaseDto<TKey>, new()
        where TEntity : class, IEntityFixable<TKey>, new()
        where TFilterDto : LazyLoadDto, new()
        where TMapper : BaseMapper<TDto, TEntity, TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FixableCrudAppServiceBase{TDto, TEntity, TKey, TFilterDto, TMapper}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        protected FixableCrudAppServiceBase(ITGenericRepository<TEntity, TKey> repository)
            : base(repository)
        {
        }

        /// <inheritdoc/>
        public virtual async Task<TDto> UpdateFixedAsync(TKey id, bool isFixed)
        {
            await this.ExecuteWithFrontUserExceptionHandlingAsync(async () =>
            {
                await this.Repository.UpdateFixedAsync(id, isFixed);
                return Task.CompletedTask;
            });

            return await this.GetAsync(id);
        }
    }
}