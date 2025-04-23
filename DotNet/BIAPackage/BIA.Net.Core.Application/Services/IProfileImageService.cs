// <copyright file="IProfileImageService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for profile image service.
    /// </summary>
    public interface IProfileImageService
    {
        /// <summary>
        /// Get the image from path or url.
        /// </summary>
        /// <param name="pathOrUrl">The path or URL to access the image.</param>
        /// <returns>Bytes for the image.</returns>
        Task<byte[]> GetAsync(string pathOrUrl);
    }
}