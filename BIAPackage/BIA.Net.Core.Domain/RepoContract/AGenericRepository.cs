// <copyright file="AGenericRepository.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.QueryOrder;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using BIA.Net.Core.Domain.Specification;

    public abstract class AGenericRepository<TEntity, TKey> : ITGenericRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected AGenericRepository(ITGenericRepository<TEntity, TKey> repository)
        {
            this.Repository = repository;
        }

        public IQueryCustomizer<TEntity> QueryCustomizer
        {
            get => this.Repository.QueryCustomizer;

            set
            {
                this.Repository.QueryCustomizer = value;
            }
        }

        public IUnitOfWork UnitOfWork => this.Repository.UnitOfWork;

        protected ITGenericRepository<TEntity, TKey> Repository { get; private set; }

        public virtual void Add(TEntity item)
        {
            this.Repository.Add(item);
        }

        public virtual void AddRange(IEnumerable<TEntity> items)
        {
            this.Repository.AddRange(items);
        }

        public virtual Task<bool> AnyAsync(Specification<TEntity> specification)
        {
            return this.Repository.AnyAsync(specification);
        }

        public virtual Task<IEnumerable<TEntity>> GetAllEntityAsync(TKey id = default, Specification<TEntity> specification = null, Expression<Func<TEntity, bool>> filter = null, QueryOrder<TEntity> queryOrder = null, int firstElement = 0, int pageCount = 0, Expression<Func<TEntity, object>>[] includes = null, string queryMode = null)
        {
            return this.Repository.GetAllEntityAsync(id, specification, filter, queryOrder, firstElement, pageCount, includes, queryMode);
        }

        public virtual Task<IEnumerable<TEntity>> GetAllEntityAsync<TOrderKey>(Expression<Func<TEntity, TOrderKey>> orderByExpression, bool ascending, TKey id = default, Specification<TEntity> specification = null, Expression<Func<TEntity, bool>> filter = null, int firstElement = 0, int pageCount = 0, Expression<Func<TEntity, object>>[] includes = null, string queryMode = null)
        {
            return this.Repository.GetAllEntityAsync(orderByExpression, ascending, id, specification, filter, firstElement, pageCount, includes, queryMode);
        }

        public virtual Task<IEnumerable<TResult>> GetAllResultAsync<TOrderKey, TResult>(Expression<Func<TEntity, TResult>> selectResult, Expression<Func<TEntity, TOrderKey>> orderByExpression, bool ascending, TKey id = default, Specification<TEntity> specification = null, Expression<Func<TEntity, bool>> filter = null, int firstElement = 0, int pageCount = 0, Expression<Func<TEntity, object>>[] includes = null, string queryMode = null)
        {
            return this.Repository.GetAllResultAsync(selectResult, orderByExpression, ascending, id, specification, filter, firstElement, pageCount, includes, queryMode);
        }

        public virtual Task<IEnumerable<TResult>> GetAllResultAsync<TResult>(Expression<Func<TEntity, TResult>> selectResult, TKey id = default, Specification<TEntity> specification = null, Expression<Func<TEntity, bool>> filter = null, QueryOrder<TEntity> queryOrder = null, int firstElement = 0, int pageCount = 0, Expression<Func<TEntity, object>>[] includes = null, string queryMode = null)
        {
            return this.Repository.GetAllResultAsync(selectResult, id, specification, filter, queryOrder, firstElement, pageCount, includes, queryMode);
        }

        public virtual Task<TEntity> GetEntityAsync(TKey id = default, Specification<TEntity> specification = null, Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, object>>[] includes = null, string queryMode = null)
        {
            return this.Repository.GetEntityAsync(id, specification, filter, includes, queryMode);
        }

        public virtual Task<Tuple<IEnumerable<TResult>, int>> GetRangeResultAsync<TResult>(Expression<Func<TEntity, TResult>> selectResult, TKey id = default, Specification<TEntity> specification = null, Expression<Func<TEntity, bool>> filter = null, QueryOrder<TEntity> queryOrder = null, int firstElement = 0, int pageCount = 0, Expression<Func<TEntity, object>>[] includes = null, string queryMode = null)
        {
            return this.Repository.GetRangeResultAsync(selectResult, id, specification, filter, queryOrder, firstElement, pageCount, includes, queryMode);
        }

        public virtual Task<TResult> GetResultAsync<TResult>(Expression<Func<TEntity, TResult>> selectResult, TKey id = default, Specification<TEntity> specification = null, Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, object>>[] includes = null, string queryMode = null)
        {
            return this.Repository.GetResultAsync(selectResult, id, specification, filter, includes, queryMode);
        }

        public virtual void Remove(TEntity item)
        {
            this.Repository.Remove(item);
        }

        public virtual void SetModified(TEntity item)
        {
            this.Repository.SetModified(item);
        }
    }
}
