// <copyright file="WebApiRepository.cs" company="BIA.Net">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Mime;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Distributed;
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
        /// Bearer header name.
        /// </summary>
        protected const string Bearer = "Bearer";

        /// <summary>
        /// The distributed cache.
        /// </summary>
        protected readonly IDistributedCache distributedCache;

        /// <summary>
        /// Gets the HTTP client.
        /// </summary>
        protected readonly HttpClient httpClient;

        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected readonly ILogger<WebApiRepository> logger;

        /// <summary>
        /// The child class name.
        /// </summary>
        protected readonly string className;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiRepository" /> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        protected WebApiRepository(HttpClient httpClient, ILogger<WebApiRepository> logger, IDistributedCache distributedCache)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.distributedCache = distributedCache;
            this.className = this.GetType().Name;
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="useBearerToken">if set to <c>true</c> [use bearer token].</param>
        /// <returns>Result, IsSuccessStatusCode, ReasonPhrase.</returns>
        protected virtual async Task<(T Result, bool IsSuccessStatusCode, string ReasonPhrase)> SendAsync<T>(HttpRequestMessage request, bool useBearerToken = false)
        {
            return await this.SendAsync<T>(request: request, useBearerToken: useBearerToken, retry: false);
        }

        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="useBearerToken">if set to <c>true</c> [use bearer token].</param>
        /// <returns>Result, IsSuccessStatusCode, ReasonPhrase.</returns>
        protected virtual async Task<(T Result, bool IsSuccessStatusCode, string ReasonPhrase)> GetAsync<T>(string url, bool useBearerToken = false)
        {
            return await this.SendAsync<T>(url: url, httpMethod: HttpMethod.Get, useBearerToken: useBearerToken, retry: false);
        }

        /// <summary>
        /// Send a DELETE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="useBearerToken">if set to <c>true</c> [use bearer token].</param>
        /// <returns>Result, IsSuccessStatusCode, ReasonPhrase.</returns>
        protected virtual async Task<(T Result, bool IsSuccessStatusCode, string ReasonPhrase)> DeleteAsync<T>(string url, bool useBearerToken = false)
        {
            return await this.SendAsync<T>(url: url, httpMethod: HttpMethod.Delete, useBearerToken: useBearerToken, retry: false);
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TBody">The type of the body.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="body">The body.</param>
        /// <param name="useBearerToken">if set to <c>true</c> [use bearer token].</param>
        /// <param name="isFormUrlEncoded">if set to <c>true</c> [is form URL encoded].</param>
        /// <returns>Result, IsSuccessStatusCode, ReasonPhrase.</returns>
        protected virtual async Task<(TResult Result, bool IsSuccessStatusCode, string ReasonPhrase)> PutAsync<TResult, TBody>(string url, TBody body, bool useBearerToken = false, bool isFormUrlEncoded = false)
        {
            return await this.SendAsync<TResult, TBody>(url: url, body: body, httpMethod: HttpMethod.Put, isFormUrlEncoded: isFormUrlEncoded, useBearerToken: useBearerToken, retry: false);
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="httpContent">Content of the HTTP.</param>
        /// <param name="useBearerToken">if set to <c>true</c> [use bearer token].</param>
        /// <param name="isFormUrlEncoded">if set to <c>true</c> [is form URL encoded].</param>
        /// <returns>Result, IsSuccessStatusCode, ReasonPhrase.</returns>
        protected virtual async Task<(TResult Result, bool IsSuccessStatusCode, string ReasonPhrase)> PutAsync<TResult>(string url, HttpContent httpContent, bool useBearerToken = false, bool isFormUrlEncoded = false)
        {
            return await this.SendAsync<TResult, object>(url: url, httpContent: httpContent, httpMethod: HttpMethod.Put, useBearerToken: useBearerToken, retry: false);
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TBody">The type of the body.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="body">The body.</param>
        /// <param name="useBearerToken">if set to <c>true</c> [use bearer token].</param>
        /// <param name="isFormUrlEncoded">if set to <c>true</c> [is form URL encoded].</param>
        /// <returns>Result, IsSuccessStatusCode, ReasonPhrase.</returns>
        protected virtual async Task<(TResult Result, bool IsSuccessStatusCode, string ReasonPhrase)> PostAsync<TResult, TBody>(string url, TBody body, bool useBearerToken = false, bool isFormUrlEncoded = false)
        {
            return await this.SendAsync<TResult, TBody>(url: url, body: body, httpMethod: HttpMethod.Post, isFormUrlEncoded: isFormUrlEncoded, useBearerToken: useBearerToken, retry: false);
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="httpContent">Content of the HTTP.</param>
        /// <param name="useBearerToken">if set to <c>true</c> [use bearer token].</param>
        /// <param name="isFormUrlEncoded">if set to <c>true</c> [is form URL encoded].</param>
        /// <returns>Result, IsSuccessStatusCode, ReasonPhrase.</returns>
        protected virtual async Task<(TResult Result, bool IsSuccessStatusCode, string ReasonPhrase)> PostAsync<TResult>(string url, HttpContent httpContent, bool useBearerToken = false, bool isFormUrlEncoded = false)
        {
            return await this.SendAsync<TResult, object>(url: url, httpContent: httpContent, httpMethod: HttpMethod.Post, useBearerToken: useBearerToken, retry: false);
        }

        /// <summary>
        /// Checks the condition retry.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="useBearerToken">if set to <c>true</c> [use bearer token].</param>
        /// <returns>Return true if the retry condition is Ok.</returns>
        protected virtual bool CheckConditionRetry(HttpResponseMessage response, bool useBearerToken)
        {
            return useBearerToken &&
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
        /// Deserializes if required.
        /// </summary>
        /// <typeparam name="T">Type of return.</typeparam>
        /// <param name="res">The resource.</param>
        /// <returns>object T.</returns>
        private T DeserializeIfRequired<T>(string res)
        {
            T content;
            if (typeof(T) == typeof(string))
            {
                content = (T)Convert.ChangeType(res, typeof(T));
            }
            else
            {
                content = JsonConvert.DeserializeObject<T>(res);
            }

            return content;
        }

        /// <summary>
        /// Add bearer in http request authorization.
        /// </summary>
        /// <returns>A async task.</returns>
        private async Task AddAuthorizationBearerAsync()
        {
            string bearerToken = await this.GetBearerTokenInCacheAsync();

            if (string.IsNullOrWhiteSpace(bearerToken) || !this.CheckTokenValid(bearerToken))
            {
                bearerToken = await this.GetBearerTokenAsync();
                await this.SetBearerTokenInCacheAsync(bearerToken);
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, bearerToken);
        }

        /// <summary>
        /// Get the jwt expiration date.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>The DateTimeOffset.</returns>
        private DateTimeOffset GetJwtTokenExpirationDate(string token)
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
        private bool CheckTokenValid(string token)
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
        private string GetBearerCacheKey()
        {
            return $"{this.className}|{Bearer}";
        }

        /// <summary>
        /// Get the bearer token in the cache.
        /// </summary>
        /// <returns>The bearer token.</returns>
        private async Task<string> GetBearerTokenInCacheAsync()
        {
            return await this.distributedCache.GetStringAsync(this.GetBearerCacheKey());
        }

        /// <summary>
        /// Set the bearer token in cache.
        /// </summary>
        /// <param name="bearerToken">The bearer token.</param>
        /// <returns>A async task.</returns>
        private async Task SetBearerTokenInCacheAsync(string bearerToken)
        {
            if (!string.IsNullOrWhiteSpace(bearerToken))
            {
                DateTimeOffset expirationDate = this.GetJwtTokenExpirationDate(bearerToken);
                DistributedCacheEntryOptions option = new DistributedCacheEntryOptions() { AbsoluteExpiration = expirationDate.AddSeconds(-10) };
                await this.distributedCache.SetStringAsync(this.GetBearerCacheKey(), bearerToken, options: option);
            }
            else
            {
                await this.distributedCache.SetStringAsync(this.GetBearerCacheKey(), string.Empty);
            }
        }

        private async Task<(T Result, bool IsSuccessStatusCode, string ReasonPhrase)> SendAsync<T>(string url = default, HttpMethod httpMethod = default, HttpRequestMessage request = default, bool useBearerToken = false, bool retry = false)
        {
            if (!string.IsNullOrWhiteSpace(url) || request != default)
            {
                if (!string.IsNullOrWhiteSpace(url))
                {
                    this.logger.LogInformation($"Call WebApi {httpMethod?.Method}: {url}");
                }
                else
                {
                    this.logger.LogInformation($"Call WebApi {request?.Method?.Method}: {request?.RequestUri?.AbsoluteUri}");
                }

                if (useBearerToken)
                {
                    await this.AddAuthorizationBearerAsync();
                }

                HttpResponseMessage response = default;

                if (request != default)
                {
                    response = await this.httpClient.SendAsync(request);
                }
                else if (httpMethod?.Method == HttpMethod.Delete.Method)
                {
                    response = await this.httpClient.DeleteAsync(url);
                }
                else
                {
                    response = await this.httpClient.GetAsync(url);
                }

                if (response.IsSuccessStatusCode)
                {
                    string res = await response.Content.ReadAsStringAsync();
                    T result = this.DeserializeIfRequired<T>(res);
                    return (result, response.IsSuccessStatusCode, default(string));
                }
                else
                {
                    if (!retry && this.CheckConditionRetry(response, useBearerToken))
                    {
                        await this.SetBearerTokenInCacheAsync(null);
                        return await this.SendAsync<T>(url: url, httpMethod: httpMethod, request: request, useBearerToken: useBearerToken, retry: true);
                    }

                    this.logger.LogError($"Url:{url} ReasonPhrase:{response.ReasonPhrase}");
                    return (default(T), response.IsSuccessStatusCode, response.ReasonPhrase);
                }
            }

            return (default(T), default(bool), default(string));
        }

        private async Task<(TResult Result, bool IsSuccessStatusCode, string ReasonPhrase)> SendAsync<TResult, TBody>(string url, HttpMethod httpMethod, TBody body = default, HttpContent httpContent = default, bool isFormUrlEncoded = false, bool useBearerToken = false, bool retry = false)
        {
            if (!string.IsNullOrWhiteSpace(url) && (body != null || httpContent != null))
            {
                this.logger.LogInformation($"Call WebApi {httpMethod.Method}: {url}");
                if (useBearerToken)
                {
                    await this.AddAuthorizationBearerAsync();
                }

                HttpResponseMessage response = default;

                if (httpContent == default)
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

                if (httpMethod.Method == HttpMethod.Put.Method)
                {
                    response = await this.httpClient.PutAsync(url, httpContent);
                }
                else
                {
                    response = await this.httpClient.PostAsync(url, httpContent);
                }

                httpContent?.Dispose();

                if (response.IsSuccessStatusCode)
                {
                    string res = await response.Content.ReadAsStringAsync();
                    TResult result = this.DeserializeIfRequired<TResult>(res);
                    return (result, response.IsSuccessStatusCode, default(string));
                }
                else
                {
                    if (!retry && this.CheckConditionRetry(response, useBearerToken))
                    {
                        await this.SetBearerTokenInCacheAsync(null);
                        return await this.SendAsync<TResult, TBody>(url: url, httpMethod: httpMethod, body: body, httpContent: httpContent, isFormUrlEncoded: isFormUrlEncoded, useBearerToken: useBearerToken, retry: true);
                    }

                    this.logger.LogError($"Url:{url} ReasonPhrase:{response.ReasonPhrase}");
                    return (default(TResult), response.IsSuccessStatusCode, response.ReasonPhrase);
                }
            }

            return (default(TResult), default(bool), default(string));
        }
    }
}
