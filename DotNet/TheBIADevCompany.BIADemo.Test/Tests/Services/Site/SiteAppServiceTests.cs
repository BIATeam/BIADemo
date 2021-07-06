// <copyright file="SiteAppServiceTests.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Test.Tests.Services.Site
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.User;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TheBIADevCompany.BIADemo.Application.Site;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;
    using TheBIADevCompany.BIADemo.Test.Data;

    /// <summary>
    /// Class used to test <see cref="SiteAppService"/> methods.
    /// </summary>
    /// <seealso cref="AbstractUnitTest" />
    [TestClass]
    public class SiteAppServiceTests : AbstractUnitTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteAppServiceTests"/> class.
        /// </summary>
        public SiteAppServiceTests()
            : base(true)
        {
            // Initialize AbstractUnitTest.isInitDB to true, which is used to start with some default data in the DB for each test of this test suite.
        }

        /// <summary>
        /// Test <see cref="ISiteAppService.GetAllAsync"/> method when user has <see cref="Rights.Sites.AccessAll"/> rights.
        /// </summary>
        [TestMethod("SiteAppServiceTests.GetAllAsync_UserRights_AccessAllTest")]
        public void GetAllAsync_UserRights_AccessAllTest()
        {
            #region Setup context
            // Mock authentication data (IPrincipal).
            this.principalBuilder.MockPrincipalUserRights(new List<string>
                {
                    Rights.Sites.AccessAll,
                });

            // Initialize the service to test.
            ISiteAppService service = this.GetService<ISiteAppService>();
            #endregion Setup context

            IEnumerable<SiteDto> sites = service.GetAllAsync().Result;

            // All existing sites are returned.
            Assert.IsNotNull(sites);
            Assert.AreEqual(3, sites.Count());
        }

        /// <summary>
        /// Test <see cref="ISiteAppService.GetAllAsync"/> method when user:
        /// - has <see cref="Rights.Sites.ListAccess"/> rights
        /// - is not a member of any site.
        /// </summary>
        [TestMethod("SiteAppServiceTests.GetAllAsync_UserRights_ListAccess_NotMemberTest")]
        public void GetAllAsync_UserRights_ListAccess_NotMemberTest()
        {
            #region Setup context
            // Mock authentication data (IPrincipal).
            this.principalBuilder.MockPrincipalUserId(1)
                .MockPrincipalUserRights(new List<string>
                {
                        Rights.Sites.ListAccess,
                });

            // Initialize the service to test.
            ISiteAppService service = this.GetService<ISiteAppService>();
            #endregion Setup context

            IEnumerable<SiteDto> sites = service.GetAllAsync().Result;

            // No site is returned.
            Assert.IsNotNull(sites);
            Assert.AreEqual(0, sites.Count());
        }

        /// <summary>
        /// Test <see cref="ISiteAppService.GetAllAsync"/> method when user:
        /// - has <see cref="Rights.Sites.ListAccess"/> rights
        /// - is member of one site.
        /// </summary>
        [TestMethod("SiteAppServiceTests.GetAllAsync_UserRights_ListAccess_MemberTest")]
        public void GetAllAsync_UserRights_ListAccess_MemberTest()
        {
            #region Setup context
            // Mock authentication data (IPrincipal).
            this.principalBuilder.MockPrincipalUserId(1)
                .MockPrincipalUserRights(new List<string>
                {
                        Rights.Sites.ListAccess,
                });

            // Insert additional data in the DB.
            this.DbMock.AddUser(1, "John", "DOE", 1, 1, null);

            // Initialize the service to test.
            ISiteAppService service = this.GetService<ISiteAppService>();
            #endregion Setup context

            IEnumerable<SiteDto> sites = service.GetAllAsync().Result;

            // Only one site is returned (the one the user is a member of).
            Assert.IsNotNull(sites);
            Assert.AreEqual(1, sites.Count());

            SiteDto site = sites.First();
            Assert.AreEqual(1, site.Id);
            Assert.AreEqual(DataConstants.DefaultSitesTitles[0], site.Title);
        }

        /// <summary>
        /// Test <see cref="ISiteAppService.GetAllWithMembersAsync(SiteFilterDto)"/> method when user:
        /// - has <see cref="Rights.Sites.ListAccess"/> rights
        /// - is member of one site.
        /// </summary>
        [TestMethod("SiteAppServiceTests.GetAllWithMembersAsync_UserRights_ListAccess_MemberTest")]
        public void GetAllWithMembersAsync_UserRights_ListAccess_MemberTest()
        {
            #region Setup context
            // Mock authentication data (IPrincipal).
            this.principalBuilder.MockPrincipalUserRights(new List<string>
                {
                        Rights.Sites.ListAccess,
                })
                .MockPrincipalUserData(new UserDataDto()
                {
                    CurrentSiteId = 1,
                });

            // Insert additional data in the DB.
            this.DbMock.AddUser(1, "John", "DOE", 1, 1, null);

            // Initialize the service to test.
            ISiteAppService service = this.GetService<ISiteAppService>();
            #endregion Setup context

            SiteFilterDto filters = new SiteFilterDto()
            {
                Filters = new Dictionary<string, Dictionary<string, object>>(),
            };
            (IEnumerable<SiteInfoDto> sites, int total) = service.GetAllWithMembersAsync(filters).Result;

            // Only one site is returned (the one the user is a member of).
            Assert.IsNotNull(sites);
            Assert.AreEqual(1, total);

            SiteInfoDto site = sites.First();
            Assert.AreEqual(1, site.Id);
            Assert.AreEqual(DataConstants.DefaultSitesTitles[0], site.Title);
        }

        /// <summary>
        /// Test <see cref="ISiteAppService.GetAsync"/> method.
        /// </summary>
        [TestMethod("SiteAppServiceTests.GetAsyncTest")]
        public void GetAsyncTest()
        {
            // Initialize the service to test.
            ISiteAppService service = this.GetService<ISiteAppService>();

            SiteDto site = service.GetAsync(1).Result;

            Assert.IsNotNull(site);
            Assert.AreEqual(DataConstants.DefaultSitesTitles[0], site.Title);
        }

        /// <summary>
        /// Test <see cref="ISiteAppService.AddAsync"/> method.
        /// </summary>
        [TestMethod("SiteAppServiceTests.AddAsyncTest")]
        public void AddAsyncTest()
        {
            // Initialize the service to test.
            ISiteAppService service = this.GetService<ISiteAppService>();

            SiteDto siteDto = new SiteDto()
            {
                Title = "TLS",
            };
            SiteDto site = service.AddAsync(siteDto).Result;

            Assert.IsNotNull(site);

            Assert.AreEqual(4, this.DbMock.CountSites());
            Assert.AreEqual("TLS", this.DbMock.GetSite(DataConstants.DefaultSitesTitles.Length + 1).Title);
        }

        /// <summary>
        /// Test <see cref="ISiteAppService.RemoveAsync"/> method.
        /// </summary>
        /// <returns>The <see cref="Task"/> representing the asynchronous operation to perform.</returns>
        [TestMethod("SiteAppServiceTests.RemoveAsyncTest")]
        public async Task RemoveAsyncTest()
        {
            // Initialize the service to test.
            ISiteAppService service = this.GetService<ISiteAppService>();

            await service.RemoveAsync(2);

            Assert.AreEqual(2, this.DbMock.CountSites());
            Assert.IsNull(this.DbMock.GetSite(2));
            Assert.AreEqual(DataConstants.DefaultSitesTitles[0], this.DbMock.GetSite(1).Title);
            Assert.AreEqual(DataConstants.DefaultSitesTitles[2], this.DbMock.GetSite(3).Title);
        }

        /// <summary>
        /// Test <see cref="ISiteAppService.UpdateAsync"/> method.
        /// </summary>
        [TestMethod("SiteAppServiceTests.UpdateAsyncTest")]
        public void UpdateAsyncTest()
        {
            // Initialize the service to test.
            ISiteAppService service = this.GetService<ISiteAppService>();

            SiteDto siteDto = new SiteDto()
            {
                Id = 2,
                Title = "TLS",
            };
            SiteDto site = service.UpdateAsync(siteDto).Result;

            Assert.IsNotNull(site);

            Assert.AreEqual(3, this.DbMock.CountSites());
            Assert.AreEqual("TLS", this.DbMock.GetSite(2).Title);
        }
    }
}