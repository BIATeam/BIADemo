namespace BIA.Net.Core.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.File;
    using BIA.Net.Core.Domain.File.Entities;
    using BIA.Net.Core.Domain.User.Entities;

    public interface IBiaFileDownloaderService
    {
        void PrepareDownload(Func<Task<FileDownloadDataDto>> getFileDownloadDataTask, int requestedByUserId);
        string GenerateDownloadToken(Guid fileGuid, int requestedByUserId);
        FileDownloadData GetFileDownloadData(Guid fileGuid, string token);
    }
}
