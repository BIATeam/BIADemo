// BIADemo only
// <copyright file="RemoteBiaApiRwService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Utilities
{
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// Remote Auth Service.
    /// </summary>
    public class RemoteBiaApiRwService : IRemoteBiaApiRwService
    {
        /// <summary>
        /// The bia remote repository.
        /// </summary>
        private readonly IRemoteBiaApiRwRepository remoteBiaApiRwRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteBiaApiRwService"/> class.
        /// </summary>
        /// <param name="remoteBiaApiRwRepository">The bia remote repository.</param>
        public RemoteBiaApiRwService(IRemoteBiaApiRwRepository remoteBiaApiRwRepository)
        {
            this.remoteBiaApiRwRepository = remoteBiaApiRwRepository;
        }

        /// <inheritdoc cref="IRemoteBiaApiRwService.PingAsync"/>
        public async Task<bool> PingAsync()
        {
            return await this.remoteBiaApiRwRepository.PingAsync();
        }
    }
}
