// BIADemo only
// <copyright file="PlaneAppServiceTests.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Test.Tests.Services.Plane
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BIA.Net.Core.Domain.Dto.User;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TheBIADevCompany.BIADemo.Application.Plane;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;
    using TheBIADevCompany.BIADemo.Test.Data;

    /// <summary>
    /// Class used to test <see cref="PlaneAppService"/> methods.
    /// </summary>
    /// <seealso cref="AbstractUnitTest" />
    [TestClass]
    public class PlaneAppServiceTests : AbstractUnitTest
    {
        /// <summary>
        /// The service to test.
        /// </summary>
        private IPlaneAppService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneAppServiceTests"/> class.
        /// </summary>
        public PlaneAppServiceTests()
            : base(true)
        {
            // Initialize AbstractUnitTest.isInitDB to true, which is used to start with some default data in the DB for each test of this test suite.
        }

        /// <summary>
        /// Initialize the context before each test.
        /// </summary>
        [TestInitialize]
        public void InitTest()
        {
            // Mock authentication data (IPrincipal).
            this.principalBuilder
                .MockPrincipalUserData(new UserDataDto()
                {
                    CurrentSiteId = 1,
                });

            this.service = this.GetService<IPlaneAppService>();
        }

        /// <summary>
        /// Test <see cref="IPlaneAppService.GetAllAsync(TFilterDto)"/> method.
        /// </summary>
        [TestMethod("PlaneAppServiceTests.GetAllAsyncTest")]
        public void GetAllAsyncTest()
        {
            (IEnumerable<PlaneDto> results, int total) = this.service.GetRangeAsync(null).Result;
            Assert.AreEqual(2, total);
            List<PlaneDto> planes = results.ToList();
            Assert.AreEqual(DataConstants.DefaultPlanesMsn[0], planes[0].Msn);
            Assert.AreEqual(DataConstants.DefaultPlanesMsn[1], planes[1].Msn);
        }

        /// <summary>
        /// Test <see cref="IPlaneAppService.AddAsync(TDto)"/> method.
        /// </summary>
        [TestMethod("PlaneAppServiceTests.AddAsyncTest")]
        public void AddAsyncTest()
        {
            // Retrieve the initial number of planes in DB.
            int nbPlanesDefault = this.DbMock.CountPlanes();

            // Add new plane.
            int id = 3;
            int capacity = 333;
            DateTime firstFlightDate = new DateTime(2003, 3, 3);
            DateTime firstFlightTime = new DateTime(2003, 3, 3, 12, 0, 0);
            bool isActive = false;
            DateTime lastFlightDate = new DateTime(2013, 4, 4);
            string msn = "AB-0001";

            PlaneDto dto = this.service.AddAsync(new PlaneDto()
            {
                Id = id,
                Capacity = capacity,
                FirstFlightDate = firstFlightDate,
                FirstFlightTime = firstFlightTime,
                IsActive = isActive,
                LastFlightDate = lastFlightDate,
                Msn = msn,
            }).Result;
            Assert.IsNotNull(dto);

            // Check that the number of planes has increased.
            int nbPlanes = this.DbMock.CountPlanes();
            Assert.AreEqual(nbPlanesDefault + 1, nbPlanes);

            // Check that the plane has been correctly stored in DB.
            Plane plane = this.DbMock.GetPlane(id);
            Assert.AreEqual(capacity, plane.Capacity);
            Assert.AreEqual(firstFlightTime, plane.FirstFlightDate);
            Assert.AreEqual(id, plane.Id);
            Assert.AreEqual(isActive, plane.IsActive);
            Assert.AreEqual(lastFlightDate, plane.LastFlightDate);
            Assert.AreEqual(msn, plane.Msn);
        }
    }
}
