// BIADemo only
// <copyright file="PlanePlaneTypeAudit.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Entities
{
    using BIA.Net.Core.Domain.Audit;

    /// <summary>
    /// Audit entity for <see cref="PlaneType"/>.
    /// </summary>
    public class PlanePlaneTypeAudit : AuditEntity<PlanePlaneType, int>
    {
        /// <summary>
        /// Gets or sets the PlaneTypeId.
        /// </summary>
        public int PlaneTypeId { get; set; }

        /// <summary>
        /// Gets or sets the PlaneId.
        /// </summary>
        public int PlaneId { get; set; }

        /// <summary>
        /// Gets or sets the PlaneTypeTitle.
        /// </summary>
        public string PlaneTypeTitle { get; set; }

        /// <inheritdoc/>
        protected override void FillSpecificProperties(PlanePlaneType entity)
        {
            this.PlaneTypeTitle = entity.PlaneType.Title;
        }
    }
}
