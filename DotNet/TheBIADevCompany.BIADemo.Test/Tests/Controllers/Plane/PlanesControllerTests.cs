// BIADemo only
// <copyright file="PlanesControllerTests.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Test.Tests.Controllers.Plane
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using BIA.Net.Core.Domain.Dto.User;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;
    using TheBIADevCompany.BIADemo.Presentation.Api.Controllers;

    /// <summary>
    /// Class used to test <see cref="PlanesController"/> methods.
    /// </summary>
    /// <seealso cref="AbstractUnitTest" />
    [TestClass]
    public class PlanesControllerTests : AbstractUnitTest
    {
        /// <summary>
        /// The controller to test.
        /// </summary>
        private PlanesController controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanesControllerTests"/> class.
        /// </summary>
        public PlanesControllerTests()
            : base(true)
        {
            // Initialize AbstractUnitTest.isInitDB to true, which is used to start with some default data in the DB for each test of this test suite.
        }

        /// <summary>
        /// Method used to initialize the context before each test.
        /// </summary>
        [TestInitialize]
        public void InitTest()
        {
            this.principalBuilder
                .MockPrincipalUserData(new UserDataDto()
                {
                    CurrentSiteId = 1,
                });

            // Create a new instance of the controller to test and set its HttpContext.
            this.controller = this.GetControllerWithHttpContext<PlanesController>();
        }

        /// <summary>
        /// Test <see cref="PlanesController.GetAll(BIA.Net.Core.Domain.Dto.Base.LazyLoadDto)"/> method.
        /// </summary>
        [TestMethod("PlanesControllerTests.GetAllTest")]
        public void GetAllTest()
        {
            ObjectResult result = this.controller.GetAll(null).Result as ObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);

            IEnumerable<PlaneDto> planes = result.Value as IEnumerable<PlaneDto>;
            Assert.IsNotNull(planes);
            Assert.AreEqual(2, planes.Count());
        }
    }
}