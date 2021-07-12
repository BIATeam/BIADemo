// <copyright file="RequestHelper.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    /// <summary>
    /// The helper used to encapsulate the API calls.
    /// </summary>
    public static class RequestHelper
    {
        /// <summary>
        /// Call an API with GET verb.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="url">The base URL of the API.</param>
        /// <param name="urlParameters">The URl parameters.</param>
        /// <returns>The result of the API call.</returns>
        public static async Task<string> GetAsync(string url, Dictionary<string, string> urlParameters)
        {
            try
            {
                using (var httpClient = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true, UseProxy = false }) { Timeout = TimeSpan.FromMilliseconds(200000) })
                {
                    using (var response = await httpClient.GetAsync(CreateUrl(url, urlParameters)))
                    {

                        string apiResponse = await response.Content.ReadAsStringAsync();
                        return apiResponse;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Can't join url : " + url, ex);
            }
        }
        /// <summary>
        /// Call an API with GET verb.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="url">The base URL of the API.</param>
        /// <param name="urlParameters">The URl parameters.</param>
        /// <returns>The result of the API call.</returns>
        public static async Task<T> GetAsync<T>(string url, Dictionary<string, string> urlParameters)
        {
            using (var httpClient = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true, UseProxy = false }) { Timeout = TimeSpan.FromMilliseconds(200000) })
            {
                using (var response = await httpClient.GetAsync(CreateUrl(url, urlParameters)))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(apiResponse);
                }
            }
        }


        /// <summary>
        /// Call an API with POST verb.
        /// </summary>
        /// <param name="url">The base URL of the API.</param>
        /// <param name="urlParameters">The URl parameters.</param>
        /// <param name="data">The data to post.</param>
        /// <returns>The result of the API call.</returns>
        public static async Task<string> PostAsync(string url, Dictionary<string, string> urlParameters, object data)
        {
            using (var httpClient = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true, UseProxy = false }) { Timeout = TimeSpan.FromMilliseconds(200000) })
            {
                var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync(CreateUrl(url, urlParameters), content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return apiResponse;
                }
            }
        }

        /// <summary>
        /// Call an API with POST verb.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="url">The base URL of the API.</param>
        /// <param name="urlParameters">The URl parameters.</param>
        /// <param name="data">The data to post.</param>
        /// <returns>The result of the API call.</returns>
        public static async Task<T> PostAsync<T>(string url, Dictionary<string, string> urlParameters, object data)
        {
            using (var httpClient = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true, UseProxy = false }) { Timeout = TimeSpan.FromMilliseconds(200000) })
            {
                var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync(CreateUrl(url, urlParameters), content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(apiResponse);
                }
            }
        }

        /// <summary>
        /// Create the final URL adding the parameters.
        /// </summary>
        /// <param name="url">The base URL of the API.</param>
        /// <param name="parameters">The URl parameters.</param>
        /// <returns>The final URL.</returns>
        private static string CreateUrl(string url, Dictionary<string, string> parameters)
        {
            var parameterBuilder = new StringBuilder();

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    parameterBuilder.Append(parameterBuilder.Length == 0 ? "?" : "&");
                    parameterBuilder.Append($"{parameter.Key}={parameter.Value}");
                }
            }

            return $"{url}{parameterBuilder}";
        }
    }
}