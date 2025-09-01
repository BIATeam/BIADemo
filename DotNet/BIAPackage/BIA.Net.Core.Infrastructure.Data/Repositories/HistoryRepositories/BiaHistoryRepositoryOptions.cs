namespace BIA.Net.Core.Infrastructure.Data.Repositories.HistoryRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BiaHistoryRepositoryOptions
    {
        public string AppVersion { get; set; }
        required public bool StampAppVersion { get; set; }
        required public bool StampMigratedAt { get; set; }
    }
}
