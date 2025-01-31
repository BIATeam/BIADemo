namespace BIA.Net.Core.Common.Configuration.WorkerFeature
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Metadata.Ecma335;
    using System.Text;
    using System.Threading.Tasks;

    public class ArchiveConfiguration
    {
        public bool isActive { get; set; }
        public List<ArchiveEntityConfiguration> ArchiveEntityConfigurations { get; set; } = new List<ArchiveEntityConfiguration>();
    }

    public class ArchiveEntityConfiguration
    {
        public string EntityName { get; set; }
        public string TargetDirectoryPath { get; set; }
        public bool EnableDeleteStep { get; set; }

        public bool IsMatchingEntityType(Type entityType) => EntityName.Equals(entityType.Name, StringComparison.InvariantCultureIgnoreCase);
        public bool IsValid => !string.IsNullOrWhiteSpace(EntityName) && !string.IsNullOrWhiteSpace(TargetDirectoryPath);
    }
}
