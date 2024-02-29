// BIADemo only
// <copyright file="IPlaneAirportRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.RepoContract
{
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;

    /// <summary>
    /// Interface PlaneAirport Repository.
    /// </summary>
    public interface IPlaneAirportRepository
    {
        void Add(PlaneAirport planeAirport);

        void Remove(PlaneAirport planeAirport);
    }
}