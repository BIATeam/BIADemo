// <copyright file="ArchiveConfiguration.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration.WorkerFeature
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Configuration for archiving.
    /// </summary>
    public class ArchiveConfiguration
    {
        /// <summary>
        /// Indicates wiether archive is active or not.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The <see cref="List{ArchiveEntityConfiguration}"/> of entities archive configurations.
        /// </summary>
        public List<ArchiveEntityConfiguration> ArchiveEntityConfigurations { get; set; } = new List<ArchiveEntityConfiguration>();

        /// <summary>
        /// Configuration for entity archiving.
        /// </summary>
        public class ArchiveEntityConfiguration
        {
            /// <summary>
            /// Entity name.
            /// </summary>
            public string EntityName { get; set; }

            /// <summary>
            /// Target directory path to store the archives.
            /// </summary>
            public string TargetDirectoryPath { get; set; }

            /// <summary>
            /// Indicates weither the current entity archive configuration is valid.
            /// </summary>
            public bool IsValid => !string.IsNullOrWhiteSpace(this.EntityName) && !string.IsNullOrWhiteSpace(this.TargetDirectoryPath);

            /// <summary>
            /// Indicates weither the current entity archive configuration match with the provided entity type.
            /// </summary>
            /// <param name="entityType">The entity <see cref="Type"/> to compare.</param>
            /// <returns><see cref="bool"/>.</returns>
            public bool IsMatchingEntityType(Type entityType) => this.EntityName.Equals(entityType.Name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
