// <copyright file="ExampleAppService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Example
{
    using System.IO;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.File;

    /// <summary>
    /// Application service for examples in BIADemo.
    /// </summary>
    public class ExampleAppService : IExampleAppService
    {
        private readonly IBiaFileDownloaderService fileDownloaderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleAppService"/> class.
        /// </summary>
        /// <param name="fileDownloaderService">The file downloader service.</param>
        public ExampleAppService(IBiaFileDownloaderService fileDownloaderService)
        {
            this.fileDownloaderService = fileDownloaderService;
        }

        /// <inheritdoc/>
        public async Task NotifyDownloadReadyFileExample(int requestedByUserId)
        {
            var currentAssemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;

            var downloadFileExamplePath = Path.Combine(Path.GetDirectoryName(currentAssemblyLocation), "Resources", "DownloadFileExample.txt");
            var fileDownloadData = FileDownloadDataDto.Create("FileExample.txt", "text/plain; charset=utf-8", downloadFileExamplePath);

            await this.fileDownloaderService.NotifyDownloadReadyAsync(fileDownloadData, requestedByUserId);
        }
    }
}
