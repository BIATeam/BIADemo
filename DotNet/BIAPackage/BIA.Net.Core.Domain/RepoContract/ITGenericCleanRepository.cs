namespace BIA.Net.Core.Domain.RepoContract
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    public interface ITGenericCleanRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        Task<int> RemoveAll(Expression<Func<TEntity, bool>> rule);
    }
}
