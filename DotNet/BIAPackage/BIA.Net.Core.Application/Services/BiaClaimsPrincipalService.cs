// <copyright file="BiaClaimsPrincipalService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using System.Security.Principal;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Authentication;

    /// <summary>
    /// Service for BIA Claims Principal.
    /// </summary>
    public class BiaClaimsPrincipalService : IBiaClaimsPrincipalService
    {
        private readonly BiaClaimsPrincipal biaClaimsPrincipal;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaClaimsPrincipalService"/> class.
        /// </summary>
        /// <param name="principal">The <see cref="IPrincipal"/>.</param>
        public BiaClaimsPrincipalService(IPrincipal principal)
        {
            if (!TryGetAsBiaClaimsPrincipal(principal, out this.biaClaimsPrincipal))
            {
                throw new BadRequestException("Bad claims format");
            }
        }

        /// <inheritdoc/>
        public int GetUserId()
        {
            return this.biaClaimsPrincipal.GetUserId();
        }

        /// <inheritdoc/>
        public T GetUserData<T>()
            where T : class
        {
            return this.biaClaimsPrincipal.GetUserData<T>();
        }

        /// <inheritdoc/>
        public BiaClaimsPrincipal GetBiaClaimsPrincipal()
        {
            return this.biaClaimsPrincipal;
        }

        private static bool TryGetAsBiaClaimsPrincipal(IPrincipal principal, out BiaClaimsPrincipal result)
        {
            var isBiaClaimsPrincipal = principal is BiaClaimsPrincipal;
            result = isBiaClaimsPrincipal ? principal as BiaClaimsPrincipal : null;
            return isBiaClaimsPrincipal;
        }
    }
}
