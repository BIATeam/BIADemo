// <copyright file="FileRepository.cs" company="BIA.Net">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// File Repository.
    /// </summary>
    /// <seealso cref="IFileRepository" />
    public class FileRepository : IFileRepository
    {
        /// <summary>
        /// Gets the logger.
        /// </summary>
        private readonly ILogger<FileRepository> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileRepository"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public FileRepository(ILogger<FileRepository> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<byte[]> GetImageBytesAsync(string imagePath)
        {
            try
            {
                return await Task.Run(() => File.ReadAllBytes(imagePath));
            }
            catch (FileNotFoundException ex)
            {
                this.logger.LogError(ex, "File not found: {ImagePath}", imagePath);
                return null;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error loading image from file {ImagePath}: {Message}", imagePath, ex.Message);
                return null;
            }
        }
    }
}
