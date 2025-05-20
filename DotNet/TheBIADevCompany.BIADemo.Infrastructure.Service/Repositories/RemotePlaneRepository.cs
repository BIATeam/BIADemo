// BIADemo only
// <copyright file="RemotePlaneRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration.BiaWebApi;
    using BIA.Net.Core.Common.Helpers;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Infrastructure.Service.Dto;

    /// <summary>
    /// RemotePlane Repository.
    /// </summary>
    public class RemotePlaneRepository : BiaWebApiJwtRepository, IRemotePlaneRepository
    {
        /// <summary>
        /// The URL plane.
        /// </summary>
        private readonly string urlPlane;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemotePlaneRepository"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        /// <param name="biaWebApiAuthRepository">The bia web API authentication repository.</param>
        public RemotePlaneRepository(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<RemotePlaneRepository> logger,
            IBiaDistributedCache distributedCache,
            IBiaWebApiAuthRepository biaWebApiAuthRepository)
             : base(httpClient, logger, distributedCache, biaWebApiAuthRepository, configuration.GetSection("MyBiaWebApi").Get<BiaWebApi>())
        {
            this.urlPlane = "/api/Planes/";
        }

        /// <inheritdoc cref="IRemotePlaneRepository.GetAsync"/>
        public async Task<Plane> GetAsync(int id)
        {
            var result = await this.GetAsync<Plane>($"{this.BaseAddress}{this.urlPlane}{id}");
            return result.IsSuccessStatusCode ? result.Result : null;
        }

        /// <inheritdoc cref="IRemotePlaneRepository.DeleteAsync"/>
        public async Task<bool> DeleteAsync(int id)
        {
            var result = await this.DeleteAsync<Plane>($"{this.BaseAddress}{this.urlPlane}{id}");
            return result.IsSuccessStatusCode;
        }

        /// <inheritdoc cref="IRemotePlaneRepository.PostAsync"/>
        public async Task<Plane> PostAsync(Plane plane)
        {
            RemotePlaneDto dto = new RemotePlaneDto();
            PropertyMapper.Map(plane, dto);
            dto.CurrentAirport = new OptionDto { Id = plane.CurrentAirportId, DtoState = DtoState.Unchanged };
            var result = await this.PostAsync<RemotePlaneDto, RemotePlaneDto>($"{this.BaseAddress}{this.urlPlane}", dto);

            if (result.IsSuccessStatusCode)
            {
                PropertyMapper.Map(result.Result, plane);
                return plane;
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc cref="IRemotePlaneRepository.PutAsync"/>
        public async Task<Plane> PutAsync(Plane plane)
        {
            RemotePlaneDto dto = new RemotePlaneDto();
            PropertyMapper.Map(plane, dto);
            dto.CurrentAirport = new OptionDto { Id = plane.CurrentAirport.Id, Display = string.Empty, DtoState = DtoState.Unchanged };
            var result = await this.PutAsync<RemotePlaneDto, RemotePlaneDto>($"{this.BaseAddress}{this.urlPlane}{dto.Id}", dto);

            if (result.IsSuccessStatusCode)
            {
                PropertyMapper.Map(result.Result, plane);
                return plane;
            }
            else
            {
                return null;
            }
        }
    }
}