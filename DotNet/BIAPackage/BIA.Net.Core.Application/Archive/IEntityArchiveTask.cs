namespace BIA.Net.Core.Application.Archive
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    public interface IEntityArchiveTask
    {
        public Task Run();
    }
}
