namespace BIA.Net.Core.Application.Clean
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain;

    public interface ICleanService
    {
        public Task RunAsync();
    }
}
