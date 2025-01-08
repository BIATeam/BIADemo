// <copyright file="IImageUrlRepository.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface IImageUrlRepository.
    /// </summary>
    public interface IImageUrlRepository
    {
        /// <summary>
        /// Get the image from an url.
        /// </summary>
        /// <param name="imageUrl">The url of the image.</param>
        /// <returns>The bytes of the image.</returns>
        public Task<byte[]> GetImageBytesAsync(string imageUrl);
    }
}
