// <copyright file="ServiceApiRwController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Base
{
    using BIA.Net.Core.Common;
    using Microsoft.AspNetCore.Authorization;

    /// <summary>
    /// Service Api Rw Controller.
    /// </summary>
    [Authorize(Policy = BiaConstants.Policy.ServiceApiRW)]
    public abstract class ServiceApiRwController : AuthControllerBase
    {
    }
}
