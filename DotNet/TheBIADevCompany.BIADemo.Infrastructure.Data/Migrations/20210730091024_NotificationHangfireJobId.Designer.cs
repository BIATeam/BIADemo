﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TheBIADevCompany.BIADemo.Infrastructure.Data;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20210730091024_NotificationHangfireJobId")]
    partial class NotificationHangfireJobId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BIA.Net.Core.Domain.DistCacheModule.Aggregate.DistCache", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(449)
                        .HasColumnType("nvarchar(449)");

                    b.Property<DateTimeOffset?>("AbsoluteExpiration")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("ExpiresAtTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<long?>("SlidingExpirationInSeconds")
                        .HasColumnType("bigint");

                    b.Property<byte[]>("Value")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExpiresAtTime")
                        .HasDatabaseName("Index_ExpiresAtTime");

                    b.ToTable("DistCache");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("JobId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NotifiedRoleId")
                        .HasColumnType("int");

                    b.Property<bool>("Read")
                        .HasColumnType("bit");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<int>("SiteId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("NotifiedRoleId");

                    b.HasIndex("SiteId");

                    b.HasIndex("TypeId");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate.NotificationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("NotificationType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = "Task"
                        },
                        new
                        {
                            Id = 2,
                            Code = "Info"
                        },
                        new
                        {
                            Id = 3,
                            Code = "Success"
                        },
                        new
                        {
                            Id = 4,
                            Code = "Warning"
                        },
                        new
                        {
                            Id = 5,
                            Code = "Error"
                        });
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate.NotificationUser", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("NotificationId")
                        .HasColumnType("int");

                    b.Property<bool>("Read")
                        .HasColumnType("bit");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("UserId", "NotificationId");

                    b.HasIndex("NotificationId");

                    b.ToTable("NotificationUser");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate.Airport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("Airports");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate.Plane", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<DateTime>("FirstFlightDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastFlightDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Msn")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<int?>("PlaneTypeId")
                        .HasColumnType("int");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<int>("SiteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.HasKey("Id");

                    b.HasIndex("PlaneTypeId");

                    b.HasIndex("SiteId");

                    b.ToTable("Planes");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate.PlaneAirport", b =>
                {
                    b.Property<int>("PlaneId")
                        .HasColumnType("int");

                    b.Property<int>("AirportId")
                        .HasColumnType("int");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("PlaneId", "AirportId");

                    b.HasIndex("AirportId");

                    b.ToTable("PlaneAirport");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate.PlaneType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CertificationDate")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.ToTable("PlanesTypes");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.SiteModule.Aggregate.Site", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("Sites");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<int>("SiteId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SiteId");

                    b.HasIndex("UserId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate.MemberRole", b =>
                {
                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("MemberId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("MemberRole");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LabelEn")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LabelEs")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LabelFr")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = "Site_Admin",
                            LabelEn = "Site Admin"
                        });
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Company")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Country")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("DaiDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Department")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("DistinguishedName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Domain")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)")
                        .HasDefaultValue("--");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ExternalCompany")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("Guid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEmployee")
                        .HasColumnType("bit");

                    b.Property<bool>("IsExternal")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastLoginDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Manager")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Office")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Sid")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("--");

                    b.Property<string>("Site")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SubDepartment")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Login", "Domain")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.ViewModule.Aggregate.View", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Preference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("TableId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ViewType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Views");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.ViewModule.Aggregate.ViewSite", b =>
                {
                    b.Property<int>("SiteId")
                        .HasColumnType("int");

                    b.Property<int>("ViewId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("SiteId", "ViewId");

                    b.HasIndex("ViewId");

                    b.ToTable("ViewSite");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.ViewModule.Aggregate.ViewUser", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("ViewId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("UserId", "ViewId");

                    b.HasIndex("ViewId");

                    b.ToTable("ViewUser");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate.Notification", b =>
                {
                    b.HasOne("TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate.Role", "NotifiedRole")
                        .WithMany()
                        .HasForeignKey("NotifiedRoleId");

                    b.HasOne("TheBIADevCompany.BIADemo.Domain.SiteModule.Aggregate.Site", "Site")
                        .WithMany()
                        .HasForeignKey("SiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate.NotificationType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");

                    b.Navigation("NotifiedRole");

                    b.Navigation("Site");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate.NotificationUser", b =>
                {
                    b.HasOne("TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate.Notification", "Notification")
                        .WithMany("NotificationUsers")
                        .HasForeignKey("NotificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate.User", "User")
                        .WithMany("NotificationUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Notification");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate.Plane", b =>
                {
                    b.HasOne("TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate.PlaneType", "PlaneType")
                        .WithMany()
                        .HasForeignKey("PlaneTypeId");

                    b.HasOne("TheBIADevCompany.BIADemo.Domain.SiteModule.Aggregate.Site", "Site")
                        .WithMany()
                        .HasForeignKey("SiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlaneType");

                    b.Navigation("Site");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate.PlaneAirport", b =>
                {
                    b.HasOne("TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate.Airport", "Airport")
                        .WithMany("ClientPlanes")
                        .HasForeignKey("AirportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate.Plane", "Plane")
                        .WithMany("ConnectingAirports")
                        .HasForeignKey("PlaneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Airport");

                    b.Navigation("Plane");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate.Member", b =>
                {
                    b.HasOne("TheBIADevCompany.BIADemo.Domain.SiteModule.Aggregate.Site", "Site")
                        .WithMany("Members")
                        .HasForeignKey("SiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate.User", "User")
                        .WithMany("Members")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Site");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate.MemberRole", b =>
                {
                    b.HasOne("TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate.Member", "Member")
                        .WithMany("MemberRoles")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate.Role", "Role")
                        .WithMany("MemberRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.ViewModule.Aggregate.ViewSite", b =>
                {
                    b.HasOne("TheBIADevCompany.BIADemo.Domain.SiteModule.Aggregate.Site", "Site")
                        .WithMany("ViewSites")
                        .HasForeignKey("SiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheBIADevCompany.BIADemo.Domain.ViewModule.Aggregate.View", "View")
                        .WithMany("ViewSites")
                        .HasForeignKey("ViewId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Site");

                    b.Navigation("View");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.ViewModule.Aggregate.ViewUser", b =>
                {
                    b.HasOne("TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate.User", "User")
                        .WithMany("ViewUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheBIADevCompany.BIADemo.Domain.ViewModule.Aggregate.View", "View")
                        .WithMany("ViewUsers")
                        .HasForeignKey("ViewId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("View");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate.Notification", b =>
                {
                    b.Navigation("NotificationUsers");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate.Airport", b =>
                {
                    b.Navigation("ClientPlanes");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate.Plane", b =>
                {
                    b.Navigation("ConnectingAirports");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.SiteModule.Aggregate.Site", b =>
                {
                    b.Navigation("Members");

                    b.Navigation("ViewSites");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate.Member", b =>
                {
                    b.Navigation("MemberRoles");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate.Role", b =>
                {
                    b.Navigation("MemberRoles");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate.User", b =>
                {
                    b.Navigation("Members");

                    b.Navigation("NotificationUsers");

                    b.Navigation("ViewUsers");
                });

            modelBuilder.Entity("TheBIADevCompany.BIADemo.Domain.ViewModule.Aggregate.View", b =>
                {
                    b.Navigation("ViewSites");

                    b.Navigation("ViewUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
