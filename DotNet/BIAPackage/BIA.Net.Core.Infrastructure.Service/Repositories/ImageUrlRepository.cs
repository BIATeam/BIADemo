// <copyright file="ImageUrlRepository.cs" company="BIA.Net">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using Microsoft.Extensions.Configuration;
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
        /// <param name="configuration">The configuration of the application.</param>
        /// <param name="tokenProvider">The optional token provider for acces to profile image url.</param>
#pragma warning disable S6672 // Generic logger injection should match enclosing type
        public ImageUrlRepository(HttpClient httpClient, ILogger<ImageUrlRepository> logger, IBiaDistributedCache distributedCache, IConfiguration configuration, ITokenProvider tokenProvider = null)
#pragma warning restore S6672 // Generic logger injection should match enclosing type
             : base(httpClient, logger, distributedCache)
        {
            this.Configuration = configuration;
            this.TokenProvider = tokenProvider;
        }

        /// <summary>
        /// The configuration of the application.
        /// </summary>
        protected IConfiguration Configuration { get; }

        /// <summary>
        /// The configuration of the application.
        /// </summary>
        protected ITokenProvider TokenProvider { get; }

        /// <inheritdoc/>
        public async Task<byte[]> GetImageBytesAsync(string imageUrl)
        {
            BiaNetSection biaNetSection = null;

            if (this.Configuration != null)
            {
                biaNetSection = new BiaNetSection();
                this.Configuration.GetSection("BiaNet").Bind(biaNetSection);
            }

            try
            {
                switch (biaNetSection.ProfileConfiguration.AuthentMode)
                {
                    case AuthentMode.Token:
                        await this.AddAuthorizationBearerAsync();
                        break;
                    case AuthentMode.ApiKey:
                        this.HttpClient.DefaultRequestHeaders.Add("X-API-Key", biaNetSection.ProfileConfiguration.ApiKey);
                        break;
                    default:
                        break;
                }

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

        /// <summary>
        /// Retrieve a token from the provider.
        /// </summary>
        /// <returns>The token.</returns>
        protected async override Task<string> GetBearerTokenAsync()
        {
            if (this.TokenProvider != null)
            {
                return await this.TokenProvider.GetTokenAsync();
            }
            else
            {
                throw new NotImplementedException("Define the way you want to get the token by creating a TokenProvider class implementing ITokenProvider and add it to your IocContainer.");
            }
        }
    }
}
