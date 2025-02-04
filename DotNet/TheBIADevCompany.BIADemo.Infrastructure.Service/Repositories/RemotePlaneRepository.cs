// BIADemo only
// <copyright file="RemotePlaneRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration.AuthenticationSection;
    using BIA.Net.Core.Common.Helpers;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Infrastructure.Service.Dto;

    /// <summary>
    /// RemotePlane Repository.
    /// </summary>
    public class RemotePlaneRepository : WebApiRepository, IRemotePlaneRepository
    {
        /// <summary>
        /// The base address.
        /// </summary>
        private readonly string baseAddress;

        /// <summary>
        /// The URL plane.
        /// </summary>
        private readonly string urlPlane;

        /// <summary>
        /// The URL login.
        /// </summary>
        private readonly string urlLogin;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemotePlaneRepository"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        public RemotePlaneRepository(HttpClient httpClient, IConfiguration configuration, ILogger<RemotePlaneRepository> logger, IBiaDistributedCache distributedCache)
             : base(httpClient, logger, distributedCache)
        {
#pragma warning disable S125 // Sections of code should not be commented out

            // For a real project, the fields baseAddress, urlPlane, urlLogin must be entered in the appsettings.json file
            // and retrieved via the configuration field.

            // this.baseAddress = configuration["RemoteBIADemoWebApi:baseAddress"];
            this.baseAddress = "http://localhost:32128/BIADemo/WebApi";

            // this.urlPlane = configuration["RemoteBIADemoWebApi:urlPlane"];
            this.urlPlane = "/api/Planes/";

            // this.urlLogin = configuration["RemoteBIADemoWebApi:urlLogin"];
            this.urlLogin = "/api/Auth/login?lightToken=false";

            this.AuthenticationConfiguration.Mode = AuthenticationMode.Token;

#pragma warning restore S125 // Sections of code should not be commented out
        }

        /// <inheritdoc cref="IRemotePlaneRepository.GetAsync"/>
        public async Task<Plane> GetAsync(int id)
        {
            var result = await this.GetAsync<Plane>($"{this.baseAddress}{this.urlPlane}{id}");
            return result.IsSuccessStatusCode ? result.Result : null;
        }

        /// <inheritdoc cref="IRemotePlaneRepository.DeleteAsync"/>
        public async Task<bool> DeleteAsync(int id)
        {
            var result = await this.DeleteAsync<Plane>($"{this.baseAddress}{this.urlPlane}{id}");
            return result.IsSuccessStatusCode;
        }

        /// <inheritdoc cref="IRemotePlaneRepository.PostAsync"/>
        public async Task<Plane> PostAsync(Plane plane)
        {
            RemotePlaneDto dto = new RemotePlaneDto();
            PropertyMapper.Map(plane, dto);
            var result = await this.PostAsync<RemotePlaneDto, RemotePlaneDto>($"{this.baseAddress}{this.urlPlane}", dto, true);

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
            var result = await this.PutAsync<RemotePlaneDto, RemotePlaneDto>($"{this.baseAddress}{this.urlPlane}{dto.Id}", dto, true);

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

        /// <inheritdoc />
        protected override async Task<string> GetBearerTokenAsync()
        {
            var result = await this.SendAsync<AuthInfoDto<AdditionalInfoDto>>($"{this.baseAddress}{this.urlLogin}", HttpMethod.Get, skipAuthent: true);
            return result.Result?.Token;
        }
    }
}
