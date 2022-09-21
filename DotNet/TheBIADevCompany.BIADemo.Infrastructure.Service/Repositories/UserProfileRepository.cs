// <copyright file="UserProfileRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// WorkInstruction Repository.
    /// </summary>
    /// <seealso cref="TheBIADevCompany.BIADemo.Domain.RepoContract.IWorkInstructionRepository" />
    public class UserProfileRepository : WebApiRepository, IUserProfileRepository
    {
        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly BiaNetSection configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileRepository"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        public UserProfileRepository(HttpClient httpClient, IOptions<BiaNetSection> configuration, ILogger<UserProfileRepository> logger, IDistributedCache distributedCache)
            : base(httpClient, logger, distributedCache)
        {
            this.configuration = configuration.Value;
        }

        /// <inheritdoc cref="IUserProfileRepository.GetAsync"/>
        public virtual async Task<UserProfileDto> GetAsync(string login)
        {
            if (string.IsNullOrWhiteSpace(this.configuration.UserProfile.Url) || string.IsNullOrWhiteSpace(login))
            {
                return null;
            }

            return (await this.GetAsync<UserProfileDto>($"{this.configuration.UserProfile.Url}?login={login}")).Result;
        }
    }
}
