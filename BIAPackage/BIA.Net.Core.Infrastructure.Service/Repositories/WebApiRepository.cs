// <copyright file="WebApiRepository.cs" company="BIA.Net">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// WebApi Repository.
    /// </summary>
    public abstract class WebApiRepository
    {
        /// <summary>
        /// The date format to use for adding date parameter in the url.
        /// </summary>
        protected const string FormatDate = "yyyy-MM-dd'T'HH:mm:ss";

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiRepository"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="logger">The logger.</param>
        protected WebApiRepository(HttpClient httpClient, ILogger<WebApiRepository> logger)
        {
            this.HttpClient = httpClient;
            this.Logger = logger;
        }

        /// <summary>
        /// Gets the HTTP client.
        /// </summary>
        protected HttpClient HttpClient { get; private set; }

        /// <summary>
        /// Gets or sets the base address.
        /// </summary>
        protected string BaseAddress { get; set; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected ILogger<WebApiRepository> Logger { get; private set; }

        /// <summary>
        /// Gets the T asynchronous.
        /// </summary>
        /// <typeparam name="T">Type of result.</typeparam>
        /// <param name="url">The URL.</param>
        /// <returns>Result, IsSuccessStatusCode, ReasonPhrase.</returns>
        protected virtual async Task<(T Result, bool IsSuccessStatusCode, string ReasonPhrase)> GetAsync<T>(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                this.Logger.LogInformation($"Call WebApi Get: {url}");
                HttpResponseMessage response = await this.HttpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string res = await response.Content.ReadAsStringAsync();
                    T content = JsonConvert.DeserializeObject<T>(res);
                    return (content, response.IsSuccessStatusCode, default(string));
                }
                else
                {
                    this.Logger.LogError($"Url:{url} ReasonPhrase:{response.ReasonPhrase}");
                    return (default(T), response.IsSuccessStatusCode, response.ReasonPhrase);
                }
            }

            return (default(T), default(bool), default(string));
        }

        /// <summary>
        /// Post the T asynchronous.
        /// </summary>
        /// <typeparam name="T">Type of result.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="content">Content of the post.</param>
        /// <returns>Result, IsSuccessStatusCode, ReasonPhrase.</returns>
        protected virtual async Task<(T Result, bool IsSuccessStatusCode, string ReasonPhrase)> PostAsync<T, U>(string url, U body)
        {
            if (!string.IsNullOrWhiteSpace(url) && body != null)
            {
                this.Logger.LogInformation($"Call WebApi Post: {url}");

                string json = JsonConvert.SerializeObject(body);

                HttpResponseMessage response = default;
                using (HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    response = await this.HttpClient.PostAsync(url, httpContent);
                }

                if (response.IsSuccessStatusCode)
                {
                    string res = await response.Content.ReadAsStringAsync();
                    T result = JsonConvert.DeserializeObject<T>(res);
                    return (result, response.IsSuccessStatusCode, default(string));
                }
                else
                {
                    this.Logger.LogError($"Url:{url} ReasonPhrase:{response.ReasonPhrase}");
                    return (default(T), response.IsSuccessStatusCode, response.ReasonPhrase);
                }
            }

            return (default(T), default(bool), default(string));
        }
    }
}
