namespace BIA.Net.Core.WorkerService.Features.Archive
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    public interface IEntityArchiveConfiguration
    {
        public Type EntityType { get; }

        public Expression<Func<object, bool>> Step1Predicate();
        public IQueryable<TEntity> SetIncludes<TEntity>(IQueryable<TEntity> entities) where TEntity : class;
    }
}
