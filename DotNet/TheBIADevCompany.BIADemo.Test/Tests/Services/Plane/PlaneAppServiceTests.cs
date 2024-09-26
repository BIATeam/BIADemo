// BIADemo only
// <copyright file="PlaneAppServiceTests.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Test.Tests.Services.Plane
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TheBIADevCompany.BIADemo.Application.Plane;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
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
            this.PrincipalBuilder
                .MockPrincipalUserData(new UserDataDto()
                {
                    CurrentTeams =
                    {
                        new CurrentTeamDto()
                        {
                            TeamTypeId = (int)TeamTypeId.Site,
                            TeamId = 1,
                        },
                    },
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
            DateTime deliveryDate = new DateTime(1990, 10, 10, 0, 0, 0, DateTimeKind.Utc);
            string syncTime = "12:00:00";
            bool isActive = false;
            DateTime lastFlightDate = new DateTime(2013, 4, 4, 0, 0, 0, DateTimeKind.Utc);
            string msn = "AB-0001";
            OptionDto airport = new OptionDto() { Id = 1, Display = "BDX" };

            PlaneDto dto = this.service.AddAsync(new PlaneDto()
            {
                Id = id,
                Capacity = capacity,
                IsActive = isActive,
                LastFlightDate = lastFlightDate,
                DeliveryDate = deliveryDate,
                SyncTime = syncTime,
                Msn = msn,
                SyncFlightDataTime = syncTime,
                CurrentAirport = airport,
            }).Result;
            Assert.IsNotNull(dto);

            // Check that the number of planes has increased.
            int nbPlanes = this.DbMock.CountPlanes();
            Assert.AreEqual(nbPlanesDefault + 1, nbPlanes);

            // Check that the plane has been correctly stored in DB.
            Plane plane = this.DbMock.GetPlane(id);
            Assert.AreEqual(capacity, plane.Capacity);
            Assert.AreEqual(deliveryDate, plane.DeliveryDate);
            Assert.AreEqual(TimeSpan.Parse(syncTime, new CultureInfo("en-US")), plane.SyncTime);
            Assert.AreEqual(id, plane.Id);
            Assert.AreEqual(isActive, plane.IsActive);
            Assert.AreEqual(lastFlightDate, plane.LastFlightDate);
            Assert.AreEqual(msn, plane.Msn);
        }

        /// <summary>
        /// Test parallel requests with readonly context.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod("PlaneAppServiceTests.GetInParallelAsyncTest")]
        public async Task GetInParallelAsyncTest()
        {
            int id1 = 1;
            int id2 = 2;

            var task1 = this.service.GetAsync(id: id1, isReadOnlyMode: true);
            var task2 = this.service.GetAsync(id: id2, isReadOnlyMode: true);

            var obj1 = await task1;
            var obj2 = await task2;

            Assert.AreEqual(id1, obj1.Id);
            Assert.AreEqual(id2, obj2.Id);
        }
    }
}
