// <copyright file="DateTimeConversionDbContextOptionsExtensionBase.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.DateTimeConversion
{
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Base class for DateTime conversion EF Core options extension.
    /// </summary>
    public abstract class DateTimeConversionDbContextOptionsExtensionBase : IDbContextOptionsExtension
    {
        private DbContextOptionsExtensionInfo info;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeConversionDbContextOptionsExtensionBase"/> class.
        /// </summary>
        /// <param name="serviceLifetime">The service lifetime for the registered services.</param>
        protected DateTimeConversionDbContextOptionsExtensionBase(ServiceLifetime serviceLifetime)
        {
            this.ServiceLifetime = serviceLifetime;
        }

        /// <inheritdoc/>
        public DbContextOptionsExtensionInfo Info => this.info ??= new ExtensionInfo(this);

        /// <summary>
        /// Gets the service lifetime.
        /// </summary>
        protected ServiceLifetime ServiceLifetime { get; }

        /// <inheritdoc/>
        public abstract void ApplyServices(IServiceCollection services);

        /// <inheritdoc/>
        public void Validate(IDbContextOptions options)
        {
            // No validation needed
        }

        private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
        {
            public ExtensionInfo(IDbContextOptionsExtension extension)
                : base(extension)
            {
            }

            public override bool IsDatabaseProvider => false;

            public override string LogFragment => "using DateTimeConversion";

            public override int GetServiceProviderHashCode() => this.Extension.GetType().GetHashCode();

            public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
                => other is ExtensionInfo;

            public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
            {
                debugInfo["DateTimeConversion"] = "1";
            }
        }
    }
}
