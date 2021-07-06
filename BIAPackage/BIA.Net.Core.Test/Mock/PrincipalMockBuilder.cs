// <copyright file="PrincipalMockBuilder.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Test.Mock
{
    using System.Collections.Generic;
    using BIA.Net.Core.Application.Authentication;
    using BIA.Net.Core.Test.IoC;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;

    /// <summary>
    /// Builder class used to create a mock of <see cref="BIAClaimsPrincipal"/>.
    /// </summary>
    public class PrincipalMockBuilder
    {
        /// <summary>
        /// The mock to build.
        /// </summary>
        private readonly Mock<BIAClaimsPrincipal> mock;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrincipalMockBuilder"/> class.
        /// </summary>
        public PrincipalMockBuilder()
        {
            this.mock = new Mock<BIAClaimsPrincipal>();
        }

        /// <summary>
        /// Get the mocked object.
        /// </summary>
        /// <returns>The mocked principal.</returns>
        public BIAClaimsPrincipal Build()
        {
            return this.mock.Object;
        }

        /// <summary>
        /// Build and apply the currently configured mock to the given collection of services.
        /// 
        /// Note: Contrary to <see cref="Build"/>, this method does not only return the mocked object, but also applies it to the 
        /// dependency injection system.
        /// </summary>
        /// <param name="services">The collection of services to update.</param>
        /// <returns>The mocked principal.</returns>
        public BIAClaimsPrincipal BuildAndApply(IServiceCollection services)
        {
            BIAClaimsPrincipal mock = this.Build();
            BIAIocContainerTest.ApplyPrincipalMock(services, mock);

            return mock;
        }

        /// <summary>
        /// Mock the value returned by the <see cref="BIAClaimsPrincipal.GetUserId"/> method.
        /// </summary>
        /// <param name="userId">The user ID to return.</param>
        /// <returns>The updated mock builder.</returns>
        public PrincipalMockBuilder MockPrincipalUserId(int userId)
        {
            this.mock.Setup(p => p.GetUserId())
                .Returns(userId);

            return this;
        }

        /// <summary>
        /// Mock the value returned by the <see cref="BIAClaimsPrincipal.GetUserData{T}"/> method.
        /// </summary>
        /// <param name="userData">The user data to return.</param>
        /// <returns>The updated mock builder.</returns>
        public PrincipalMockBuilder MockPrincipalUserData<T>(T userData)
        {
            this.mock.Setup(p => p.GetUserData<T>())
                .Returns(userData);

            return this;
        }

        /// <summary>
        /// Mock the value returned by the <see cref="BIAClaimsPrincipal.GetUserRights"/> method.
        /// </summary>
        /// <param name="rights">The list of rights to return.</param>
        /// <returns>The updated mock builder.</returns>
        public PrincipalMockBuilder MockPrincipalUserRights(IEnumerable<string> rights)
        {
            this.mock.Setup(p => p.GetUserRights())
                .Returns(rights);

            return this;
        }

        /// <summary>
        /// Mock the value returned by the <see cref="BIAClaimsPrincipal.GetUserLogin"/> method.
        /// </summary>
        /// <param name="login">The user login to return.</param>
        /// <returns>The updated mock builder.</returns>
        public PrincipalMockBuilder MockPrincipalUserLogin(string login)
        {
            this.mock.Setup(p => p.GetUserLogin())
                .Returns(login);

            return this;
        }
    }
}
