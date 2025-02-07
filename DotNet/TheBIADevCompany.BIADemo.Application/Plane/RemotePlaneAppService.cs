// BIADemo only
// <copyright file="RemotePlaneAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Plane
{
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// RemotePlanApp Service.
    /// </summary>
    public class RemotePlaneAppService : IRemotePlaneAppService
    {
        /// <summary>
        /// The remote plane repository.
        /// </summary>
        private readonly IRemotePlaneRepository remotePlaneRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemotePlaneAppService"/> class.
        /// </summary>
        /// <param name="remotePlaneRepository">The remote plane repository.</param>
        public RemotePlaneAppService(IRemotePlaneRepository remotePlaneRepository)
        {
            this.remotePlaneRepository = remotePlaneRepository;
        }

        /// <inheritdoc cref="IRemotePlaneAppService.ExampleCallApiAsync"/>
        public async Task ExampleCallApiAsync()
        {
            var plane = new Plane();

            plane.Capacity = 10;
            plane.SiteId = 1;
            plane.Msn = "MSN1";
            plane.IsActive = true;
            plane.SyncFlightDataTime = new TimeSpan(1, 30, 34);
            plane.CurrentAirportId = 1;
            plane.CurrentAirport = new Airport { Id = 1 };

            plane = await this.remotePlaneRepository.PostAsync(plane);

            if (plane?.Id > 0)
            {
                plane = await this.remotePlaneRepository.GetAsync(plane.Id);

                if (plane?.Id > 0)
                {
                    plane.Capacity += 1;
                    plane = await this.remotePlaneRepository.PutAsync(plane);
                    await this.remotePlaneRepository.DeleteAsync(plane.Id);
                }
            }
        }
    }
}
