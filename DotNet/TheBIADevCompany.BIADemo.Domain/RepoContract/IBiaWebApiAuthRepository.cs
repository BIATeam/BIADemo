// <copyright file="IBiaWebApiAuthRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.RepoContract
{
    using System.Threading.Tasks;

    public interface IBiaWebApiAuthRepository
    {
        Task<string> LoginAsync(string baseAddress, string urlLogin = "/api/Auth/token");
    }
}