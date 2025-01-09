// <copyright file="ImageUrlRepository.cs" company="BIA.Net">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// ImageUrl Repository.
    /// </summary>
    /// <seealso cref="IImageUrlRepository" />
    public class ImageUrlRepository : WebApiRepository, IImageUrlRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageUrlRepository"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="distributedCache">The distributed cache.</param>
#pragma warning disable S6672 // Generic logger injection should match enclosing type
        public ImageUrlRepository(HttpClient httpClient, ILogger<ImageUrlRepository> logger, IBiaDistributedCache distributedCache)
#pragma warning restore S6672 // Generic logger injection should match enclosing type
             : base(httpClient, logger, distributedCache)
        {
        }

        /// <inheritdoc/>
        public async Task<byte[]> GetImageBytesAsync(string imageUrl)
        {
            try
            {
                using (var response = await this.HttpClient.GetAsync(imageUrl))
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsByteArrayAsync();
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, "Error getting image from URL {ImageUrl} : {Message}", imageUrl, ex.Message);
                return null;
            }
        }
    }
}
