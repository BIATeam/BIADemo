// <copyright file="MockEntityFrameWorkInMemory.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Test.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using BIA.Net.Core.Domain.User.Entities;
    using BIA.Net.Core.Domain.View.Entities;
    using BIA.Net.Core.Infrastructure.Data;
    using BIA.Net.Core.Test.Data;
#if BIA_FRONT_FEATURE
    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    // End BIADemo
    using TheBIADevCompany.BIADemo.Domain.Site.Entities;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;
#endif
    using TheBIADevCompany.BIADemo.Infrastructure.Data;

    /// <summary>
    /// Manage the mock of the DB context as an "in memory" database.
    /// </summary>
    /// <seealso cref="AbstractMockEntityFramework{TDbContext}"/>
    public class MockEntityFrameworkInMemory : AbstractMockEntityFramework<DataContext, DataContextNoTracking>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockEntityFrameworkInMemory"/> class.
        /// </summary>
        /// <param name="dbContext">The DB context.</param>
        /// <param name="dbContextReadOnly">The DB context readonly.</param>
        public MockEntityFrameworkInMemory(IQueryableUnitOfWork dbContext, IQueryableUnitOfWorkNoTracking dbContextReadOnly)
            : base(dbContext, dbContextReadOnly)
        {
            // Do nothing. Used to create the DbContext through IoC.
        }
#if BIA_FRONT_FEATURE

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
                Site site = new Site
                {
                    Id = id++,
                    Title = title,
                    Members = new List<Member>(),
                };

                this.GetDbContext().Sites.Add(site);
            }

            this.GetDbContext().SaveChanges();
        }

        #endregion Sites methods

        #region Users methods

        /// <inheritdoc cref="IDataUsers.AddMember(int, int, int, ICollection{MemberRole})"/>
        public void AddMember(int id, int teamId, int userId, ICollection<MemberRole> roles)
        {
            Member member = new Member()
            {
                Id = id,
                TeamId = teamId,
                UserId = userId,
                MemberRoles = roles,
            };

            this.GetDbContext().Members.Add(member);
            this.GetDbContext().SaveChanges();
        }

        /// <inheritdoc cref="IDataUsers.AddUser(int, string, string, int?, int?, ICollection{MemberRole})"/>
        public void AddUser(int id, string firstName, string lastName, int? memberId = null, int? memberSiteId = null, ICollection<MemberRole> memberRoles = null)
        {
            User user = new User()
            {
                Id = id,
                Email = $"{firstName}{lastName}@fake.com",
                FirstName = firstName,
                IsActive = true,
                LastLoginDate = DateTime.Now.AddDays(-2),
                LastName = lastName,
                Login = firstName + lastName,
                Members = new List<Member>(),
                ViewUsers = new List<ViewUser>(),
                LastSyncDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
#if BIA_USER_CUSTOM_FIELDS_BACK
                Company = "TheBIADevCompany",
                Country = "France",
                Department = "DM",
                DistinguishedName = "DistinguishedName",
                ExternalCompany = string.Empty,
                IsEmployee = true,
                IsExternal = false,
                Manager = "Big BOSS",
                Office = "101",
                Site = DataConstants.DefaultSitesTitles[0],
                SubDepartment = "BIA",
#endif
            };

            this.GetDbContext().Users.Add(user);

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
            var airport = new Airport
            {
                Id = 1,
                City = "Bordeaux",
                Name = "BDX",
            };
            this.GetDbContext().Airports.Add(airport);

            var plane1 = new Plane
            {
                SiteId = 1,
                Id = 1,
                Capacity = 200,
                SyncTime = TimeSpan.Parse("00:00", new CultureInfo("en-US")),
                DeliveryDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true,
                LastFlightDate = DateTime.Now,
                SyncFlightDataTime = TimeSpan.Parse("10:00", new CultureInfo("en-US")),
                CurrentAirportId = 1,
                CurrentAirport = airport,
                Msn = DataConstants.DefaultPlanesMsn[0],
            };

            var plane2 = new Plane
            {
                SiteId = 1,
                Id = 2,
                Capacity = 300,
                SyncTime = TimeSpan.Parse("12:00", new CultureInfo("en-US")),
                DeliveryDate = new DateTime(2001, 2, 3, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true,
                LastFlightDate = DateTime.Now,
                SyncFlightDataTime = TimeSpan.Parse("13:00", new CultureInfo("en-US")),
                CurrentAirportId = 1,
                CurrentAirport = airport,
                Msn = DataConstants.DefaultPlanesMsn[1],
            };

            this.GetDbContext().Planes.Add(plane1);
            this.GetDbContext().Planes.Add(plane2);
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
#endif
        #region AbstractMockEntityFrameworkInMemory methods

        /// <inheritdoc />
        public override void InitDefaultData()
        {
#if BIA_FRONT_FEATURE
            this.InitDefaultSites();

            // Begin BIADemo
            this.InitDefaultPlanes();

            // End BIADemo
#endif
        }

        #endregion AbstractMockEntityFrameworkInMemory methods
    }
}
