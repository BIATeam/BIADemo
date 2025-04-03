// BIADemo only
// <copyright file="BiaRemoteService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Utilities
{
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    public class BiaRemoteService : IBiaRemoteService
    {
        /// <summary>
        /// The bia remote repository.
        /// </summary>
        private readonly IBiaRemoteRepository biaRemoteRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaRemoteService"/> class.
        /// </summary>
        /// <param name="biaRemoteRepository">The bia remote repository.</param>
        public BiaRemoteService(IBiaRemoteRepository biaRemoteRepository)
        {
            this.biaRemoteRepository = biaRemoteRepository;
        }

        /// <inheritdoc cref="IBiaRemoteService.PingAsync"/>
        public async Task<bool> PingAsync()
        {
            return await this.biaRemoteRepository.PingAsync();
        }
    }
}
