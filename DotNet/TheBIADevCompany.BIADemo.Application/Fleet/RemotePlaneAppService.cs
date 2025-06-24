// BIADemo only
// <copyright file="RemotePlaneAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Fleet
{
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;
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

        /// <inheritdoc />
        public async Task<bool> CheckExistAsync(int id)
        {
            var plane = new Plane();
            plane.Id = id;

            plane = await this.remotePlaneRepository.GetAsync(plane.Id);
            return plane?.Id > 0;
        }
    }
}
