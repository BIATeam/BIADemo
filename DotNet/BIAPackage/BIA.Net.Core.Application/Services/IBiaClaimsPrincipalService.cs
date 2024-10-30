// <copyright file="IBiaClaimsPrincipalService.cs" company="BIA">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using BIA.Net.Core.Domain.Authentication;

    /// <summary>
    /// Interface for BIA claims principal services.
    /// </summary>
    public interface IBiaClaimsPrincipalService
    {
        /// <summary>
        /// Retrieve the user data from principal claims as <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to map the user data.</typeparam>
        /// <returns><typeparamref name="T"/>.</returns>
        T GetUserData<T>()
            where T : class;

        /// <summary>
        /// Retrieve the user id from principal claims.
        /// </summary>
        /// <returns>User id as int.</returns>
        int GetUserId();

        /// <summary>
        /// Return the principal claims as <see cref="BiaClaimsPrincipal"/>.
        /// </summary>
        /// <returns><see cref="BiaClaimsPrincipal"/>.</returns>
        BiaClaimsPrincipal GetBiaClaimsPrincipal();
    }
}