namespace BIA.Net.Core.Domain.RepoContract
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Archive;

    public interface ITGenericArchiveRepository<TEntity, TKey> where TEntity : class, IEntityArchivable<TKey>
    {
        Task<IReadOnlyList<TEntity>> GetItemsToArchiveAsync();
        Task<IReadOnlyList<TEntity>> GetItemsToDeleteAsync();
        Task UpdateArchiveStateAsync(TEntity entity, ArchiveStateEnum archiveState);
        Task RemoveAsync(TEntity entity);
    }
}