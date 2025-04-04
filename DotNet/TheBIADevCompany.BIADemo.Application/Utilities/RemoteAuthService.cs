// BIADemo only
// <copyright file="RemoteAuthService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Utilities
{
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// Remote Auth Service.
    /// </summary>
    public class RemoteAuthService : IRemoteAuthService
    {
        /// <summary>
        /// The bia remote repository.
        /// </summary>
        private readonly IRemoteAuthRepository biaRemoteRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteAuthService"/> class.
        /// </summary>
        /// <param name="biaRemoteRepository">The bia remote repository.</param>
        public RemoteAuthService(IRemoteAuthRepository biaRemoteRepository)
        {
            this.biaRemoteRepository = biaRemoteRepository;
        }

        /// <inheritdoc cref="IRemoteAuthService.PingAsync"/>
        public async Task<bool> PingAsync()
        {
            return await this.biaRemoteRepository.PingAsync();
        }
    }
}
