namespace BIA.Net.Core.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.File;
    using BIA.Net.Core.Domain.User.Entities;

    public interface IBiaFileDownloaderService
    {
        void PrepareDownload(Func<Task<FileDownloadData>> getFileDownloadDataTask, int requestedByUserId);
    }
}
