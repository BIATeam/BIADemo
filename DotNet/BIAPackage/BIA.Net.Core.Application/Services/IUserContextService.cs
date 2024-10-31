// <copyright file="IUserContextService.cs" company="BIA">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    /// <summary>
    /// Interface for user context services.
    /// </summary>
    public interface IUserContextService
    {
        /// <summary>
        /// Retrieve the user context language id.
        /// </summary>
        /// <returns>Language id as int.</returns>
        int GetLanguageId();
    }
}