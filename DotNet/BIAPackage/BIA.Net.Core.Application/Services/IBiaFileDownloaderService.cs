namespace BIA.Net.Core.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IBiaFileDownloaderService
    {
        void Start(Func<Task<string>> asyncAction);
    }
}
