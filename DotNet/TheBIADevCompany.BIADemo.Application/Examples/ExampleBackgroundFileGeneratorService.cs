// <copyright file="ExampleBackgroundFileGeneratorService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Examples
{
    using System.IO;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.File;

    /// <summary>
    /// Example implementation of the <see cref="IBiaBackgroundFileGeneratorService"/> interface that generates a file for download in a background job.
    /// This example demonstrates how to implement the file generation logic and return the necessary data for the file download.
    /// You can customize the file generation logic to create files based on your specific requirements, such as generating reports, exporting data, or creating any other type of file that needs to be downloaded by the user.
    /// </summary>
    public class ExampleBackgroundFileGeneratorService : IExampleBackgroundFileGeneratorService
    {
        /// <inheritdoc/>
        public Task<FileDownloadDataDto> GenerateAsync()
        {
            var currentAssemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;

            // Light example file
            var downloadFileExamplePath = Path.Combine(Path.GetDirectoryName(currentAssemblyLocation), "Resources", "DownloadFileExample.txt");
            return Task.FromResult(new FileDownloadDataDto() { FilePath = downloadFileExamplePath, FileContentType = "text/plain; charset=utf-8", FileName = "FileExample.txt" });

            // Huge example file (you must create it by yourself for testing)
            var downloadHugeFileExamplePath = Path.Combine(Path.GetDirectoryName(currentAssemblyLocation), "Resources", "DownloadHugeFileExample.zip");
            return Task.FromResult(new FileDownloadDataDto() { FilePath = downloadHugeFileExamplePath, FileContentType = "application/zip", FileName = "HugeFileExample.zip" });
        }
    }
}
