// <copyright file="WebApiRepository.cs" company="BIA">
//  Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Mime;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration.AuthenticationSection;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Net.Http.Headers;
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
        /// Bearer header name.
        /// </summary>
        protected const string Bearer = "Bearer";

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiRepository" /> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        /// <param name="configurationSection">The authentication configuration for the API requests.</param>
        protected WebApiRepository(
            HttpClient httpClient,
            ILogger logger,
            IBiaDistributedCache distributedCache,
            AuthenticationConfiguration configurationSection = null)
        {
            this.HttpClient = httpClient;
            this.Logger = logger;
            this.DistributedCache = distributedCache;
            this.AuthenticationConfiguration = configurationSection ?? new AuthenticationConfiguration { Mode = AuthenticationMode.Anonymous };

            this.ClassName = this.GetType().Name;
        }

        /// <summary>
        /// Is httpClient currently being configured.
        /// </summary>
        protected bool OngoingConfiguration { get; set; }

        /// <summary>
        /// Is httpClient configured.
        /// </summary>
        protected bool ConfiguredHttpClient { get; set; }

        /// <summary>
        /// Gets or sets the compression level.
        /// </summary>
        protected CompressionLevel CompressionLevel { get; set; } = CompressionLevel.NoCompression;

        /// <summary>
        /// The authentication configuration for the API requests.
        /// </summary>
        protected AuthenticationConfiguration AuthenticationConfiguration { get; set; }

        /// <summary>
        /// The distributed cache.
        /// </summary>
        protected IBiaDistributedCache DistributedCache { get; init; }

        /// <summary>
        /// Gets the HTTP client.
        /// </summary>
        protected HttpClient HttpClient { get; init; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected ILogger Logger { get; init; }

        /// <summary>
        /// The child class name.
        /// </summary>
        protected string ClassName { get; init; }

        /// <summary>
        /// Get the authentication configuration from the configuration section.
        /// </summary>
        /// <param name="configurationSection">The authentication configuration from the configuration for the API requests.</param>
        /// <returns>The authentication configuration for the API requests.</returns>
        public static AuthenticationConfiguration GetAuthenticationConfiguration(IConfigurationSection configurationSection)
        {
            if (configurationSection != null)
            {
                AuthenticationConfiguration authenticationConfiguration = new AuthenticationConfiguration();
                configurationSection.Bind(authenticationConfiguration);
                return authenticationConfiguration;
            }
            else
            {
                return new AuthenticationConfiguration { Mode = AuthenticationMode.Anonymous };
            }
        }

        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="cacheDurationInMinute">The cache duration in minute.</param>
        /// <returns>
        /// Result, IsSuccessStatusCode, ReasonPhrase.
        /// </returns>
        protected virtual async Task<(T Result, bool IsSuccessStatusCode, string ReasonPhrase)> GetAsync<T>(string url, double cacheDurationInMinute = default)
        {
            return await this.SendAsync<T>(url: url, httpMethod: HttpMethod.Get, retry: false, cacheDurationInMinute: cacheDurationInMinute);
        }

        /// <summary>
        /// Retrieves a list of objects by sending a POST request with a list of keys, utilizing cache when possible.
        /// Cached objects are returned directly, while missing objects are fetched from the API and optionally cached.
        /// </summary>
        /// <typeparam name="TResult">The type of the result objects.</typeparam>
        /// <typeparam name="TKey">The type of the key used to identify objects.</typeparam>
        /// <param name="url">The URL to send the POST request to.</param>
        /// <param name="keys">The list of keys to retrieve objects for.</param>
        /// <param name="keySelector">A function to extract the key from a result object.</param>
        /// <param name="cacheDurationInMinute">The duration in minutes to cache fetched objects. If 0, caching is disabled.</param>
        /// <param name="isFormUrlEncoded">Indicates whether the POST body should be form URL encoded.</param>
        /// <returns>
        /// A tuple containing:
        /// - Result: The list of retrieved objects (from cache and/or API),
        /// - IsSuccessStatusCode: True if the API call was successful or not needed,
        /// - ReasonPhrase: The reason phrase from the API response if applicable.
        /// </returns>
        protected virtual async Task<(List<TResult> Result, bool IsSuccessStatusCode, string ReasonPhrase)> GetByPostAsync<TResult, TKey>(
            string url,
            List<TKey> keys,
            Func<TResult, TKey> keySelector,
            double cacheDurationInMinute = default,
            bool isFormUrlEncoded = false)
        {
            if (keys == null || keys.Count == 0)
            {
                return (new List<TResult>(), true, null);
            }

            // Get cached objects
            List<TResult> cachedObjects = await this.GetCachedObjectsAsync<TResult, TKey>(keys);
            List<TKey> objectNotInCacheKeys = keys.Except(cachedObjects.Select(keySelector)).ToList();

            List<TResult> fetchedObjects = new List<TResult>();
            bool isSuccess = true;
            string reasonPhrase = null;

            if (objectNotInCacheKeys.Any())
            {
                var fetchResult = await this.PostAsync<List<TResult>, List<TKey>>(url, objectNotInCacheKeys, isFormUrlEncoded);
                fetchedObjects = fetchResult.Result ?? new List<TResult>();
                isSuccess = fetchResult.IsSuccessStatusCode;
                reasonPhrase = fetchResult.ReasonPhrase;

                if (cacheDurationInMinute > 0 && isSuccess && fetchedObjects.Any())
                {
                    await this.CacheObjectsAsync(
                        fetchedObjects.ToDictionary(keySelector, x => x),
                        cacheDurationInMinute);
                }
            }

            List<TResult> allObjects = cachedObjects.Concat(fetchedObjects).ToList();

            return (allObjects, isSuccess, reasonPhrase);
        }

        /// <summary>
        /// Send a DELETE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="url">The URL.</param>
        /// <returns>Result, IsSuccessStatusCode, ReasonPhrase.</returns>
        protected virtual async Task<(T Result, bool IsSuccessStatusCode, string ReasonPhrase)> DeleteAsync<T>(string url)
        {
            return await this.SendAsync<T>(url: url, httpMethod: HttpMethod.Delete, retry: false);
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TBody">The type of the body.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="body">The body.</param>
        /// <param name="isFormUrlEncoded">if set to <c>true</c> [is form URL encoded].</param>
        /// <returns>Result, IsSuccessStatusCode, ReasonPhrase.</returns>
        protected virtual async Task<(TResult Result, bool IsSuccessStatusCode, string ReasonPhrase)> PutAsync<TResult, TBody>(string url, TBody body, bool isFormUrlEncoded = false)
        {
            return await this.SendAsync<TResult, TBody>(url: url, body: body, httpMethod: HttpMethod.Put, isFormUrlEncoded: isFormUrlEncoded, retry: false);
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="httpContent">Content of the HTTP.</param>
        /// <param name="isFormUrlEncoded">if set to <c>true</c> [is form URL encoded].</param>
        /// <returns>Result, IsSuccessStatusCode, ReasonPhrase.</returns>
        protected virtual async Task<(TResult Result, bool IsSuccessStatusCode, string ReasonPhrase)> PutAsync<TResult>(string url, HttpContent httpContent, bool isFormUrlEncoded = false)
        {
            return await this.SendAsync<TResult, object>(url: url, httpContent: httpContent, httpMethod: HttpMethod.Put, retry: false);
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TBody">The type of the body.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="body">The body.</param>
        /// <param name="isFormUrlEncoded">if set to <c>true</c> [is form URL encoded].</param>
        /// <returns>Result, IsSuccessStatusCode, ReasonPhrase.</returns>
        protected virtual async Task<(TResult Result, bool IsSuccessStatusCode, string ReasonPhrase)> PostAsync<TResult, TBody>(string url, TBody body, bool isFormUrlEncoded = false)
        {
            return await this.SendAsync<TResult, TBody>(url: url, body: body, httpMethod: HttpMethod.Post, isFormUrlEncoded: isFormUrlEncoded, retry: false);
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="httpContent">Content of the HTTP.</param>
        /// <param name="isFormUrlEncoded">if set to <c>true</c> [is form URL encoded].</param>
        /// <returns>Result, IsSuccessStatusCode, ReasonPhrase.</returns>
        protected virtual async Task<(TResult Result, bool IsSuccessStatusCode, string ReasonPhrase)> PostAsync<TResult>(string url, HttpContent httpContent, bool isFormUrlEncoded = false)
        {
            return await this.SendAsync<TResult, object>(url: url, httpContent: httpContent, httpMethod: HttpMethod.Post, retry: false);
        }

        /// <summary>
        /// Checks the condition retry.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>Return true if the retry condition is Ok.</returns>
        protected virtual bool CheckConditionRetry(HttpResponseMessage response)
        {
            return this.AuthenticationConfiguration.Mode == AuthenticationMode.Token &&
                (response.StatusCode == System.Net.HttpStatusCode.Forbidden ||
                response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                (int)response.StatusCode == 498); // Token expired/invalid
        }

        /// <summary>
        /// Retrieve a token from the provider.
        /// </summary>
        /// <returns>The token.</returns>
        protected virtual Task<string> GetBearerTokenAsync()
        {
            throw new NotImplementedException("For authentication with bearer token, you must implement the GetBearerTokenAsync() method");
        }

        /// <summary>
        /// Add bearer in http request authorization.
        /// </summary>
        /// <returns>A async task.</returns>
        protected virtual async Task AddAuthorizationBearerAsync()
        {
            string bearerToken = await this.GetBearerTokenInCacheAsync();

            if (string.IsNullOrWhiteSpace(bearerToken) || !this.CheckTokenValid(bearerToken))
            {
                bearerToken = await this.GetBearerTokenAsync();
                await this.SetBearerTokenInCacheAsync(bearerToken);
            }

            this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, bearerToken);
        }

        /// <summary>
        /// Get the jwt expiration date.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>The DateTimeOffset.</returns>
        protected virtual DateTimeOffset GetJwtTokenExpirationDate(string token)
        {
            DateTimeOffset expirationDate = default;

            if (!string.IsNullOrWhiteSpace(token))
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtSecurityToken = tokenHandler.ReadJwtToken(token);
                expirationDate = new DateTimeOffset(jwtSecurityToken.ValidTo);
            }

            return expirationDate;
        }

        /// <summary>
        ///  Check if the token is valid.
        /// </summary>
        /// <param name="token">The bearerToken.</param>
        /// <returns>Return true if the token is valid.</returns>
        protected virtual bool CheckTokenValid(string token)
        {
            bool isValid = false;

            if (!string.IsNullOrWhiteSpace(token))
            {
                DateTimeOffset expirationDate = this.GetJwtTokenExpirationDate(token);
                isValid = expirationDate != default && expirationDate > DateTime.UtcNow.AddSeconds(10);
            }

            return isValid;
        }

        /// <summary>
        /// Get the storage key of the token in the cache.
        /// </summary>
        /// <returns>The storage key.</returns>
        protected virtual string GetBearerCacheKey()
        {
            return $"{this.ClassName}|{Bearer}";
        }

        /// <summary>
        /// Get the bearer token in the cache.
        /// </summary>
        /// <returns>The bearer token.</returns>
        protected virtual async Task<string> GetBearerTokenInCacheAsync()
        {
            return await this.DistributedCache.Get<string>(this.GetBearerCacheKey());
        }

        /// <summary>
        /// Set the bearer token in cache.
        /// </summary>
        /// <param name="bearerToken">The bearer token.</param>
        /// <returns>A async task.</returns>
        protected virtual async Task SetBearerTokenInCacheAsync(string bearerToken)
        {
            if (!string.IsNullOrWhiteSpace(bearerToken))
            {
                DateTimeOffset expirationDate = this.GetJwtTokenExpirationDate(bearerToken);
                TimeSpan expirationFromNow = expirationDate - DateTimeOffset.UtcNow.Add(new TimeSpan(0, 0, 10));
                await this.DistributedCache.Add(this.GetBearerCacheKey(), bearerToken, expirationFromNow.TotalMinutes);
            }
            else
            {
                await this.DistributedCache.Remove(this.GetBearerCacheKey());
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns>Result, IsSuccessStatusCode, ReasonPhrase.</returns>
        protected virtual async Task<(T Result, bool IsSuccessStatusCode, string ReasonPhrase)> SendAsync<T>(HttpRequestMessage request)
        {
            return await this.SendAsync<T>(request: request, retry: false);
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="url">The url.</param>
        /// <param name="httpMethod">The httpMethod.</param>
        /// <param name="request">The request.</param>
        /// <param name="retry">if true it is a retry operation.</param>
        /// <param name="cacheDurationInMinute">The cache duration in minute.</param>
        /// <returns>
        /// Result, IsSuccessStatusCode, ReasonPhrase.
        /// </returns>
        protected virtual async Task<(T Result, bool IsSuccessStatusCode, string ReasonPhrase)> SendAsync<T>(
            string url = default,
            HttpMethod httpMethod = default,
            HttpRequestMessage request = default,
            bool retry = false,
            double cacheDurationInMinute = default)
        {
            if (!string.IsNullOrWhiteSpace(url) || request != default)
            {
                if (!string.IsNullOrWhiteSpace(url))
                {
                    string message = $"Call WebApi {httpMethod?.Method}: {url}";
                    this.Logger.LogInformation(message);
                }
                else
                {
                    string message = $"Call WebApi {request?.Method?.Method}: {request?.RequestUri?.AbsoluteUri}";
                    this.Logger.LogInformation(message);
                }

                if (!this.OngoingConfiguration && !this.ConfiguredHttpClient)
                {
                    await this.ConfigureHttpClientAsync();
                }

                HttpResponseMessage response = default;
                string cacheKey = $"{nameof(WebApiRepository)}|{httpMethod?.Method}|{url}";

                // POST, PUT
                if (request != default)
                {
                    if (this.CompressionLevel != CompressionLevel.NoCompression)
                    {
                        await this.CompressRequestContentAsync(request);
                    }

                    response = await this.HttpClient.SendAsync(request);
                }

                // DELETE
                else if (httpMethod?.Method == HttpMethod.Delete.Method)
                {
                    response = await this.HttpClient.DeleteAsync(url);
                }

                // GET
                else
                {
                    if (cacheDurationInMinute > 0)
                    {
                        string cachedResponse = await this.DistributedCache.Get<string>(cacheKey);
                        if (!string.IsNullOrEmpty(cachedResponse))
                        {
                            response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                            {
                                Content = new StringContent(cachedResponse, Encoding.UTF8, MediaTypeNames.Application.Json),
                            };

                            string message = $"Retrieve from cache {httpMethod?.Method}: {url}";
                            this.Logger.LogInformation(message);

                            cacheDurationInMinute = default;
                        }
                        else
                        {
                            response = await this.HttpClient.GetAsync(url);
                        }
                    }
                    else
                    {
                        response = await this.HttpClient.GetAsync(url);
                    }
                }

                if (response.IsSuccessStatusCode)
                {
                    string res = await response.Content.ReadAsStringAsync();
                    string contentType = response.Content.Headers.ContentType?.MediaType;

                    if (contentType == MediaTypeNames.Application.Json)
                    {
                        if (cacheDurationInMinute > 0)
                        {
                            await this.DistributedCache.Add(cacheKey, res, cacheDurationInMinute);
                        }

                        if (typeof(T) == typeof(string))
                        {
                            return ((T)(object)res, response.IsSuccessStatusCode, default(string));
                        }

                        T result = JsonConvert.DeserializeObject<T>(res);
                        return (result, response.IsSuccessStatusCode, default(string));
                    }
                    else
                    {
                        this.Logger.LogWarning("Expected JSON but got '{ContentType}'. Returning default value.", contentType);
                        return (default(T), response.IsSuccessStatusCode, "Response is not JSON.");
                    }
                }
                else
                {
                    if (!this.OngoingConfiguration && !retry && this.CheckConditionRetry(response))
                    {
                        await this.SetBearerTokenInCacheAsync(null);
                        await this.ConfigureHttpClientAsync();
                        return await this.SendAsync<T>(url: url, httpMethod: httpMethod, request: request, retry: true);
                    }

                    string message = $"Url:{url} ReasonPhrase:{response.ReasonPhrase}";
                    this.Logger.LogError(message);
                    return (default(T), response.IsSuccessStatusCode, response.ReasonPhrase);
                }
            }

            return (default(T), default(bool), default(string));
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <typeparam name="TBody">The body type.</typeparam>
        /// <param name="url">The url.</param>
        /// <param name="httpMethod">The httpMethod.</param>
        /// <param name="body">The body.</param>
        /// <param name="httpContent">The request.</param>
        /// <param name="isFormUrlEncoded">specify if form url is encoded.</param>
        /// <param name="retry">if true it is a retry operation.</param>
        /// <returns>Result, IsSuccessStatusCode, ReasonPhrase.</returns>
        protected virtual async Task<(TResult Result, bool IsSuccessStatusCode, string ReasonPhrase)> SendAsync<TResult, TBody>(
            string url,
            HttpMethod httpMethod,
            TBody body = default,
            HttpContent httpContent = default,
            bool isFormUrlEncoded = false,
            bool retry = false)
        {
            if (!string.IsNullOrWhiteSpace(url) && (!object.Equals(body, default(TBody)) || httpContent != default))
            {
                string message = $"Call WebApi {httpMethod.Method}: {url}";
                this.Logger.LogInformation(message);

                if (!this.OngoingConfiguration && !this.ConfiguredHttpClient)
                {
                    await this.ConfigureHttpClientAsync();
                }

                HttpResponseMessage response = default;

                if (!retry && httpContent == default)
                {
                    string json = JsonConvert.SerializeObject(body);
                    if (isFormUrlEncoded)
                    {
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        httpContent = new FormUrlEncodedContent(dictionary);
                    }
                    else
                    {
                        httpContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
                    }
                }

                if (this.CompressionLevel != CompressionLevel.NoCompression)
                {
                    httpContent = await this.CompressRequestContentAsync(httpContent);
                }

                if (httpMethod.Method == HttpMethod.Put.Method)
                {
                    response = await this.HttpClient.PutAsync(url, httpContent);
                }
                else
                {
                    response = await this.HttpClient.PostAsync(url, httpContent);
                }

                if (response.IsSuccessStatusCode)
                {
                    string res = await response.Content.ReadAsStringAsync();
                    TResult result = JsonConvert.DeserializeObject<TResult>(res);
                    httpContent?.Dispose();
                    return (result, response.IsSuccessStatusCode, default(string));
                }
                else
                {
                    if (!this.OngoingConfiguration && !retry && this.CheckConditionRetry(response))
                    {
                        await this.SetBearerTokenInCacheAsync(null);
                        await this.ConfigureHttpClientAsync();
                        return await this.SendAsync<TResult, TBody>(url: url, httpMethod: httpMethod, body: body, httpContent: httpContent, isFormUrlEncoded: isFormUrlEncoded, retry: true);
                    }

                    string errorMessage = $"Url:{url} ReasonPhrase:{response.ReasonPhrase}";
                    this.Logger.LogError(errorMessage);
                    httpContent?.Dispose();
                    return (default(TResult), response.IsSuccessStatusCode, response.ReasonPhrase);
                }
            }

            return (default(TResult), default(bool), default(string));
        }

        /// <summary>
        /// To be called by child constructor.
        /// Add httpClient authorizations (token or API key).
        /// </summary>
        protected virtual void ConfigureHttpClient()
        {
            this.ConfigureHttpClientAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Add httpClient authorizations (token or API key).
        /// </summary>
        /// <returns>Task.</returns>
        protected virtual async Task ConfigureHttpClientAsync()
        {
            this.OngoingConfiguration = true;
            switch (this.AuthenticationConfiguration.Mode)
            {
                case AuthenticationMode.Token:
                    await this.AddAuthorizationBearerAsync();
                    break;
                case AuthenticationMode.ApiKey:
                    this.HttpClient.DefaultRequestHeaders.Add(this.AuthenticationConfiguration.ApiKeyName, this.AuthenticationConfiguration.ApiKey);
                    break;
                case AuthenticationMode.Standard:
                    var credentials = CredentialRepository.RetrieveCredentials(this.AuthenticationConfiguration.CredentialSource);
                    var byteArray = Encoding.ASCII.GetBytes($"{credentials.Login}:{credentials.Password}");
                    this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    break;
                case AuthenticationMode.Default:
                    this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Negotiate");
                    break;
                default:
                    break;
            }

            this.ConfiguredHttpClient = true;
            this.OngoingConfiguration = false;
        }

        /// <summary>
        /// Retrieves a list of objects from the distributed cache based on the provided keys.
        /// Only objects found in the cache are returned; missing objects are ignored.
        /// </summary>
        /// <typeparam name="TResult">The type of the objects to retrieve.</typeparam>
        /// <typeparam name="TKey">The type of the keys used to identify objects.</typeparam>
        /// <param name="keys">The list of keys to look up in the cache.</param>
        /// <returns>A list of objects found in the cache corresponding to the provided keys.</returns>
        /// <exception cref="ArgumentException">Thrown if the keys list is null or empty.</exception>
        protected virtual async Task<List<TResult>> GetCachedObjectsAsync<TResult, TKey>(List<TKey> keys)
        {
            if (keys == null || keys.Count == 0)
            {
                throw new ArgumentException("Keys list cannot be null or empty.", nameof(keys));
            }

            List<string> cacheKeys = keys.Select(key => $"{typeof(TResult).Namespace}|{typeof(TResult).Name}|{key}").ToList();

            List<TResult> returns = new List<TResult>();

            foreach (string cacheKey in cacheKeys)
            {
                TResult obj = await this.DistributedCache.Get<TResult>(cacheKey);
                if (!object.Equals(obj, default(TResult)))
                {
                    returns.Add(obj);
                }
            }

            return returns;
        }

        /// <summary>
        /// Stores a collection of objects in the distributed cache with the specified cache duration.
        /// Each object is cached using a key composed of its type and the provided key.
        /// </summary>
        /// <typeparam name="TResult">The type of the objects to cache.</typeparam>
        /// <typeparam name="TKey">The type of the keys used to identify objects.</typeparam>
        /// <param name="dicts">A dictionary mapping keys to objects to be cached.</param>
        /// <param name="cacheDurationInMinute">The duration in minutes to keep the objects in the cache.</param>
        /// <returns>A task representing the asynchronous cache operation.</returns>
        /// <exception cref="ArgumentException">Thrown if the dictionary is null or empty.</exception>
        protected virtual async Task CacheObjectsAsync<TResult, TKey>(Dictionary<TKey, TResult> dicts, double cacheDurationInMinute)
        {
            if (dicts == null || dicts.Count == 0)
            {
                throw new ArgumentException("Dictionary cannot be null or empty.", nameof(dicts));
            }

            foreach (var dict in dicts)
            {
                await this.DistributedCache.Add($"{typeof(TResult).Namespace}|{typeof(TResult).Name}|{dict.Key}", dict.Value, cacheDurationInMinute);
            }
        }

        /// <summary>
        /// Compresses the content of the given <see cref="HttpRequestMessage"/> using GZip if the request method is POST or PUT
        /// and the content is not already compressed. Updates the request's content with the compressed version.
        /// </summary>
        /// <param name="request">The HTTP request message whose content may be compressed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected virtual async Task CompressRequestContentAsync(HttpRequestMessage request)
        {
            if (request.Content != null &&
                (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put))
            {
                request.Content = await this.CompressContentIfNeededAsync(request.Content);
            }
        }

        /// <summary>
        /// Compresses the provided HttpContent using GZip if not already compressed.
        /// Returns the compressed content, or the original if already compressed or null.
        /// </summary>
        /// <param name="httpContent">The HttpContent to compress.</param>
        /// <returns>The compressed HttpContent, or the original if already compressed or null.</returns>
        protected virtual async Task<HttpContent> CompressRequestContentAsync(HttpContent httpContent)
        {
            if (httpContent == null)
            {
                return null;
            }

            return await this.CompressContentIfNeededAsync(httpContent);
        }

        /// <summary>
        /// Compresses the provided <see cref="HttpContent"/> using GZip if compression is enabled and the content
        /// is not already compressed. Returns the compressed content, or the original if compression is not applied.
        /// </summary>
        /// <param name="content">The HTTP content to potentially compress.</param>
        /// <returns>The compressed <see cref="HttpContent"/>, or the original if compression is not applied.</returns>
        protected virtual async Task<HttpContent> CompressContentIfNeededAsync(HttpContent content)
        {
            if (content == null)
            {
                return null;
            }

            if (this.CompressionLevel != CompressionLevel.NoCompression && !content.Headers.ContentEncoding.Contains("gzip"))
            {
                string originalContent = await content.ReadAsStringAsync();
                string mediaType = content.Headers.ContentType?.MediaType ?? MediaTypeNames.Application.Json;
                ByteArrayContent compressedContent = this.CreateGzipCompressedContent(originalContent, mediaType);

                // Do not copy the Content-Length header; it will be recalculated automatically.
                foreach (var header in content.Headers.Where(header =>
                    !compressedContent.Headers.Contains(header.Key) &&
                    !string.Equals(header.Key, HeaderNames.ContentLength, StringComparison.OrdinalIgnoreCase)))
                {
                    compressedContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }

                // Ensure that Content-Length is not present
                if (compressedContent.Headers.Contains(HeaderNames.ContentLength))
                {
                    compressedContent.Headers.Remove(HeaderNames.ContentLength);
                }

                content.Dispose();
                return compressedContent;
            }

            return content;
        }

        /// <summary>
        /// Creates a GZip-compressed HTTP content from the specified string and media type.
        /// </summary>
        /// <param name="content">The string content to compress.</param>
        /// <param name="mediaType">The media type of the content (e.g., "application/json").</param>
        /// <returns>A <see cref="ByteArrayContent"/> containing the compressed data and appropriate headers.</returns>
        protected virtual ByteArrayContent CreateGzipCompressedContent(string content, string mediaType)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            using (MemoryStream output = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(output, this.CompressionLevel, true))
                {
                    gzip.Write(bytes, 0, bytes.Length);
                }

                byte[] compressed = output.ToArray();

                // Log compression gain
                double ratio = bytes.Length == 0 ? 0 : (1.0 - ((double)compressed.Length / bytes.Length)) * 100.0;
                this.Logger.LogInformation(
                    "GZip compression: CompressionLevel = {CompressionLevel}, original size = {OriginalSize} bytes, compressed size = {CompressedSize} bytes, gain = {Gain:F2}%.",
                    this.CompressionLevel,
                    bytes.Length,
                    compressed.Length,
                    ratio);

                ByteArrayContent byteContent = new ByteArrayContent(compressed);
                byteContent.Headers.ContentEncoding.Add("gzip");
                byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mediaType);

                return byteContent;
            }
        }
    }
}
