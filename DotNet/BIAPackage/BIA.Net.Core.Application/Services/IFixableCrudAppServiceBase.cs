// <copyright file="IFixableCrudAppServiceBase.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The interface defining the fixable CRUD methods.
    /// </summary>
    /// <typeparam name="TDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The primary key type.</typeparam>
    /// <typeparam name="TFilterDto">The filter DTO type.</typeparam>
    public interface IFixableCrudAppServiceBase<TDto, TEntity, TKey, TFilterDto> : ICrudAppServiceBase<TDto, TEntity, TKey, TFilterDto>
        where TDto : BaseDto<TKey>, new()
        where TEntity : class, IEntityFixable<TKey>, new()
        where TFilterDto : LazyLoadDto, new()
    {
        /// <summary>
        /// Update the fixed status of an <see cref="IEntityFixable{TKey}"/>.
        /// </summary>
        /// <param name="id">ID of the entity.</param>
        /// <param name="isFixed">Fixed status.</param>
        /// <returns>Updated DTO.</returns>
        Task<TDto> UpdateFixedAsync(TKey id, bool isFixed);
    }
}