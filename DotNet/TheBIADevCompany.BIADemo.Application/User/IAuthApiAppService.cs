// <copyright file="AuthApiAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System.Threading.Tasks;

    public interface IAuthApiAppService
    {
        Task<string> LoginAsync();
    }
}