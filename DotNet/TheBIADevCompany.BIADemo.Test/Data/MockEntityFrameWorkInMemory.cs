// <copyright file="MockEntityFrameWorkInMemory.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Test.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BIA.Net.Core.Infrastructure.Data;
    using BIA.Net.Core.Test.Data;

    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;

    // End BIADemo
    using TheBIADevCompany.BIADemo.Domain.SiteModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.ViewModule.Aggregate;
    using TheBIADevCompany.BIADemo.Infrastructure.Data;

    /// <summary>
    /// Manage the mock of the DB context as an "in memory" database.
    /// </summary>
    /// <seealso cref="AbstractMockEntityFramework{TDbContext}"/>
    public class MockEntityFrameworkInMemory : AbstractMockEntityFramework<DataContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockEntityFrameworkInMemory"/> class.
        /// </summary>
        /// <param name="dbContext">The DB context.</param>
        public MockEntityFrameworkInMemory(IQueryableUnitOfWork dbContext)
            : base(dbContext)
        {
            // Do nothing. Used to create the DbContext through IoC.
        }

        #region Sites methods

        /// <inheritdoc cref="IDataSites.CountSites"/>
        public int CountSites()
        {
            return this.GetDbContext().Sites.Count();
        }

        /// <inheritdoc cref="IDataSites.GetSite(int)"/>
        public Site GetSite(int id)
        {
            return this.GetDbContext().Sites.Find(id);
        }

        /// <inheritdoc cref="IDataSites.InitDefaultSites"/>
        public void InitDefaultSites()
        {
            int id = 1;

            foreach (string title in DataConstants.DefaultSitesTitles)
            {
                this.GetDbContext().Sites.Add(new Site
                {
                    Id = id++,
                    Title = title,
                    Members = new List<Member>(),
                });
            }

            this.GetDbContext().SaveChanges();
        }

        #endregion Sites methods

        #region Users methods

        /// <inheritdoc cref="IDataUsers.AddMember(int, int, int, ICollection{MemberRole})"/>
        public void AddMember(int id, int siteId, int userId, ICollection<MemberRole> roles)
        {
            this.GetDbContext().Members.Add(new Member()
            {
                Id = id,
                SiteId = siteId,
                UserId = userId,
                MemberRoles = roles,
            });
            this.GetDbContext().SaveChanges();
        }

        /// <inheritdoc cref="IDataUsers.AddUser(int, string, string, int?, int?, ICollection{MemberRole})"/>
        public void AddUser(int id, string firstName, string lastName, int? memberId = null, int? memberSiteId = null, ICollection<MemberRole> memberRoles = null)
        {
            this.GetDbContext().Users.Add(new User()
            {
                Id = id,
                Company = "TheBIADevCompany",
                Country = "France",
                DaiDate = new DateTime(2000, 1, 1),
                Department = "DM",
                DistinguishedName = "DistinguishedName",
                Domain = "EU",
                Email = $"{firstName}{lastName}@fake.com",
                ExternalCompany = string.Empty,
                FirstName = firstName,
                Guid = Guid.Empty,
                IsActive = true,
                IsEmployee = true,
                IsExternal = false,
                LastLoginDate = DateTime.Now.AddDays(-2),
                LastName = lastName,
                Login = firstName + lastName,
                Manager = "Big BOSS",
                Members = new List<Member>(),
                Office = "101",
                Sid = $"SID-{firstName}{lastName}",
                Site = DataConstants.DefaultSitesTitles[0],
                SubDepartment = "BIA",
                ViewUsers = new List<ViewUser>(),
            });

            if (memberId != null)
            {
                this.AddMember(memberId.Value, memberSiteId.Value, id, memberRoles);
            }
            else
            {
                // We do not save changes in the "if", because it is already done by AddMember().
                this.GetDbContext().SaveChanges();
            }
        }

        #endregion Users methods

        // Begin BIADemo
        #region Planes methods

        /// <inheritdoc cref="IDataPlanes.InitDefaultPlanes"/>
        public void InitDefaultPlanes()
        {
            this.GetDbContext().Planes.Add(new Plane
            {
                SiteId = 1,
                Id = 1,
                Capacity = 200,
                FirstFlightDate = new DateTime(2000, 1, 1),
                IsActive = true,
                LastFlightDate = DateTime.Now,
                Msn = DataConstants.DefaultPlanesMsn[0],
            });
            this.GetDbContext().Planes.Add(new Plane
            {
                SiteId = 1,
                Id = 2,
                Capacity = 300,
                FirstFlightDate = new DateTime(2001, 2, 3),
                IsActive = true,
                LastFlightDate = DateTime.Now,
                Msn = DataConstants.DefaultPlanesMsn[1],
            });
            this.GetDbContext().SaveChanges();
        }

        /// <inheritdoc cref="IDataPlanes.CountPlanes"/>
        public int CountPlanes()
        {
            return this.GetDbContext().Planes.Count();
        }

        /// <inheritdoc cref="IDataPlanes.GetPlane(int)"/>
        public Plane GetPlane(int id)
        {
            return this.GetDbContext().Planes.Find(id);
        }

        #endregion Planes methods

        // End BIADemo
        #region AbstractMockEntityFrameworkInMemory methods

        /// <inheritdoc cref="AbstractMockEntityFramework{TDbContext}.InitDefaultData" />
        public override void InitDefaultData()
        {
            this.InitDefaultSites();

            // Begin BIADemo
            this.InitDefaultPlanes();

            // End BIADemo
        }

        #endregion AbstractMockEntityFrameworkInMemory methods
    }
}
