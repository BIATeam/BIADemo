// <copyright file="IFileRepository.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface IFileRepository.
    /// </summary>
    public interface IFileRepository
    {
        /// <summary>
        /// Get the image from a path.
        /// </summary>
        /// <param name="imagePath">The path to the image.</param>
        /// <returns>The bytes of the image.</returns>
        public Task<byte[]> GetImageBytesAsync(string imagePath);
    }
}
