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
            Plane plane = await this.remotePlaneRepository.GetAsync(id);
            return plane?.Id > 0;
        }

        /// <inheritdoc />
        public async Task<bool> CreateAsync()
        {
            Plane newPlane = GenerateFromValues();
            newPlane = await this.remotePlaneRepository.PostAsync(newPlane);
            return newPlane?.Id > 0;
        }

        private static Plane GenerateFromValues()
        {
            return new Plane
            {
                Msn = "MSN30013",
                Manufacturer = "Manufacturer 561896418",
                IsActive = false,
                IsMaintenance = true,
                FirstFlightDate = new DateTime(2027, 2, 3, 0, 0, 0, DateTimeKind.Utc),
                LastFlightDate = null,
                DeliveryDate = null,
                NextMaintenanceDate = new DateTime(2024, 12, 9, 0, 0, 0, DateTimeKind.Utc),
                SyncTime = null,
                SyncFlightDataTime = TimeSpan.Parse("03:44:05", System.Globalization.CultureInfo.InvariantCulture),
                Capacity = 45,
                MotorsCount = 1,
                TotalFlightHours = 9093,
                Probability = null,
                FuelCapacity = 2f,
                FuelLevel = null,
                OriginalPrice = 1911.24m,
                EstimatedPrice = null,
                SiteId = 1,
                PlaneTypeId = 2,
                CurrentAirportId = 7,
            };
        }
    }
}
