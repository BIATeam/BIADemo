// <copyright file="BiaWebApiRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration.BiaWebApi;

    public interface IBiaWebApiRepository
    {
        string BaseAddress { get; }

        void Init(BiaWebApi biaWebApi);

        Task<string> GetTokenAsync(string url = "/api/Auth/token");

        Task<string> LoginAsync(string url = "/api/Auth/login?lightToken=false");
    }
}