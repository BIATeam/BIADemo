namespace BIA.Net.Core.Domain.Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    public class LinkedAuditMapper<TReferenceEntity, TLinkedAuditEntity> : ILinkedAuditMapper
    {
        public Type LinkedAuditEntityType => typeof(TLinkedAuditEntity);
        public string ReferenceEntityPropertyName => Common.Helpers.PropertyMapper.GetPropertyName(this.ReferenceEntityProperty);
        public string LinkedAuditEntityDisplayPropertyName => Common.Helpers.PropertyMapper.GetPropertyName(this.LinkedAuditEntityDisplayProperty);
        public string LinkedAuditEntityIdentifierPropertyName => Common.Helpers.PropertyMapper.GetPropertyName(this.LinkedAuditEntityIdentifierProperty);

        public required Expression<Func<TReferenceEntity, object>> ReferenceEntityProperty { get; set; }
        public required Expression<Func<TLinkedAuditEntity, object>> LinkedAuditEntityDisplayProperty { get; set; }
        public required Expression<Func<TLinkedAuditEntity, object>> LinkedAuditEntityIdentifierProperty { get; set; }
    }
}
