// BIADemo only
// <copyright file="FlightModelBuilder.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    /// <summary>
    /// Class used to update the model builder for flight domain.
    /// </summary>
    public static class FlightModelBuilder
    {
        /// <summary>
        /// Create the model for projects.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            CreateFlightModel(modelBuilder);
        }

        /// <summary>
        /// Create the model for flights.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateFlightModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flight>()
                .HasOne(x => x.DepartureAirport)
                .WithMany()
                .HasForeignKey(x => x.DepartureAirportId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Flight>()
                .HasOne(x => x.ArrivalAirport)
                .WithMany()
                .HasForeignKey(x => x.ArrivalAirportId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}