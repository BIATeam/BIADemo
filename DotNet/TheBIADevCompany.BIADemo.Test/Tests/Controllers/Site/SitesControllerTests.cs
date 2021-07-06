// <copyright file="SitesControllerTests.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Test.Tests.Controllers.Site
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;
    using TheBIADevCompany.BIADemo.Presentation.Api.Controllers;
    using TheBIADevCompany.BIADemo.Test.Data;

    /// <summary>
    /// Class used to test <see cref="SitesController"/> methods.
    /// </summary>
    /// <seealso cref="AbstractUnitTest" />
    [TestClass]
    public class SitesControllerTests : AbstractUnitTest
    {
        /// <summary>
        /// The controller to test.
        /// </summary>
        private SitesController controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="SitesControllerTests"/> class.
        /// </summary>
        public SitesControllerTests()
            : base(false)
        {
            // Initialize AbstractUnitTest.isInitDB to false, which is used to start with an empty DB for each test of this test suite.
        }

        /// <summary>
        /// Method used to initialize the context before each test.
        /// </summary>
        [TestInitialize]
        public void InitTest()
        {
            // Create a new instance of the controller to test and set its HttpContext.
            this.controller = this.GetControllerWithHttpContext<SitesController>();
        }

        /// <summary>
        /// Test site removal from the controller.
        /// </summary>
        [TestMethod("SitesControllerTests.RemoveSiteByController")]
        public void RemoveSiteByController()
        {
            // Add default data in DB.
            this.DbMock.InitDefaultSites();
            int nbSites = this.DbMock.CountSites();

            // Remove existing site.
            IStatusCodeActionResult response = this.controller.Remove(2).Result as IStatusCodeActionResult;
            Assert.IsNotNull(response);
            Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(nbSites - 1, this.DbMock.CountSites());

            // Check first site is still OK.
            var site = this.DbMock.GetSite(1);
            Assert.IsNotNull(site);
            Assert.AreEqual(DataConstants.DefaultSitesTitles[0], site.Title);

            // Check second site has been removed.
            Assert.IsNull(this.DbMock.GetSite(2));

            // Check third site is still OK.
            site = this.DbMock.GetSite(3);
            Assert.IsNotNull(site);
            Assert.AreEqual(DataConstants.DefaultSitesTitles[2], site.Title);
        }

        /// <summary>
        /// Test outcomes of <see cref="SitesController.Remove(int)"/> based on the given site ID.
        /// </summary>
        /// <param name="siteId">The ID of the site to remove.</param>
        /// <param name="expectedResult">The expected HTTP status.</param>
        [DataTestMethod]
        [DataRow(-1, HttpStatusCode.NotFound)]
        [DataRow(0, HttpStatusCode.BadRequest)]
        [DataRow(1, HttpStatusCode.OK)]
        [DataRow(2, HttpStatusCode.OK)]
        [DataRow(404, HttpStatusCode.NotFound)]
        public void TryRemoveSitesByController(int siteId, HttpStatusCode expectedResult)
        {
            // Add default data in DB.
            this.DbMock.InitDefaultSites();
            int nbSites = this.DbMock.CountSites();

            IStatusCodeActionResult response = this.controller.Remove(siteId).Result as IStatusCodeActionResult;

            Assert.IsNotNull(response);
            Assert.AreEqual((int)expectedResult, response.StatusCode);

            // If removal is supposed to be successful, check that the item has really been removed.
            if (expectedResult == HttpStatusCode.OK)
            {
                Assert.IsNull(this.DbMock.GetSite(siteId));
                nbSites -= 1;
            }

            Assert.AreEqual(nbSites, this.DbMock.CountSites());
        }

        /// <summary>
        /// Test outcomes of <see cref="SitesController.Update(int, SiteDto)"/> based on the given site ID.
        /// </summary>
        /// <param name="siteId">The site identifier.</param>
        /// <param name="expectedResult">The expected result.</param>
        [DataTestMethod]
        [DataRow(-1, HttpStatusCode.NotFound)]
        [DataRow(0, HttpStatusCode.BadRequest)]
        [DataRow(1, HttpStatusCode.OK)]
        [DataRow(2, HttpStatusCode.OK)]
        [DataRow(404, HttpStatusCode.NotFound)]
        public void TryUpdateSitesByController(int siteId, HttpStatusCode expectedResult)
        {
            // Add default data in DB.
            this.DbMock.InitDefaultSites();
            int nbSites = this.DbMock.CountSites();

            string newTitle = "New title";
            SiteDto newSite = new SiteDto()
            {
                Id = siteId,
                Title = newTitle,
            };

            IStatusCodeActionResult statusResponse = this.controller.Update(siteId, newSite).Result as IStatusCodeActionResult;

            Assert.IsNotNull(statusResponse);
            Assert.AreEqual((int)expectedResult, statusResponse.StatusCode);
            Assert.AreEqual(nbSites, this.DbMock.CountSites());

            if (expectedResult == HttpStatusCode.OK)
            {
                Assert.AreEqual(newTitle, this.DbMock.GetSite(siteId).Title);
            }
        }

        /// <summary>
        /// Test outcomes of <see cref="SitesController.Update(int, SiteDto)"/> when the ID of the site to update is not the same as the given one.
        /// </summary>
        [TestMethod("SitesControllerTests.TryUpdateSite_NotMatchingIds_ByController")]
        public void TryUpdateSite_NotMatchingIds_ByController()
        {
            // Add default data in DB.
            this.DbMock.InitDefaultSites();
            int nbSites = this.DbMock.CountSites();

            SiteDto newSite = new SiteDto()
            {
                Id = 2,
                Title = "Site with not matching ID",
            };

            IStatusCodeActionResult statusResponse = this.controller.Update(3, newSite).Result as IStatusCodeActionResult;

            Assert.IsNotNull(statusResponse);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, statusResponse.StatusCode);

            // Check that nothing has changed in DB.
            Assert.AreEqual(DataConstants.DefaultSitesTitles[1], this.DbMock.GetSite(2).Title);
            Assert.AreEqual(DataConstants.DefaultSitesTitles[2], this.DbMock.GetSite(3).Title);
            Assert.AreEqual(nbSites, this.DbMock.CountSites());
        }

        /// <summary>
        /// Test site update workflow by controller.
        /// </summary>
        [TestMethod("SitesControllerTests.UpdateSiteWorkflowByController")]
        public void UpdateSiteWorkflowByController()
        {
            #region Setup additional context

            // Add default data in DB.
            this.DbMock.InitDefaultSites();

            // Add member: required for the GetAll() method.
            this.DbMock.AddUser(1, "John", "DOE", 1, 1, new List<MemberRole>
                {
                    new MemberRole()
                    {
                        MemberId = 1,
                    },
                });

            this.principalBuilder.MockPrincipalUserRights(new List<string>
                {
                    Rights.Sites.AccessAll,
                });

            #endregion Setup additional context

            // Check GetAll behavior (used when displaying the list of available sites).
            SiteFilterDto filter = new SiteFilterDto()
            {
                Filters = null,
                UserId = 1,
            };
            ObjectResult response = this.controller.GetAll(filter).Result as ObjectResult;
            Assert.IsNotNull(response);
            Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);

            IEnumerable<SiteInfoDto> listSites = response.Value as IEnumerable<SiteInfoDto>;
            Assert.IsNotNull(listSites);
            Assert.AreEqual(1, listSites.Count());

            // Check Get behavior (used when displaying the "Edit" popup).
            response = this.controller.Get(1).Result as ObjectResult;
            Assert.IsNotNull(response);
            Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);

            SiteDto selectedSite = response.Value as SiteDto;
            Assert.IsNotNull(selectedSite);
            Assert.AreEqual(DataConstants.DefaultSitesTitles[0], selectedSite.Title);
            Assert.AreEqual(1, selectedSite.Id);

            // Check Update behavior (used when validating the "Edit" popup).
            string newTitle = "New site 1";
            SiteDto newSite = new SiteDto()
            {
                Id = 1,
                Title = newTitle,
            };
            response = this.controller.Update(1, newSite).Result as ObjectResult;
            Assert.IsNotNull(response);
            Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);

            // - Check response of the Update API
            selectedSite = response.Value as SiteDto;
            Assert.IsNotNull(selectedSite);
            Assert.AreEqual(newTitle, selectedSite.Title);
            Assert.AreEqual(1, selectedSite.Id);

            // - Check DB content.
            Assert.AreEqual(newTitle, this.DbMock.GetSite(1).Title);
        }

        /// <summary>
        /// Test outcomes of <see cref="SitesController.Add(SiteDto)"/> based on the given site ID.
        /// Note: for this test, we will encounter <see cref="HttpStatusCode.InternalServerError"/> when trying to add a site with an existing ID.
        /// </summary>
        /// <param name="siteId">The site identifier.</param>
        /// <param name="expectedResult">The expected result.</param>
        [DataTestMethod]
        [DataRow(-1, HttpStatusCode.Created)]
        [DataRow(0, HttpStatusCode.Created)]
        [DataRow(1, HttpStatusCode.InternalServerError)]
        [DataRow(2, HttpStatusCode.InternalServerError)]
        [DataRow(404, HttpStatusCode.Created)]
        public void TryInsertSitesByController(int siteId, HttpStatusCode expectedResult)
        {
            // Add default data in DB.
            this.DbMock.InitDefaultSites();
            int nbSites = this.DbMock.CountSites();

            string siteTitle = "New title";
            SiteDto newSite = new SiteDto()
            {
                Id = siteId,
                Title = siteTitle,
            };

            IStatusCodeActionResult statusResponse = this.controller.Add(newSite).Result as IStatusCodeActionResult;

            Assert.IsNotNull(statusResponse);
            Assert.AreEqual((int)expectedResult, statusResponse.StatusCode);

            if (expectedResult == HttpStatusCode.Created)
            {
                nbSites += 1;

                // Specific use-case: when site ID is 0, we use an automatically incremented ID, otherwise we use the given ID.
                siteId = (siteId == 0) ? nbSites : siteId;

                Assert.AreEqual(siteTitle, this.DbMock.GetSite(siteId).Title);
            }

            Assert.AreEqual(nbSites, this.DbMock.CountSites());
        }

        /// <summary>
        /// Test site creation by controller.
        /// </summary>
        [TestMethod("SitesControllerTests.InsertSiteByController")]
        public void InsertSiteByController()
        {
            // Try to add null site: it shall fail.
            IStatusCodeActionResult response = this.controller.Add(null).Result as IStatusCodeActionResult;
            Assert.IsNotNull(response);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.AreEqual(0, this.DbMock.CountSites());

            // Add site.
            SiteDto siteDto = new SiteDto()
            {
                Title = "PST",
            };

            response = this.controller.Add(siteDto).Result as IStatusCodeActionResult;
            Assert.IsNotNull(response);
            Assert.AreEqual((int)HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(1, this.DbMock.CountSites());
            Assert.AreEqual("PST", this.DbMock.GetSite(1).Title);

            // Add site.
            siteDto = new SiteDto()
            {
                Title = "TMR",
            };

            response = this.controller.Add(siteDto).Result as IStatusCodeActionResult;
            Assert.IsNotNull(response);
            Assert.AreEqual((int)HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(2, this.DbMock.CountSites());
            Assert.AreEqual("PST", this.DbMock.GetSite(1).Title);
            Assert.AreEqual("TMR", this.DbMock.GetSite(2).Title);
        }

        /// <summary>
        /// Create a site, then update it by controller.
        /// </summary>
        [TestMethod("SitesControllerTests.UpdateCreatedSiteByController")]
        public void UpdateCreatedSiteByController()
        {
            // Create new site.
            SiteDto siteDto = new SiteDto
            {
                Title = "TLS",
            };
            IStatusCodeActionResult response = this.controller.Add(siteDto).Result as IStatusCodeActionResult;
            Assert.IsNotNull(response);
            Assert.AreEqual((int)HttpStatusCode.Created, response.StatusCode);

            var site = this.DbMock.GetSite(1);
            Assert.AreEqual("TLS", site.Title);

            // Update the previously created site.
            siteDto.Title = "BLC";
            response = this.controller.Update(1, siteDto).Result as IStatusCodeActionResult;
            Assert.IsNotNull(response);
            Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);

            site = this.DbMock.GetSite(1);
            Assert.AreEqual("BLC", site.Title);
        }
    }
}
