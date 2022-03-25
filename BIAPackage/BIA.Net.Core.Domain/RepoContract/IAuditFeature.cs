using System;

namespace BIA.Net.Core.Domain.RepoContract
{
    public interface IAuditFeature
    {
        void UseAuditFeatures(IServiceProvider serviceProvider);
    }
}
