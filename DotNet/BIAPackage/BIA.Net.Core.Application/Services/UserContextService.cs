// <copyright file="UserContextService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using BIA.Net.Core.Domain.Service;

    /// <summary>
    /// User context service.
    /// </summary>
    public class UserContextService : IUserContextService
    {
        private readonly UserContext userContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserContextService"/> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        public UserContextService(UserContext userContext)
        {
            this.userContext = userContext;
        }

        /// <inheritdoc/>
        public int GetLanguageId()
        {
            return this.userContext.LanguageId;
        }
    }
}
