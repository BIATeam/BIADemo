namespace BIA.Net.Core.Common.Configuration.WorkerFeature
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ArchiveConfiguration
    {
        public bool IsActive { get; set; }
        public List<string> EntitiesToArchive { get; set; } = new List<string>();
        public string ArchivePath { get; set; }
    }
}
