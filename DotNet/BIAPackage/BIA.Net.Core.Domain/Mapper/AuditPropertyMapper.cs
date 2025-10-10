namespace BIA.Net.Core.Domain.Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    public class AuditPropertyMapper<TReferenceEntity, TLinkedEntity> : IAuditPropertyMapper
    {
        public Type LinkedEntityType => typeof(TLinkedEntity);
        public string ReferenceEntityPropertyName => Common.Helpers.PropertyMapper.GetPropertyName(this.ReferenceEntityProperty);
        public string ReferenceEntityPropertyIdentifierName => Common.Helpers.PropertyMapper.GetPropertyName(this.ReferenceEntityPropertyIdentifier);
        public string LinkedEntityPropertyDisplayName => Common.Helpers.PropertyMapper.GetPropertyName(this.LinkedEntityPropertyDisplay);

        public required Expression<Func<TReferenceEntity, object>> ReferenceEntityProperty { get; set; }
        public required Expression<Func<TReferenceEntity, object>> ReferenceEntityPropertyIdentifier { get; set; }
        public required Expression<Func<TLinkedEntity, object>> LinkedEntityPropertyDisplay { get; set; }
    }
}
